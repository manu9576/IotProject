using System.ComponentModel;

namespace Sensors.Weather
{
    internal class WeatherHumidity : WeatherSensor, INotifyPropertyChanged
    {
        internal WeatherHumidity(WebWeather webWeather, string name, int sensorId, bool rgbDisplay)
            : base(webWeather, name, sensorId, rgbDisplay)
        { }

        public override double Value => webWeather.GetHumidity();

        public override string Unit => "%";
    }
}
