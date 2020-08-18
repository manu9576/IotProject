using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace Sensors.Weather
{
    internal class WeatherWindSpeed : ISensor, INotifyPropertyChanged
    {
        private readonly WebWeather webWeather;
        public event PropertyChangedEventHandler PropertyChanged;

        internal WeatherWindSpeed(WebWeather webWeather, string name, int sensorId)
        {
            this.webWeather = webWeather;
            this.Name = name;
            this.SensorId = sensorId;
            this.webWeather.PropertyChanged += WebWeather_PropertyChanged;
        }

        private void WebWeather_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
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

        public int SensorId { get; }
      
    }
}
