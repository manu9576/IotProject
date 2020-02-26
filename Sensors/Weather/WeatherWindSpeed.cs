namespace Sensors.Weather
{
    internal class WeatherWindSpeed : ISensor
    {
        private readonly WebWeather webWeather;

        internal WeatherWindSpeed(WebWeather webWeather)
        {
            this.webWeather = webWeather;
        }

        public string Name => "Weather Wind Speed";

        public double Value
        {
            get
            {
                return webWeather.GetWindSpeed() * 3.6;
            }
        }

        public string Unit => "km/h";
    }
}
