using System.Collections.Generic;

namespace CidConnectada.Entities.Model.Dto.Google.PlaceAutoComplete
{
    public class PlaceAutoCompleteResultDto
    {
        public IList<PredictionDto> predictions { get; set; }
        public string status { get; set; }
    }
}