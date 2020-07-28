using Sensors.Weather;

namespace Sensors.Configuration
{
    class OpenWeatherMapSensorConfiguration
    {
        public int SensorId { get; set; }
        public string Name { get; set; }
        public SensorType SensorType => SensorType.OpenWeatherMap;

        public SensorWeatherType SensorWeatherType { get; set; }
    }
}
