namespace Sensors.Weather
{
    internal class WeatherHumidity : ISensor
    {
        private readonly WebWeather webWeather;

        internal WeatherHumidity(WebWeather webWeather)
        {
            this.webWeather = webWeather;
        }

        public string Name => "Weather Humidity";

        public double Value
        {
            get
            {
                return webWeather.GetHumidity();
            }
        }

        public string Unit => "%";

    }
}
