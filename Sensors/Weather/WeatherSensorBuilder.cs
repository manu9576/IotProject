using System;

namespace Sensors.Weather
{
    public static class WeatherSensorBuilder
    {
        private static WebWeather webWeather = null;
        private static Refresher refresher = null;

        public static ISensor GetSensor(SensorWeatherType sensorWeatherType, string name)
        {
            if (webWeather == null)
            {
                webWeather = new WebWeather();
                refresher = new Refresher();
                refresher.AddSensor(webWeather);
                refresher.Start(600000);
            }

            switch (sensorWeatherType)
            {
                case SensorWeatherType.Temperature:
                    return new WeatherTemperature(webWeather, name);

                case SensorWeatherType.Humidity:
                    return new WeatherHumidity(webWeather, name);

                case SensorWeatherType.Pressure:
                    return new WeatherPressure(webWeather, name);

                case SensorWeatherType.WindSpeed:
                    return new WeatherWindSpeed(webWeather, name);

                case SensorWeatherType.WindDirection:
                    return new WeatherWindDirection(webWeather, name);

                default:
                    throw new Exception("The type <" + webWeather + "> is not manage by the WeatherSensorBuilder");
            }
        }
    }
}