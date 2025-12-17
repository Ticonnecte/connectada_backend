using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using CidConnectada.Dao.Account;
using CidConnectada.Dao.Organograma;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Identity;
using CidConnectada.Entities.Model.Organograma;
using CidConnectada.Services.Impl.Identity;
using CidConnectada.Services.Intf.Messaging;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Zenite.Pi.Context;
using Zenite.Pi.Entities.Model.Account;
using Zenite.Pi.Exceptions;
using Zenite.Pi.IoC;
using Zenite.Pi.Services.Impl;
using Zenite.Wa.Models.Zapi.Message;

namespace CidConnectada.Services.Impl.Account
{
    public abstract class UsuarioGenericService<TEntity, TDao> : CadastroBaseService<TEntity, TDao, int, int, string>
        where TEntity : Usuario
        where TDao : UsuarioGenericDao<TEntity>
    {
        public UsuarioGenericService(TDao _cadDao,
            Func<ContextRequest<int, string>> contextFactory,
            AspNetUsersDao aspNetUsersDao,
            AspNetRolesDao aspNetRolesDao,
            RefreshTokenDao refreshTokenDao,
            DeviceDao deviceDao,
            PrefeituraDao prefeituraDao,
            VerificacaoContaDao verificacaoContaDao,
            IZApiService zApiService,
            Func<ApplicationUserManager> userManagerFactory
        ) : base(_cadDao, contextFactory)
        {
            AspNetUsersDao = aspNetUsersDao;
            AspNetRolesDao = aspNetRolesDao;
            RefreshTokenDao = refreshTokenDao;
            DeviceDao = deviceDao;
            PrefeituraDao = prefeituraDao;
            VerificacaoContaDao = verificacaoContaDao;
            ZApiService = zApiService;
            _userManagerFactory = userManagerFactory;
        }

        #region Daos-Servicos

        private readonly AspNetUsersDao AspNetUsersDao;
        private readonly AspNetRolesDao AspNetRolesDao;
        private readonly RefreshTokenDao RefreshTokenDao;
        private readonly DeviceDao DeviceDao;
        private readonly PrefeituraDao PrefeituraDao;
        private readonly VerificacaoContaDao VerificacaoContaDao;
        private readonly IZApiService ZApiService;
        private readonly Func<ApplicationUserManager> _userManagerFactory;
        protected virtual ApplicationUserManager UserManager => _userManagerFactory();

        #endregion

        #region CRUD

        public override string GetNomeEntidade(int indexDetail = 0)
        {
            return "Usuario";
        }

        public override object GetValorCampoDescritivoPadrao(TEntity entity)
        {
            return $"UserName: {entity.AspNetUsers.Username}";
        }

        protected override Expression<Func<TEntity, bool>> GetUnicidadeFilter(TEntity entity)
        {
            // return e => e.AspNetUsers.Username == entity.AspNetUsers.Username
            //     && e.Key != entity.Key;

            IList<TEntity> list = cadDao.Where(e => (e.AspNetUsers.Username == entity.AspNetUsers.Username
                || e.Cpf == entity.Cpf
                || e.Rg == entity.Rg
                || e.AspNetUsers.PhoneNumber == entity.AspNetUsers.PhoneNumber
                || e.AspNetUsers.Email == entity.AspNetUsers.Email)
                && e.Key != entity.Key && e.TenantKey == entity.TenantKey).ToList();

            string excMessage = "Ja existe um usuário com:";
            int excCount = 0;

            if (entity.Cpf != null && list.Any(e => e.Cpf == entity.Cpf))
            {
                excMessage += $" Cpf: {entity.Cpf}";
                excCount++;
            }

            if (entity.Rg != null && list.Any(e => e.Rg == entity.Rg))
            {
                excMessage += $" Rg: {entity.Rg}";
                excCount++;
            }

            if (entity.AspNetUsers.PhoneNumber != null && list.Any(e => e.AspNetUsers.PhoneNumber == entity.AspNetUsers.PhoneNumber))
            {
                excMessage += $" Telefone: {entity.AspNetUsers.PhoneNumber}";
                excCount++;
            }

            if (entity.AspNetUsers.Email != null && list.Any(e => e.AspNetUsers.Email == entity.AspNetUsers.Email))
            {
                excMessage += $" Email: {entity.AspNetUsers.Email}";
                excCount++;
            }

            if (excCount == 0)
            {
                return e => false;
            }

            throw new PiBusinessException(excMessage);
        }

        public async Task<TEntity> IncluirAsync(TEntity entity, ApplicationUser appUser, string password)
        {
            //TODO: remover senha do post de FUNCIONARIO e ADMIN para ser gerada aqui
            
            IdentityResult result = await UserManager.CreateAsync(appUser, password);
            
            if (!result.Succeeded)
                throw new PiBusinessException(result.Errors.ToList());

            entity.Status = UserStatusEnum.New;
            var aspNetUsers = await AspNetUsersDao.FindByKeyPlusAsync(
                new string[1] {"AspNetUserRolesSet.AspNetRoles"}, appUser.Id);
            entity.AspNetUsers = aspNetUsers;
            
            return await base.IncluirAsync(entity);
            
            //TODO: após incluir FUNCIONARIO ou ADMIN enviar credenciais por email.
        }
        
        protected override async Task AlterarDynamic(TEntity entity, bool async = false)
        {
            IdentityResult identityResult = null;
            
            if (entity is Cidadao && Context.CacheRequest.TryGetValue("email", out object email))
                identityResult = await UserManager.SetEmailAsync(entity.AspNetUsers.Key, (string)email);
            
            if (!(entity is Cidadao) && Context.CacheRequest.TryGetValue("telefone", out object telefone))
                identityResult = await UserManager.SetPhoneNumberAsync(entity.AspNetUsers.Key, (string)telefone);
            
            if (identityResult != null && !identityResult.Succeeded)
                throw new PiBusinessException(identityResult.Errors.ToList());
            
            await base.AlterarDynamic(entity, async);
        }

        protected override async Task ExcluirDynamic(TEntity entity, bool async = false)
        {
            if (entity.AspNetUsers == null)
                entity = await cadDao.FindByKeyPlusAsync(new string[1] { "AspNetUsers" }, entity.Key);
            
            ApplicationUser appUser = await UserManager.FindByIdAsync(entity.AspNetUsers.Key);
            
            await base.ExcluirDynamic(entity, async);
            await UserManager.DeleteAsync(appUser);
        }

        #endregion

        #region Custom

        public async Task IncluirAdminAsync(TEntity entity, ApplicationUser appUser)
        {
            string password = GerarSenhaForte();
            entity = await IncluirAsync(entity, appUser, password);
            await EnviarCredenciais(entity, password);
        }

        public async Task DeleteIfPhoneNotConfirmedAsync(string username)
        {
            Usuario user = cadDao.FindByUsername(username);
            
            if (user != null && !user.AspNetUsers.Phonenumberconfirmed)
            {
                await AspNetUsersDao.DeleteByKeyAsync(user.AspNetUsers.Key);
                await cadDao.DeleteByKeyAsync(user.Key);
            }
        }

        public TEntity GetPrincipal()
        {
            return cadDao.FirstOrDefault(u => u.IndPrincipal.HasValue && u.IndPrincipal.Value);
            //return await cadDao.FirstOrDefaultAsync(u => u.IndPrincipal.HasValue && u.IndPrincipal.Value);
        }
        
        //public override void SetValoresPadroes(User entity, OperacaoEntidadeEnum operacao)
        //{
        //    if (operacao == OperacaoEntidadeEnum.Incluir)
        //    {
        //        entity.OrgaoNome = entity.Orgao.Pessoa.Nome;
        //        entity.SecretariaNome = entity.Secretaria.Pessoa.Nome;
        //    }
        //    base.SetValoresPadroes(entity, operacao);
        //}

        public virtual IList<Tuple<UserStatusEnum, UserStatusEnum>> AllowedTransactions
        {
            get
            {
                IList<Tuple<UserStatusEnum, UserStatusEnum>> result = new List<Tuple<UserStatusEnum, UserStatusEnum>>
                {
                    new Tuple<UserStatusEnum, UserStatusEnum>(UserStatusEnum.New, UserStatusEnum.Active),
                    new Tuple<UserStatusEnum, UserStatusEnum>(UserStatusEnum.New, UserStatusEnum.Deleted),

                    new Tuple<UserStatusEnum, UserStatusEnum>(UserStatusEnum.Active, UserStatusEnum.Banned),
                    new Tuple<UserStatusEnum, UserStatusEnum>(UserStatusEnum.Active, UserStatusEnum.Deleted)
                };

                return result;
            }
        }

        public async Task<IList<AspNetRoles>> GetRolesListAsync()
        {
            return await AspNetRolesDao.AllAsync();
        }

        public string GetRoleIdByName(string roleName)
        {
            return AspNetRolesDao.FirstOrDefault(r => r.Name == roleName).Key;
        }

        public async Task<AspNetUsers> GetAspNetUsers(string key)
        {
            return await AspNetUsersDao.FindByKeyAsync(key);
        }

        public TEntity FindByUsername(string userName)
        {
            return cadDao.FindByUsername(userName);
        }

        public async Task<IList<TEntity>> GetByRole(string id)
        {
            return await cadDao.Where(u =>
                u.AspNetUsers.AspNetUserRolesSet.Any(ur => ur.RoleId == id)).ToListAsync();
        }

        #endregion

        #region AccountService

        public void AddUser(IHieUser<string> user)
        {
            Add((TEntity)user);
        }

        public void DeleteUser(string operationKey)
        {
            var entity = (TEntity)ObterUser(operationKey);
            Excluir(entity);
        }

        public string GetMasterRole()
        {
            return "ADMIN";
        }

        public IHieUser<string> ObterUser(string operationKey)
        {
            return cadDao.ObterUser(operationKey);
        }

        public void SetTenantToCurrentUser(int tenantKey)
        {
            Prefeitura tenant = PrefeituraDao.FindByKey(tenantKey);
            TEntity entity = (TEntity)Context.User;
            entity.Prefeitura = tenant;
            Alterar(entity);
        }

        #endregion
        
        #region Messaging

        protected async Task SendEmail(Usuario user, MailMessage message)
        {
            using (var smtp = new SmtpClient(ApplicationContext.AppSettings["SMTP:Servidor"]))
            {
                string emailHie = ApplicationContext.AppSettings["SMTP:Email"];
                string secretHie = ApplicationContext.AppSettings["SMTP:Secret"];

                message.From = new MailAddress(emailHie);
                smtp.Credentials = new NetworkCredential(emailHie, secretHie);
                smtp.EnableSsl = true;

                message.To.Add(user.AspNetUsers.Email);
                await smtp.SendMailAsync(message);
            }
        }
        
        public async Task EnviarCredenciais(Usuario entity, string password)
        {
            string hiEContato = ApplicationContext.AppSettings["hiE:Contato"];
            string perfil = entity.AspNetUsers.AspNetUserRolesSet.First().AspNetRoles.Name;

            var mail = new MailMessage
            {
                Subject = "Boas-vindas ao Cidade Conectada: Suas Credenciais de Acesso",
                Body = $@"<p>Prezado(a) {entity.Nome} {entity.Sobrenome},</p>

                    <p>É com grande satisfação que damos as boas-vindas à Prefeitura de {entity.Prefeitura.Nome} ao <strong>Cidade Conectada</strong>!</p>

                    <p>Este e-mail contém suas credenciais de acesso temporárias para a conta de <strong>{perfil}</strong> da sua prefeitura. 
                    Com este perfil, você terá controle total sobre todas as funcionalidades habilitadas para este perfil no domínio a seguir.</p>

                    <p><strong>Detalhes de Acesso:</strong><br />
                    <strong>URL:</strong> <code>https://{entity.Prefeitura.Dominio}.connectada.hie.tec.br</code><br />
                    <strong>E-mail:</strong> <code>{entity.AspNetUsers.Email}</code><br />
                    <strong>Senha Temporária:</strong> <code>{password}</code></p>

                    <p><strong>Atenção:</strong> Por questões de segurança, é <strong>fundamental</strong> que você altere sua senha temporária no primeiro acesso ao sistema. 
                    Isso garantirá a proteção de seus dados e do ambiente da prefeitura.</p>

                    <p>Para qualquer dúvida ou necessidade de suporte, nossa equipe está à disposição. 
                    Por favor, entre em contato através do e-mail: <strong>{hiEContato}</strong>.</p>

                    <p>Estamos entusiasmados em tê-los conosco e esperamos que o Cidade Conectada seja uma ferramenta poderosa para a gestão municipal de {entity.Prefeitura.Nome}!</p>

                    <p>Atenciosamente,<br />
                    Equipe hie Tec</p>
                    ",
                IsBodyHtml = true
            };

            await SendEmail(entity, mail);
        }

        #endregion
        
        #region AccountVerification

        private string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        private async Task StoreVerificationCode(Usuario user, string code)
        {
            Guid verificationId = Guid.NewGuid();
            var verification = new VerificacaoConta
            {
                Key = verificationId,
                Usuario = user,
                Codigo = code,
                DataExpiracaoUtc = DateTime.UtcNow.AddMinutes(15)
            };

            VerificacaoConta oldVerification = await VerificacaoContaDao.FirstOrDefaultAsync(v => v.Usuario.Key == user.Key);

            if (oldVerification != null)
            {
                VerificacaoContaDao.Delete(oldVerification);
            }

            VerificacaoContaDao.Add(verification);
        }

        public async Task SendVerificationCodeAsync(Usuario user, ServicoEnvioMsgEnum srvMsg)
        {
            string code = "123456"; //GenerateVerificationCode();
            await StoreVerificationCode(user, code);

            if (srvMsg == ServicoEnvioMsgEnum.WhatsApp)
                code = $"*{code}*";

            string title = "Código de verificação CidadeConectada";
            string message = $"Seu código de verificação para o CidadeConectada é: {code}." + "\nEste código tem validade de 15 minutos.";

            switch (srvMsg)
            {
                case ServicoEnvioMsgEnum.Email:
                    var mail = new MailMessage
                    {
                        Subject = title,
                        Body = message,
                        IsBodyHtml = false
                    };

                    await SendEmail(user, mail);
                    break;

                case ServicoEnvioMsgEnum.WhatsApp:
                    ZApiSendTextDto dto = new ZApiSendTextDto
                    {
                        phone = user.AspNetUsers.PhoneNumber,
                        message = message
                    };

                    await ZApiService.SendMessageAsync(dto);
                    break;
                //throw new ArgumentOutOfRangeException(nameof(srvMsg), srvMsg, null);
            }
        }

        #endregion

        #region RefreshToken

        public RefreshToken CreateRefreshToken(RefreshToken token)
        {
            return RefreshTokenDao.Add(token);
        }

        public async Task<RefreshToken> FindRefreshTokenAsync(Guid id)
        {
            return await RefreshTokenDao.FindByKeyPlusAsync(Context.CancelToken, RefreshTokenDao.DefaultIncludes, id);
        }

        public async Task<RefreshToken> FindRefreshTokenAsync(Guid deviceId, long userId)
        {
            return await RefreshTokenDao.SingleOrDefaultAsync(t =>
                t.Device.Key == deviceId && t.User.Key == userId);
        }

        public async Task<IList<RefreshToken>> FindRefreshTokensAsync(Guid deviceId)
        {
            return await RefreshTokenDao.Where(t => t.Device.Key == deviceId).ToListAsync();
        }

        public async Task RemoveRefreshTokenAsync(Guid id)
        {
            await RefreshTokenDao.DeleteByKeyAsync(id);
        }

        public async Task RemoveRefreshTokenAsync(IList<RefreshToken> tokens)
        {
            foreach (var token in tokens) await RefreshTokenDao.DeleteByKeyAsync(token.Key);
        }

        #endregion
        
        #region Device
        
        public Device FindDevice(Guid id)
        {
            Device result = DeviceDao.FindByKey(id);

            if (result == null)
                throw new PiBusinessException("Dispositivo não encontrado no sistema.");

            return result;
        }
        
        public async Task<Device> FindDeviceAsync(Guid id)
        {
            return await DeviceDao.FindByKeyPlusAsync(Context.CancelToken, RefreshTokenDao.DefaultIncludes, id);
        }
        
        public void AddDevice(Device device)
        {
            DeviceDao.Add(device);
        }

        public async Task RemoveDeviceAsync(Device device)
        {
            await DeviceDao.DeleteByKeyAsync(device.Key);
        }
        
        #endregion
        
        #region Password
        
        public string GerarSenhaForte(int length = 12)
        {
            if (length < 8)
                throw new ArgumentException("O comprimento da senha deve ser pelo menos 8 caracteres.");
            
            string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string Lowercase = "abcdefghijklmnopqrstuvwxyz";
            string Digits = "0123456789";
            string Special = "!@#$%^&*()-_=+[]{}|;:,.<>?";

            var randomChars = new List<char>
            {
                GetRandomChar(Uppercase),
                GetRandomChar(Lowercase),
                GetRandomChar(Digits),
                GetRandomChar(Special)
            };

            string allChars = Uppercase + Lowercase + Digits + Special;

            for (int i = randomChars.Count; i < length; i++)
            {
                randomChars.Add(GetRandomChar(allChars));
            }

            return Shuffle(randomChars);
        }

        private static char GetRandomChar(string chars)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[1];
                char selectedChar;
                do
                {
                    rng.GetBytes(buffer);
                    int index = buffer[0] % chars.Length;
                    selectedChar = chars[index];
                }
                while (!chars.Contains(selectedChar));
                return selectedChar;
            }
        }

        private static string Shuffle(List<char> chars)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                int n = chars.Count;
                while (n > 1)
                {
                    byte[] box = new byte[1];
                    do
                    {
                        rng.GetBytes(box);
                    } while (!(box[0] < n * (Byte.MaxValue / n)));

                    int k = (box[0] % n);
                    n--;
                    (chars[n], chars[k]) = (chars[k], chars[n]);
                }
            }
            return new string(chars.ToArray());
        }
        
        #endregion

    }
}