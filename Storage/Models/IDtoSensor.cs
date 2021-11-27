using System.Collections.Generic;

namespace Storage.Models
{
    public interface IDtoSensor
    {
        int SensorId { get; set; }
        string Name { get; set; }
        string Unit { get; set; }

        IList<IDtoMeasure> Measures { get; }
    }
}
