using Sensors.Configuration;
using Sensors.GrovePi;
using Sensors.Weather;
using System;
using System.Collections.ObjectModel;

namespace Sensors
{

    public static class SensorsManager
    {
        public static ObservableCollection<ISensor> Sensors { private set; get; }
        public static SensorsConfiguration SensorsConfiguration { private set; get; }

        static SensorsManager()
        {
            Sensors = new ObservableCollection<ISensor>();
            ReloadConfiguration();
        }

        static public void ReloadConfiguration()
        {
            Sensors.Clear();
            SensorsConfiguration = SensorsConfiguration.Load();

            foreach (var sensorConfiguration in SensorsConfiguration.Sensors)
            {
                switch (sensorConfiguration.SensorType)
                {
                    case Configuration.SensorType.GrovePi:

                        var grovePiSensorConfiguration = sensorConfiguration as GrovePiSensorConfiguration;

                        Sensors.Add
                            (
                                GrovePiSensorBuilder.CreateSensor(
                                    grovePiSensorConfiguration.GroveSensorType,
                                    grovePiSensorConfiguration.GrovePort,
                                    grovePiSensorConfiguration.Name)
                            );

                        break;

                    case Configuration.SensorType.OpenWeatherMap:

                        var openWeatherMapSensorConfiguration = sensorConfiguration as OpenWeatherMapSensorConfiguration;

                        Sensors.Add
                            (
                                WeatherSensorBuilder.GetSensor(
                                    openWeatherMapSensorConfiguration.SensorWeatherType,
                                    openWeatherMapSensorConfiguration.Name
                                    )
                            );

                        break;

                    default:
                        throw new Exception("Unsupported SensorType in SensorsManager");
                }
            }
        }
    }
}
