namespace Sensors
{
    public interface ISensor
    {
        string Name { get; }
        double Value { get; }
        string Unit { get; }
        void Refresh();
    }
}
