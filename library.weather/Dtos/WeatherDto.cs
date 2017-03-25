using System.Collections.Generic;

namespace library.weather.dtos
{
    public class Currently
    {
        public int Time { get; set; }
        public string Summary { get; set; }
        public string Icon { get; set; }
        public int NearestStormDistance { get; set; }
        public int NearestStormBearing { get; set; }
        public int PrecipIntensity { get; set; }
        public int PrecipProbability { get; set; }
        public double Temperature { get; set; }
        public double ApparentTemperature { get; set; }
        public double DewPoint { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public int WindBearing { get; set; }
        public int Visibility { get; set; }
        public double CloudCover { get; set; }
        public double Pressure { get; set; }
        public double Ozone { get; set; }
    }

    public class Datum
    {
        public int Time { get; set; }
        public string Summary { get; set; }
        public string Icon { get; set; }
        public int SunriseTime { get; set; }
        public int SunsetTime { get; set; }
        public double MoonPhase { get; set; }
        public double PrecipIntensity { get; set; }
        public double PrecipIntensityMax { get; set; }
        public double PrecipProbability { get; set; }
        public double TemperatureMin { get; set; }
        public int TemperatureMinTime { get; set; }
        public double TemperatureMax { get; set; }
        public int TemperatureMaxTime { get; set; }
        public double ApparentTemperatureMin { get; set; }
        public int ApparentTemperatureMinTime { get; set; }
        public double ApparentTemperatureMax { get; set; }
        public int ApparentTemperatureMaxTime { get; set; }
        public double DewPoint { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public int WindBearing { get; set; }
        public double Visibility { get; set; }
        public double CloudCover { get; set; }
        public double Pressure { get; set; }
        public double Ozone { get; set; }
        public int? PrecipIntensityMaxTime { get; set; }
        public string PrecipType { get; set; }
    }

    public class Daily
    {
        public string Summary { get; set; }
        public string Icon { get; set; }
        public List<Datum> Data { get; set; }
    }

    public class WeatherDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Timezone { get; set; }
        public int Offset { get; set; }
        public Currently Currently { get; set; }
        public Daily Daily { get; set; }
    }
}
