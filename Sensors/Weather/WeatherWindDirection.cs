namespace Sensors.Weather
{
    internal class WeatherWindDirection : ISensor
    {
        private readonly WebWeather webWeather;

        internal WeatherWindDirection(WebWeather webWeather, string name)
        {
            this.webWeather = webWeather;
            this.Name = name;
        }

        public string Name { get; }

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
