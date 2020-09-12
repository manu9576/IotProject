using System;
using System.Xml.Serialization;

namespace Sensors.Configuration
{
    public enum SensorType { OpenWeatherMap, GrovePi };

    [Serializable]
    [XmlInclude(typeof(OpenWeatherMapSensorConfiguration))]
    [XmlInclude(typeof(GrovePiSensorConfiguration))]
    public abstract class SensorConfiguration
    {
        public int SensorId { get; set; }
        public string Name { get; set; }
        public abstract SensorType SensorType { get; }
        public abstract string HashCode { get; }
    }
}