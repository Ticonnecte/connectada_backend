using AutoMapper;
using CidConnectada.Entities.Model.Banners;
using CidConnectada.Entities.Model.Comunicacao;
using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Noticias;
using CidConnectada.Entities.Model.Notificacao;
using CidConnectada.Entities.Model.Organograma;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Zenite.Pi.Entities.Model.Account;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Account
{
    public class Usuario : HieUserTenant<int, string, int>, IEquatable<Usuario>
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string OrgaoExpedidor { get; set; }

        public UserStatusEnum Status { get; set; }
        public bool ConcordaTermosDeUso { get; set; }
        public bool AceitaMsgWhastApp { get; set; }

        public bool? IndPrincipal { get; set; }

       

        [IgnoreMap]
        public virtual AspNetUsers AspNetUsers { get; set; }
        public Prefeitura Prefeitura { get; set; }

        public virtual VerificacaoConta VerificacaoConta { get; set; }
        public virtual ISet<WaNotificacaoStatus> WaNotificacaoStatusSet { get; set; } = new HashSet<WaNotificacaoStatus>();
        public virtual ISet<RefreshToken> RefreshTokenSet { get; set; } = new HashSet<RefreshToken>();
        public virtual ISet<EnvioNoticia> EnvioNoticiaSet { get; set; } = new HashSet<EnvioNoticia>();
        public virtual ISet<ExpoNotificationToken> ExpoNotificationTokenSet { get; set; } = new HashSet<ExpoNotificationToken>();
        public virtual ISet<NotificationUnicast> NotificationUnicastSet { get; set; } = new HashSet<NotificationUnicast>();
        public virtual ISet<NotificationMulticastUser> NotificationMulticastUserSet { get; set; } = new HashSet<NotificationMulticastUser>();
        public virtual ISet<EnqueteResposta> EnqueteRespostaSet { get; set; } = new HashSet<EnqueteResposta>();
        public ISet<NoticiaLog> NoticiaLogSet { get; set; } = new HashSet<NoticiaLog>();
        public ISet<Banner> BannerSet { get; set; } = new HashSet<Banner>();
        public bool Equals(Usuario other)
        {
            bool result;
            if (ReferenceEquals(other, null))
                result = false;
            else if (ReferenceEquals(other, this))
                result = true;
            else
                result = EntityUtil.EqualsEntity(this, other);

            return result;
        }

        #region HieUser

        [NotMapped]
        public override bool IsAdmin
        {
            get => AspNetUsers.AspNetUserRolesSet.Any(r => r.AspNetRoles.Name == "SA" || r.AspNetRoles.Name == "ADMIN");
        }

        [NotMapped]
        [IgnoreMap]
        public override string UserName
        {
            get => AspNetUsers.Username;
            set {}
        }

        public override string Domain
        {
            get => Prefeitura.Dominio;
        }

        public override string GetLanguage()
        {
            return "pt-BR";
        }

        public override string GetCurrentTenantName()
        {
            return Prefeitura.Name;
        }

        public override string GetOperationKey()
        {
            return AspNetUsers.Key;
        }

        #endregion
    }
}