namespace Sensors.Weather
{
    internal class WeatherTemperature : ISensor
    {
        private readonly WebWeather webWeather;

        internal WeatherTemperature(WebWeather webWeather)
        {
            this.webWeather = webWeather;
        }

        public string Name => "Weather Temperature";

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
