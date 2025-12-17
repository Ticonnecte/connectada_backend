using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Local;
using CidConnectada.Entities.Model.Saude;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zenite.Pi.Entities.Model.Account;
using Zenite.Pi.Entities.Model.MultiTenancy;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Organograma
{
    public class Prefeitura : BaseTenant<int>, IEquatable<Prefeitura>
    {
        [Required(AllowEmptyStrings = false)]
        public string Nome { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Dominio { get; set; }

        #region AWS-S3
        public string BucketName { get; set; }
        public string S3Region { get; set; }
        public string S3AccessKeyId { get; set; }
        public string S3AccessKeySecret { get; set; }
        #endregion

        #region Base64

        [NotMapped]
        public string Base64LogoHeader { get; set; }

        [NotMapped]
        public string Base64LogoHorizontal { get; set; }

        [NotMapped]
        public string Base64LogoVertical { get; set; }

        #endregion

        public string ZApiIdInstancia { get; set; }
        public string ZApiToken { get; set; }
        public string ZApiClientToken { get; set; }

        public string GoogleMapsApiKey { get; set; }

        public string PrimaryMainColor { get; set; }
        public string PrimaryDarkColor { get; set; }
        public string PrimaryLightColor { get; set; }
        public string SecondaryMainColor { get; set; }
        public string SecondaryDarkColor { get; set; }
        public string SecondaryLightColor { get; set; }

        public string LogoHeaderUrl { get; set; }
        public string LogoHorizontalUrl { get; set; }
        public string LogoVerticalUrl { get; set; }

        public string Facebook { get; set; }
        public string Youtube { get; set; }
        public string Instagram { get; set; }
        public string Site { get; set; }


        public Endereco Endereco { get; set; }
        public ISet<Secretaria> SecretariaSet { get; set; }

        public ISet<UnidadeBasicaSaude> UnidadeBasicaSaudeSet {  get; set; }

        [NotMapped]
        public override string Name
        {
            get => Nome;
            set => Nome = value;
        }
        public string S3BaseUrl => $"https://{BucketName}.s3.{S3Region}.amazonaws.com/";

        public virtual string GetS3Key(LogoPrefeituraEnum logoEnum, string extensao)
        {
            string result = "";
            if (string.IsNullOrEmpty(extensao))
            {
                switch (logoEnum)
                {
                    case LogoPrefeituraEnum.Header:
                        result = LogoHeaderUrl.Replace(S3BaseUrl, "");
                        break;
                    case LogoPrefeituraEnum.Horizontal:
                        result = LogoHorizontalUrl.Replace(S3BaseUrl, "");
                        break;
                    case LogoPrefeituraEnum.Vertical:
                        result = LogoVerticalUrl.Replace(S3BaseUrl, "");
                        break;
                }
            }
            else
            {
                result = $"logos/{Enum.GetName(typeof(LogoPrefeituraEnum), logoEnum).ToLower()}.{extensao}";
            }
            return result;
        }

        public virtual string GetS3Url(LogoPrefeituraEnum logoEnum, string extensao)
        {
            return $"{S3BaseUrl}{GetS3Key(logoEnum, extensao)}";
        }

        public bool Equals(Prefeitura other)
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

        public override string GetDomain()
        {
            return Dominio;
        }

        public override bool IsAuthorized<TUserOperationKey>(IHieUser<TUserOperationKey> user)
        {
            throw new NotImplementedException();
        }
    }
}