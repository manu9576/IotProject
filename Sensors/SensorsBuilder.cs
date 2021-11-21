using Sensors.Configuration;
using Sensors.GrovePi;
using Sensors.Weather;
using System;

namespace Sensors
{
    public class SensorsBuilder
    {
        public static ISensor GetSensor(SensorConfiguration sensorConfiguration)
        {
            ISensor sensor;

            switch (sensorConfiguration.SensorType)
            {
                case Configuration.SensorType.GrovePi:

                    var grovePiSensorConfiguration = sensorConfiguration as GrovePiSensorConfiguration;

                    sensor = GrovePiSensorBuilder.CreateSensor(
                                            grovePiSensorConfiguration.GroveSensorType,
                                            grovePiSensorConfiguration.GrovePort,
                                            grovePiSensorConfiguration.Name,
                                            grovePiSensorConfiguration.SensorId,
                                            grovePiSensorConfiguration.RgbDisplay
                                        );
                    break;

                case Configuration.SensorType.OpenWeatherMap:

                    var openWeatherMapSensorConfiguration = sensorConfiguration as OpenWeatherMapSensorConfiguration;

                    sensor = WeatherSensorBuilder.GetSensor(
                                        openWeatherMapSensorConfiguration.SensorWeatherType,
                                        openWeatherMapSensorConfiguration.Name,
                                        openWeatherMapSensorConfiguration.SensorId,
                                        openWeatherMapSensorConfiguration.RgbDisplay
                                   );
                    break;

                default:
                    throw new Exception("Unsupported SensorType in SensorBuilder : " + sensorConfiguration.SensorType.ToString());
            }

            return sensor;

        }
    }
}
