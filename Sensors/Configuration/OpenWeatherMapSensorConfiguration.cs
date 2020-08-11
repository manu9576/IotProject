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
    }
}
