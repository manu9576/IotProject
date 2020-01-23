namespace Sensors.GrovePi
{
    public interface IGrovePiSensor
    {
        string Name { get; }
        double Value { get; }
        string Unit { get; }
    }
}
