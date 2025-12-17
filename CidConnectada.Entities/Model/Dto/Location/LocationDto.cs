using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Globalization;

namespace CidConnectada.Entities.Model.Dto.Location
{
    public class LocationDto
    {
        public LocationDto() {}

        public LocationDto(decimal lat, decimal lng)
        {
            this.lat = lat;
            this.lng = lng;
        }

        [Range(-90.0, 90.0, ErrorMessage = "O valor para Latitude deve ser entre -90 e 90")]
        public decimal lat { get; set; }
        [Range(-180.0, 180.0, ErrorMessage = "O valor para Longitude deve ser entre -180 e 180")]
        public decimal lng { get; set; }

        public string ToWkt(bool alone = true)
        {
            string result = alone ? "POINT(" : "";
            result += String.Format("{0} {1}",
                lng.ToString(CultureInfo.InvariantCulture),
                lat.ToString(CultureInfo.InvariantCulture)
            );
            return result + (alone ? ")" : "");
        }

        public DbGeography ToDbGeography()
        {
            return DbGeography.FromText(ToWkt());
        }

        public static LocationDto FromDbGeo(DbGeography DbGeo)
        {
            LocationDto locationDto = new LocationDto((decimal)DbGeo.Latitude, (decimal)DbGeo.Longitude);
            return locationDto;
        }

        public static string ToWkt(decimal lat, decimal lng)
        {
            LocationDto locationDto = new LocationDto(lat, lng);
            return locationDto.ToWkt();
        }
    }
}