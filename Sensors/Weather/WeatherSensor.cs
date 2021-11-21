using System.ComponentModel;

namespace Sensors.Weather
{
    abstract class WeatherSensor: ISensor
    {
        public WeatherSensor(WebWeather webWeather, string name, int sensorId, bool rgbDisplay)
        {
            this.Name = name;
            this.SensorId = sensorId;
            this.RgbDisplay = rgbDisplay;
            this.webWeather = webWeather;
            this.webWeather.PropertyChanged += WebWeather_PropertyChanged;
        }

        public string Name { get; internal set; }

        public int SensorId { get; internal set; }

        public bool RgbDisplay { get; internal set; }

        public virtual string Unit { get; }

        public virtual double Value { get; }


        protected WebWeather webWeather;

        public event PropertyChangedEventHandler PropertyChanged;

        internal void WebWeather_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }
    }
}
