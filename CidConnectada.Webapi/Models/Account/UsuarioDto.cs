using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Zenite.Pi.Web.Models;
using Zenite.Pi.Web.Models.Pesquisa;

namespace CidConnectada.Webapi.Models.Account
{
    public class UsuarioDto : BaseEntityModel<int>
    {
        [StringLength(11, ErrorMessage = "CPF deve ter 11 caracteres numéricos", MinimumLength = 11)]
        protected string _cpf;
        protected string _telefone;
        protected string _rg { get; set; }
        
        public virtual int tenantId { get; set; }
        [Display(Name = "Email")]
        public virtual string email { get; set; }

        [Display(Name = "Perfil")]
        public virtual IList<piLookupModel<string>> rolesList { get; set; } = new List<piLookupModel<string>>();

        //[Required]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [IgnoreMap]
        [Display(Name = "senha")]
        public string password { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string orgaoExpedidor { get; set; }
        public bool aceitaMsgWhastApp { get; set; }
        public virtual string telefone
        {
            get => _telefone;
            set => _telefone = RemoverCaracteresNaoNumericos(value);
        }

        public string rg
        {
            get => _rg;
            set => _rg = RemoverCaracteresNaoNumericos(value);
        }
        public string cpf
        {
            get => _cpf;
            set => _cpf = RemoverCaracteresNaoNumericos(value);
        }
        
        protected static string RemoverCaracteresNaoNumericos(string input)
        {
            return String.IsNullOrEmpty(input) ? "" : String.Concat(input.Where(Char.IsDigit));
        }
    }
}