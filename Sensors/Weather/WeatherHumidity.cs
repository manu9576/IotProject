using System.ComponentModel;

namespace Sensors.Weather
{
    internal class WeatherHumidity : ISensor, INotifyPropertyChanged
    {
        private readonly WebWeather webWeather;
        public event PropertyChangedEventHandler PropertyChanged;

        internal WeatherHumidity(WebWeather webWeather, string name)
        {
            this.webWeather = webWeather;
            this.Name = name;
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
                return webWeather.GetHumidity();
            }
        }

        public string Unit => "%";

    }
}
