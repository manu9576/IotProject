using System;
using System.ComponentModel.DataAnnotations;


namespace Storage.Models
{
    public class Measure
    {
        [Key]
        public int MeasureId { get; set; }
        public DateTime DateTime { get; set; }
        public double Value { get; set; }

        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
    }
}
