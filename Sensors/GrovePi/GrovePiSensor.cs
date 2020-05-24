using Iot.Device.GrovePiDevice.Models;
using Iot.Device.GrovePiDevice.Sensors;
using Sensors.Weather;

namespace Sensors.GrovePi
{
    public abstract class GrovePiSensor : ISensor
    {
        protected GrovePiSensor(string name, string unit, SensorType sensorType, GrovePort port)
        {
            Name = name;
            Unit = unit;
            SensorType = sensorType;
            Port = port;
        }

        public SensorType SensorType { get; }
        public GrovePort Port { get; }
        public string Name { get; }
        public string Unit { get; }

        public abstract double Value { get; }
        public abstract void Refresh();

    }
}
