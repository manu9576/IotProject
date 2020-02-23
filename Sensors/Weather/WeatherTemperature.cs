using System;
using System.Collections.Generic;
using System.Text;

namespace Sensors.Weather
{
    internal class WeatherTemperature : ISensor
    {
        private readonly WebWeather webWeather;

        internal WeatherTemperature(WebWeather webWeather)
        {
            this.webWeather = webWeather;
        }

        public string Name => "Weather Temperature";

        public double Value 
        {
            get
            {
                var nodes = webWeather.XmlData.GetElementsByTagName("temperature");

                var temp = nodes.

                return 0.0;
            }
        }

        public string Unit => "°C";
    }
}
