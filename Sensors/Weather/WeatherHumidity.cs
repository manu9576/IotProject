using System.ComponentModel;

namespace Sensors.Weather
{
    internal class WeatherHumidity : ISensor, INotifyPropertyChanged
    {
        private readonly WebWeather webWeather;
        public event PropertyChangedEventHandler PropertyChanged;

        internal WeatherHumidity(WebWeather webWeather, string name, int sensorId, bool rgbDisplay)
        {
            this.webWeather = webWeather;
            this.Name = name;
            this.webWeather.PropertyChanged += WebWeather_PropertyChanged;
            this.SensorId = sensorId;
            this.RgbDisplay = rgbDisplay;
        }

        private void WebWeather_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }

        public string Name { get; set; }

        public double Value
        {
            get
            {
                return webWeather.GetHumidity();
            }
        }

        public bool RgbDisplay { get; private set; }

        public string Unit => "%";

        public int SensorId { get; set; }
    }
}
