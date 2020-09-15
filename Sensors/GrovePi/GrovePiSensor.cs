using Iot.Device.GrovePiDevice.Models;

namespace Sensors.GrovePi
{
    public abstract class GrovePiSensor
    {
        protected GrovePiSensor(string name, string unit, int sensorId, SensorType sensorType, GrovePort port, bool rgbDisplay)
        {
            Name = name;
            Unit = unit;
            SensorId = sensorId;
            SensorType = sensorType;
            Port = port;
            RgbDisplay = rgbDisplay;
        }

        public SensorType SensorType { get; }
        public GrovePort Port { get; }
        public string Name { get; }
        public string Unit { get; }
        public int SensorId { get; set; }
        public bool RgbDisplay { get; }

        public abstract double Value { get; }
        public abstract void Refresh();

    }
}
