﻿using System.ComponentModel;

namespace Sensors.Weather
{
    internal class WeatherWindDirection : ISensor, INotifyPropertyChanged
    {
        private readonly WebWeather webWeather;
        public event PropertyChangedEventHandler PropertyChanged;

        internal WeatherWindDirection(WebWeather webWeather, string name, int sensorId, bool rgbDisplay)
        {
            this.webWeather = webWeather;
            this.Name = name;
            SensorId = sensorId;
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
                return webWeather.GetWindDirection();
            }
        }

        public string Unit => "°";

        public bool RgbDisplay { get; private set; }
    }
}
