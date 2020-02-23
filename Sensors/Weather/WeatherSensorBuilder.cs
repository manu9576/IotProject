using System;
using System.Collections.Generic;
using System.Text;

namespace Sensors.Weather
{
    public static class WeatherSensorBuilder
    {
        private static WebWeather WebWeather;

        static WeatherSensorBuilder()
        {
            WebWeather = new WebWeather();

            Refresher refrecher = new Refresher();
        }

    }
}
