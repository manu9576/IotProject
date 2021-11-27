using System;
using System.ComponentModel.DataAnnotations;

namespace Storage.Models
{
    public class Measure : IDtoMeasure
    {
        [Key]
        public int MeasureId { get; set; }
        public DateTime DateTime { get; set; }
        public double Value { get; set; }

        public int SensorId { get; set; }
        public IDtoSensor Sensor { get; set; }
    }
}
