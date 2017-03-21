namespace core.places.dtos
{
    public class CurrentlyDto
    {
        public int Time { get; set; }
        public string Summary { get; set; }
        public string Icon { get; set; }
        public int NearestStormDistance { get; set; }
        public double PrecipIntensity { get; set; }
        public double PrecipIntensityError { get; set; }
        public int PrecipProbability { get; set; }
        public string PrecipType { get; set; }
        public double Temperature { get; set; }
        public double ApparentTemperature { get; set; }
        public double DewPoint { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public int WindBearing { get; set; }
        public double Visibility { get; set; }
        public double CloudCover { get; set; }
        public double Pressure { get; set; }
        public double Ozone { get; set; }
    }

    public class WeatherDto
    {
        public WeatherDto()
        {
            Currently = new CurrentlyDto();
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Timezone { get; set; }
        public CurrentlyDto Currently { get; set; }
    }
}
