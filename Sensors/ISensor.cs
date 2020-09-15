using System.ComponentModel;

namespace Sensors
{
    public interface ISensor : INotifyPropertyChanged
    {
        int SensorId { get; set; }
        string Name { get; }
        double Value { get; }
        string Unit { get; }
        bool RgbDisplay { get; }
    }
}
