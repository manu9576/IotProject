namespace Sensors.Weather
{
    internal class WeatherPressure : ISensor
    {
        private readonly WebWeather webWeather;

        internal WeatherPressure(WebWeather webWeather)
        {
            this.webWeather = webWeather;
        }

        public string Name => "Weather Pressure";

        public double Value
        {
            get
            {
                return webWeather.GetPressure();
            }
        }

        public string Unit => "hPa";
    }
}
