using System.ComponentModel;

namespace Sensors.Weather
{
    internal class WeatherWindSpeed : WeatherSensor, INotifyPropertyChanged
    {
        private const double CONVERT_MS_TO_KMH = 3.6;

        internal WeatherWindSpeed(WebWeather webWeather, string name, int sensorId, bool rgbDisplay) 
            : base(webWeather, name, sensorId, rgbDisplay)
        { }

        public override double Value => webWeather.GetWindSpeed() * CONVERT_MS_TO_KMH;

        public override string Unit => "km/h";
    }
}
