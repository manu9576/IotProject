namespace Sensors.Configuration
{
    public enum SensorType { OpenWeatherMap, GrovePi };

    public interface ISensorConfiguration
    {
        int SensorId { get; set; }
        string Name { get; set; }
        SensorType SensorType { get; }
    }
}