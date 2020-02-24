using Iot.Device.GrovePiDevice.Models;
using Sensors.GrovePi;
using Sensors.Weather;
using System.Collections.ObjectModel;

namespace Sensors
{

    public static class SensorsManager
    {
        public static ObservableCollection<ISensor> Sensors { private set; get; }
       

        static SensorsManager()
        {
            Sensors = new ObservableCollection<ISensor>
            {
                GrovePiSensorBuilder.CreateSensor(SensorType.DhtTemperatureSensor, GrovePort.DigitalPin7, "Température"),
                GrovePiSensorBuilder.CreateSensor(SensorType.DhtHumiditySensor, GrovePort.DigitalPin7, "Humidité"),
                WeatherSensorBuilder.GetSensor(SensorWeatherType.Temperature)
            };

        }
    }
}
