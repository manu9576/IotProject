using Sensors.Weather;
using System;
using System.Linq;

namespace Sensors.Configuration
{
    public class OpenWeatherMapSensorConfiguration : SensorConfiguration
    {
        public OpenWeatherMapSensorConfiguration() : base()
        {
            SensorId = -1;
            Name = "Nouveau capteur";
            SensorWeatherType = SensorWeatherType.Temperature;
        }

        public override SensorType SensorType
        {
            get
            {
                return SensorType.OpenWeatherMap;
            }
        }

        public SensorWeatherType SensorWeatherType { get; set; }

        public SensorWeatherType[] SensorsType
        {
            get
            {
                return (SensorWeatherType[])Enum.GetValues(typeof(SensorWeatherType)).Cast<SensorWeatherType>();
            }
        }

        public override string ToString()
        {
            return "Name= " + Name + " -- id= " + SensorId + " -- SensorType= " + SensorType + " -- SensorWeatherType= " + SensorWeatherType + " -- RgbDisplay " + RgbDisplay;
        }

        public override string HashCode
        {
            get
            {
                var hashCode = this.SensorWeatherType.ToString().GetHashCode();
                hashCode ^= this.Name.ToString().GetHashCode();
                hashCode ^= this.SensorId.ToString().GetHashCode();
                hashCode ^= this.SensorType.ToString().GetHashCode();

                return hashCode.ToString();
            }
        }
    }
}
