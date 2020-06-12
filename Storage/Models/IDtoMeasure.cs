using System;


namespace Storage.Models
{
    public interface IDtoMeasure
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }
}
