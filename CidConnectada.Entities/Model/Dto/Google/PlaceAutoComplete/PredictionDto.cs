namespace CidConnectada.Entities.Model.Dto.Google.PlaceAutoComplete
{
    public class PredictionDto
    {
        public string description { get; set; }
        public string place_id { get; set; }
        public StructuredFormattingDto structured_formatting { get; set; }
    }
}