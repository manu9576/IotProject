using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Storage.Models
{
    public class Device
    {
        [Key]
        public string Name { get; set; }
        public List<Sensor> Sensors { get; } = new List<Sensor>();
    }
}
