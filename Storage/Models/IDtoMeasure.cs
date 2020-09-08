using System;

namespace Storage.Models
{
    public interface IDtoMeasure
    {
        DateTime DateTime { get; set; }
        double Value { get; set; }
    }
}
