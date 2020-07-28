namespace Sensors.Weather
{
    internal class WeatherTemperature : ISensor
    {
        private readonly WebWeather webWeather;

        internal WeatherTemperature(WebWeather webWeather, string name)
        {
            this.webWeather = webWeather;
            this.Name = name;
        }

        public string Name { get; }

        public double Value 
        {
            get
            {
                return webWeather.GetTemperature();
            }
        }

        public string Unit => "°C";
    }
}
