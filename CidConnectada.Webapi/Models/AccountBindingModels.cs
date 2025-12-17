using CidConnectada.Entities.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CidConnectada.Webapi.Models
{
    // Models used as parameters to AccountController actions.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "senha atual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "nova senha")]
        public string NewPassword { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required][Display(Name = "Email")] public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        public int? userKey { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "nova senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirmação de senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha não está a confirmação.")]
        public string ConfirmPassword { get; set; }
    }

    public class SessionDto
    {
        [Required]
        public string userName { get; set; }
    }

    public class SendVerificationCodeDto : SessionDto
    {
        [Required]
        public ServicoEnvioMsgEnum servicoEnvioMsg { get; set; }
    }

    public class VerifyAccountModel
    {
        [Required]
        public string userName { get; set; }

        [Required][ValidateGuid] public Guid deviceId { get; set; }

        [Required] public string deviceName { get; set; }

        public string deviceType { get; set; }

        [Required] public string code { get; set; }
    }

    public class ResetPasswordDto : VerifyAccountModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "nova senha")]
        public string newPassword { get; set; }
    }
}