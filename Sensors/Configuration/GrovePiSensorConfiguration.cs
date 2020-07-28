using Iot.Device.GrovePiDevice.Models;

namespace Sensors.Configuration
{
    public class GrovePiSensorConfiguration : ISensorConfiguration
    {
        public int SensorId { get; set; }
        public string Name { get; set; }
        public SensorType SensorType => SensorType.GrovePi;

        public GrovePi.SensorType  GroveSensorType { get; set; }

        public GrovePort GrovePort { get; set; }

    }
}
