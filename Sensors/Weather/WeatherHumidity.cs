namespace Sensors.Weather
{
    internal class WeatherHumidity : ISensor
    {
        private readonly WebWeather webWeather;

        internal WeatherHumidity(WebWeather webWeather, string name)
        {
            this.webWeather = webWeather;
            this.Name = name;
        }

        public string Name { get; }

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
