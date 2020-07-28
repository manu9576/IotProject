namespace Sensors.Weather
{
    internal class WeatherPressure : ISensor
    {
        private readonly WebWeather webWeather;

        internal WeatherPressure(WebWeather webWeather, string name)
        {
            this.webWeather = webWeather;
            this.Name = name;
        }

        public string Name { get; }

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
