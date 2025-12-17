using CidConnectada.Entities.Model.Infos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace CidConnectada.Entities.Model.AWS
{
    public abstract class HtmlContent : S3FileGeneric
    {
        public override string Key { get; set; }
        public string Lead { get; set; }
        public string Conteudo { get; set; }


        // Quando se adiciona um novo campo numa tabela que já tem dados,
        // ou vc coloca um valor padrão no campo dos registros já existentes, ou vc garante que a propriedade aceita valores nulos (bool?).

        //update NOTICIA
        //set ativa = 1
        
        [DefaultValue(true)]
        public bool? Ativa { get; set; }

        public string FotoCapaUrl
        {
            get
            {
                return _ImgUrl;
            }
            set
            {
                _ImgUrl = value;
            }
        }

        public abstract string UrlPart();
        public override string GetS3Key(string extensao = null)
        {
            string result = "";
            if (!string.IsNullOrEmpty(extensao))
            {
                result = $"{UrlPart()}/{Key}/capa.{extensao}";
            }
            else
            {
                result = base.GetS3Key();
            }
            return result;
        }

        public IList<TImages> GetNewImages<TImages>(string baseUrl, IList<string> imgSources)
            where TImages : HtmlImages
        {
            IList<TImages> result = new List<TImages>();
            Type typeImage = typeof(TImages);
            ConstructorInfo imageConstructor = typeImage.GetConstructor(new Type[] { typeof(int), typeof(string) });
            foreach (string imgSource in imgSources)
            {
                if (!imgSource.Contains("base64"))
                    continue;
                string fileExtension = imgSource.Split(';').First().Replace(@"data:image/", String.Empty);
                string base64 = imgSource.Split(';').Last().Replace("base64,", String.Empty);

                if (!string.IsNullOrEmpty(base64) && base64.Length >= 128)
                {
                    int hashCode = base64.Substring(0, 128).GetHashCode();
                    string s3KeyContent = $"{UrlPart()}/{this.Key}/imagem-{hashCode}.{fileExtension}";

                    string link = $"{baseUrl}{s3KeyContent}";
                    this.Conteudo = this.Conteudo.Replace(imgSource, link);
                    if (!result.Any(ii => ii.HashId == hashCode))
                    {
                        TImages image = (TImages)imageConstructor.Invoke(new object[] { hashCode, this.Key });
                        image.HashId = hashCode;
                        image.ParentId = this.Key;
                        image.ImgUrl = link;
                        image.Base64 = base64;
                        result.Add(image);
                    }
                }
            }
            return result;
        }

    }
}
