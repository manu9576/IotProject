using System.ComponentModel;

namespace Sensors.Weather
{
    internal class WeatherTemperature : ISensor, INotifyPropertyChanged
    {
        private readonly WebWeather webWeather;
        public event PropertyChangedEventHandler PropertyChanged;

        internal WeatherTemperature(WebWeather webWeather, string name, int sensorId, bool rgbDisplay)
        {
            this.webWeather = webWeather;
            this.Name = name;
            this.SensorId = sensorId;
            this.webWeather.PropertyChanged += WebWeather_PropertyChanged;
            this.RgbDisplay = rgbDisplay;
        }

        private void WebWeather_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
        }

        public string Name { get; }
        public int SensorId { get; set; }

        public double Value
        {
            get
            {
                return webWeather.GetTemperature();
            }
        }

        public string Unit => "°C";

        public bool RgbDisplay { get; private set; }

    }
}
