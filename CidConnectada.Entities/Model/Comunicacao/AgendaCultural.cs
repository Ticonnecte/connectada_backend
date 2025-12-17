using CidConnectada.Entities.Model.AWS;
using System;
using Zenite.Pi.Util.Control;

namespace CidConnectada.Entities.Model.Comunicacao
{
    public class AgendaCultural : S3FileGeneric, IEquatable<AgendaCultural>
    {
        public override string Key { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string ImagemUrl
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
        public DateTime DhEventoInicio { get; set; }
        public DateTime DhEventoFinal { get; set; }
        public string Link { get; set; }

        public override string GetS3Key(string extensao = null)
        {
            string result = "";
            if (!string.IsNullOrEmpty(extensao))
            {
                result = $"agenda-cultural/{Key}/agendaCultutalImg.{extensao}";
            }
            else
            {
                result = base.GetS3Key();
            }
            return result;
        }

        public bool Equals(AgendaCultural other)
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
    }
}