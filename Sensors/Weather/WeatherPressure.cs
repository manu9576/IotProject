using System.ComponentModel;

namespace Sensors.Weather
{
    internal class WeatherPressure : ISensor, INotifyPropertyChanged
    {
        private readonly WebWeather webWeather;
        public event PropertyChangedEventHandler PropertyChanged;

        internal WeatherPressure(WebWeather webWeather, string name, int sensorId)
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
        public int SensorId { get; }

        public double Value
        {
            get
            {
                return webWeather.GetPressure();
            }
        }

        public string Unit => "hPa";
    }
}
