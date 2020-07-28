namespace Sensors.Weather
{
    internal class WeatherWindSpeed : ISensor
    {
        private readonly WebWeather webWeather;

        internal WeatherWindSpeed(WebWeather webWeather, string name)
        {
            this.webWeather = webWeather;
            this.Name = name;
        }

        public string Name { get; }

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
