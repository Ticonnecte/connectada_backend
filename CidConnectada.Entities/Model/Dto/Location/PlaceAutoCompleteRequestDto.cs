namespace CidConnectada.Entities.Model.Dto.Location
{
    public class PlaceAutoCompleteRequestDto
    {
        public string input { get; set; }
        public string sessionToken { get; set; }
        public LocationBias locationBias { get; set; }
    }
}