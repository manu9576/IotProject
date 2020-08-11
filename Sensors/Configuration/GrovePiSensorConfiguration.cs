using Iot.Device.GrovePiDevice.Models;
using System;
using System.Linq;

namespace Sensors.Configuration
{
    public class GrovePiSensorConfiguration : SensorConfiguration
    {
        public GrovePiSensorConfiguration()
        {
            SensorId = -1;
            Name = "Nouveau capteur";
            GroveSensorType = GrovePi.SensorType.DhtTemperatureSensor;
            GrovePort = GrovePort.AnalogPin0;
        }

        public override SensorType SensorType
        {
            get
            {
                return SensorType.GrovePi;
            }
        }

        public GrovePi.SensorType  GroveSensorType { get; set; }
        public GrovePi.SensorType[] SensorsType
        {
            get
            {
                return (GrovePi.SensorType[])Enum.GetValues(typeof(GrovePi.SensorType)).Cast<GrovePi.SensorType>();
            }
        }

        public GrovePort GrovePort { get; set; }
        public GrovePort[] GrovePorts
        {
            get
            {
                return (GrovePort[])Enum.GetValues(typeof(GrovePort)).Cast<GrovePort>();
            }
        }

    }
}
