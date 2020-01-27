using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Storage.Models
{
    public class Sensor
    {
        [Key]
        public int SensorId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }

        public string DeviceName { get; set; }
        public Device Device { get; set; }

        public List<Mesurment> Mesurements { get; } = new List<Mesurment>();
    }
}
