using System.ComponentModel;

namespace Sensors.Weather
{
    internal class WeatherTemperature : WeatherSensor, INotifyPropertyChanged
    {
        internal WeatherTemperature(WebWeather webWeather, string name, int sensorId, bool rgbDisplay)
            : base(webWeather, name, sensorId, rgbDisplay)
        { }

        public override double Value =>  webWeather.GetTemperature();

        public override string Unit => "°C";
    }
}