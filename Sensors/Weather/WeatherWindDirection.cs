namespace Sensors.Weather
{
    internal class WeatherWindDirection: ISensor
    {
        private readonly WebWeather webWeather;

        internal WeatherWindDirection(WebWeather webWeather)
        {
            this.webWeather = webWeather;
        }

        public string Name => "Weather Wind Direction";

        public double Value
        {
            get
            {
                return webWeather.GetWindDirection();
            }
        }

        public string Unit => "°";
    }
}
