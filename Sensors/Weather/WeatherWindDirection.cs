using System.ComponentModel;

namespace Sensors.Weather
{
    internal class WeatherWindDirection : WeatherSensor, INotifyPropertyChanged
    {
        internal WeatherWindDirection(WebWeather webWeather, string name, int sensorId, bool rgbDisplay)
            : base(webWeather, name, sensorId, rgbDisplay)
        { }

        public override double Value => webWeather.GetWindDirection();

        public override string Unit => "°";
    }
}
