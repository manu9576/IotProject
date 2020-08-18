using System.ComponentModel;

namespace Sensors
{
    public interface ISensor : INotifyPropertyChanged
    {
        int SensorId { get; }
        string Name { get; }
        double Value { get; }
        string Unit { get; }
    }
}
