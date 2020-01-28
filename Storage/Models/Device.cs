using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Storage.Models
{
    public class Device
    {
        [Key]
        public int DeviceId { get; set; }
        public string Name { get; set; }

        public ICollection<Sensor> Sensors { get; } = new List<Sensor>();
    }
}
