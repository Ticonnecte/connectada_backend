using System.Collections.Generic;

namespace CidConnectada.Entities.Model.Dto.Location
{
    public class PlaceAutoCompleteResponseDto
    {
        public IList<PlaceAutoCompletePredictionsDto> predictions { get; set; } = new List<PlaceAutoCompletePredictionsDto>();
    }
}