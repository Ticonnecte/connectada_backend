using CidConnectada.Entities.Model.Dto.Location;

namespace CidConnectada.Webapi.Models.Saude
{
    public class UbsViewDto : UbsBaseDto
    {
        public string tipoUnidadeEnumNome { get; set; }
        public string porteEnumNome { get; set; }
        public string regiaoAbrangenciaEnumNome { get; set; }
        public string situacaoEnumNome { get; set; }
        public string imagemUrl
        {
            get {
                return _imgUrl;
            }
            set {
                _imgUrl = value;
            }
        }
        public LocationDto coordenadas { get; set; }
    }
}