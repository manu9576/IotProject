using System.ComponentModel;

namespace Sensors.Weather
{
    internal class WeatherTemperature : ISensor, INotifyPropertyChanged
    {
        private readonly WebWeather webWeather;
        public event PropertyChangedEventHandler PropertyChanged;

        internal WeatherTemperature(WebWeather webWeather, string name, int sensorId)
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
                return webWeather.GetTemperature();
            }
        }

        public string Unit => "°C";
    }
}
