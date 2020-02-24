using System;
using System.Collections.Generic;
using System.Text;

namespace Sensors.Weather
{
    public static class WeatherSensorBuilder
    {
        private static WebWeather webWeather = null;
        private static Refresher refresher = null;

        public static ISensor GetSensor(SensorWeatherType sensorWeatherType)
        {
            if (webWeather == null)
            {
                webWeather = new WebWeather();
                refresher = new Refresher();
                refresher.AddSensor(webWeather);
                refresher.Start();
            }

            switch (sensorWeatherType)
            {
                case SensorWeatherType.Temperature:
                    return new WeatherTemperature(webWeather);

                case SensorWeatherType.Humidity:
                case SensorWeatherType.Pressure:
                case SensorWeatherType.WindSpeed:
                case SensorWeatherType.WindDirection:
                default:
                    throw new Exception("The type <" + webWeather + "> is not manage by the WeatherSensorBuilder");
            }
        }
    }
}