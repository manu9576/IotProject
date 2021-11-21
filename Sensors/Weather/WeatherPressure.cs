using System.ComponentModel;

namespace Sensors.Weather
{
    internal class WeatherPressure : WeatherSensor, INotifyPropertyChanged
    {

        internal WeatherPressure(WebWeather webWeather, string name, int sensorId, bool rgbDisplay) 
            : base(webWeather, name, sensorId, rgbDisplay)
        { }

        public override double Value => webWeather.GetPressure();

        public override string Unit => "hPa";
    }
}
