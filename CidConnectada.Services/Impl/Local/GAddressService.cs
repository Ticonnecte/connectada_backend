using AutoMapper;
using CidConnectada.Entities.Model.Account;
using CidConnectada.Entities.Model.Dto.Google;
using CidConnectada.Entities.Model.Dto.Google.Geocoding;
using CidConnectada.Entities.Model.Dto.Google.PlaceAutoComplete;
using CidConnectada.Entities.Model.Dto.Google.PlaceDetails;
using CidConnectada.Entities.Model.Dto.Location;
using CidConnectada.Services.Intf.Local;
using CidConnectada.Services.Intf.Organograma;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using Zenite.Pi.Context;
using Zenite.Pi.Exceptions;
using Zenite.Pi.IoC;
using Zenite.Pi.Services.Impl;
using Zenite.Pi.Web.WebApi;

namespace CidConnectada.Services.Impl.Local
{
    public class GAddressService : BaseService<int, string>, IGAddressService
    {
        private const string BASE_URL = "https://maps.googleapis.com/maps/api/";
        private string GoogleMapsKey => ((Usuario)Context.User).Prefeitura.GoogleMapsApiKey;
        public GAddressService(
            Func<ContextRequest<int, string>> contextFactory,
            IPrefeituraService prefeituraService

        )
            : base(contextFactory)
        {
            PrefeituraService = prefeituraService;
        }

        #region Daos-Services

        protected readonly IPrefeituraService PrefeituraService;

        protected IMapper AMapper => ApplicationContext.Resolve<IMapper>();

        #endregion

        public async Task<EnderecoDto> GeoCode(EnderecoDto enderecoDto)
        {
            string language = ((Usuario)Context.User).GetLanguage().Replace("-", "_");
            string fullAddress = String.Join(", ", new[]
            {
                enderecoDto.rua, enderecoDto.numero, enderecoDto.bairro, $"{enderecoDto.cidadeNome} - {enderecoDto.estadoSigla}", enderecoDto.cep
            }.Where(s => !String.IsNullOrWhiteSpace(s)));

            Dictionary<string, object> uriParams = new Dictionary<string, object>
            {
                { "address", fullAddress },
                { "key", GoogleMapsKey }
            };

            if (!string.IsNullOrEmpty(language))
                uriParams.Add("language", language);

            PiHttpResponse<GeoCodeResultDto> response = await Context.HttpClient.GetAsync<GeoCodeResultDto>(BASE_URL, "geocode/json", uriParams);
            if (response.Success)
            {
                GeoCodeResultDto geoCodeResult = response.Data;
                if (geoCodeResult.results.Count >= 1)
                {
                    //if (geoCodeResult != null && geoCodeResult.results.Count == 1)
                    //{
                    PlaceDto placeDto = geoCodeResult.results[0];
                    placeDto.name = enderecoDto.complemento;
                    return AMapper.Map<EnderecoDto>(placeDto);
                }
                else
                {
                    throw new PiBusinessException("Invalid Operation: More than one location was found. Please check the provided address, as it is incomplete.");
                }
            }
            else
            {
                throw new PiBusinessException(response.Message, response.StatusCode);
                //throw new PiBusinessException("Invalid Operation: No location was found. Please check the provided address, as it is incomplete.");
            }
        }

        public async Task<EnderecoDto> GeoDecode(DbGeography location)
        {
            string language = ((Usuario)Context.User).GetLanguage().Replace("-", "_");
            string latlng = $"{location.Latitude};{location.Longitude}".Replace(',', '.').Replace(';', ',');

            Dictionary<string, object> uriParams = new Dictionary<string, object>
            {
                { "latlng", latlng },
                { "key", GoogleMapsKey }
            };

            if (!string.IsNullOrEmpty(language))
                uriParams.Add("language", language);

            PiHttpResponse<GeoCodeResultDto> response = await Context.HttpClient.GetAsync<GeoCodeResultDto>(BASE_URL, "geocode/json", uriParams);
            if (response.Success)
            {
                GeoCodeResultDto geoCodeResult = response.Data;
                if (geoCodeResult.results.Any())
                {
                    double distanceMin = 1000;
                    PlaceDto placeDto = null;
                    foreach (PlaceDto place in geoCodeResult.results.Where(g => g.geometry.location_type != "GEOMETRIC_CENTER"))
                    {
                        double? distance = place.geometry.location.ToDbGeography().Distance(location);
                        if (!distance.HasValue)
                        {
                            distance = 0;
                        }
                        if (distance < distanceMin)
                        {
                            distanceMin = distance.Value;
                            placeDto = place;
                        }
                    }
                    return AMapper.Map<EnderecoDto>(placeDto);
                }
                else
                {
                    throw new PiBusinessException("Invalid Operation: No location was found. Please check the provided address, as it is incomplete.");
                }
            }
            else
            {
                throw new PiBusinessException(response.Message, response.StatusCode);
            }
        }

        public async Task<IList<PlaceAutoCompletePredictionsDto>> PlaceAutoComplete(PlaceAutoCompleteRequestDto model)
        {
            string language = ((Usuario)Context.User).GetLanguage().Replace("-", "_");

            decimal? radius = model.locationBias?.radius;
            decimal? lat = model.locationBias?.center?.lat;
            decimal? lng = model.locationBias?.center?.lng;
            string locationBias = radius != null && lat != null && lng != null
                ? $"circle:{radius}@{lat},{lng}" : "";

            Dictionary<string, object> uriParams = new Dictionary<string, object>
            {
                { "input", model.input },
                { "sessionToken", model.sessionToken },
                { "key", GoogleMapsKey }
            };

            if (!string.IsNullOrEmpty(locationBias))
                uriParams.Add("locationbias", locationBias);

            if (!string.IsNullOrEmpty(language))
                uriParams.Add("language", language);

            PiHttpResponse<PlaceAutoCompleteResultDto> response = await Context.HttpClient.GetAsync<PlaceAutoCompleteResultDto>(BASE_URL, "place/autocomplete/json", uriParams);

            if (response.Success)
            {
                PlaceAutoCompleteResultDto placeAutoCompleteResult = response.Data;
                IList<PlaceAutoCompletePredictionsDto> result = new List<PlaceAutoCompletePredictionsDto>();
                foreach (var prediction in placeAutoCompleteResult.predictions)
                {
                    result.Add(new PlaceAutoCompletePredictionsDto { fullAddress = prediction.description, googleMapsPlaceId = prediction.place_id });
                }
                return result;
            }
            throw new PiBusinessException($"Invalid Operation: The Google Api responded with the message: {response.Message}", response.StatusCode);
        }

        public async Task<EnderecoDto> PlaceDetails(string placeId, string sessionToken = null)
        {
            string language = ((Usuario)Context.User).GetLanguage().Replace("-", "_");
            string fields = "address_components,geometry,place_id,name";

            Dictionary<string, object> uriParams = new Dictionary<string, object>
            {
                { "place_id", placeId },
                { "fields", fields },
                { "key", GoogleMapsKey }
            };

            if (!string.IsNullOrEmpty(sessionToken))
                uriParams.Add("sessionToken", sessionToken);

            if (!string.IsNullOrEmpty(language))
                uriParams.Add("language", language);

            PiHttpResponse<PlaceDetailsResultDto> response = await Context.HttpClient.GetAsync<PlaceDetailsResultDto>(BASE_URL, "place/details/json", uriParams);

            if (response.Success)
            {
                return AMapper.Map<EnderecoDto>(response.Data.result);
            }
            else
            {
                throw new PiBusinessException($"Invalid Operation: The Google Api responded with the message: {response.Message}", response.StatusCode);
            }
        }
    }
}
