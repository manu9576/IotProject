using Iot.Device.GrovePiDevice.Models;
using Sensors.GrovePi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sensors
{

    public class SensorsManager
    {
        public ObservableCollection<ISensor> Sensors { private set; get; }

        public SensorsManager()
        {
            Sensors = new ObservableCollection<ISensor>
            {
                GrovePiSensorBuilder.CreateSensor(SensorType.DhtTemperatureSensor, GrovePort.DigitalPin7, "Température"),
                GrovePiSensorBuilder.CreateSensor(SensorType.DhtHumiditySensor, GrovePort.DigitalPin7, "Humidité")
            };
        }

    }
}
