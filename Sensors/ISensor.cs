using System.ComponentModel;

namespace Sensors
{
    public interface ISensor : INotifyPropertyChanged
    {
        string Name { get; }
        double Value { get; }
        string Unit { get; }
    }
}
