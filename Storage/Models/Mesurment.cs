using System;
using System.ComponentModel.DataAnnotations;


namespace Storage.Models
{
    public class Mesurment
    {
        [Key]
        public int MesurmentId { get; set; }
        public DateTime DateTime { get; set; }
        public double Value { get; set; }

        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
    }
}
