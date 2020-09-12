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

        public override string ToString()
        {
            return "Name= " + Name + " -- id= " + SensorId + " -- SensorType= " + SensorType + " -- GrovePort= " + GrovePort + " -- GroveSensorType= " + GroveSensorType;
        }

        public override string HashCode
        {
            get
            {
                var hashCode = this.GrovePort.ToString().GetHashCode();
                hashCode ^= this.GroveSensorType.ToString().GetHashCode();
                hashCode ^= this.Name.ToString().GetHashCode();
                hashCode ^= this.SensorId.ToString().GetHashCode();
                hashCode ^= this.SensorType.ToString().GetHashCode();

                return hashCode.ToString();
            }
        }

    }
}
