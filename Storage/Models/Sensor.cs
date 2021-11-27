using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Storage.Models
{
    public class Sensor : IDtoSensor
    {
        [Key]
        public int SensorId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }

        public int DeviceId { get; set; }
        public IDtoDevice Device { get; set; }

        public IList<IDtoMeasure> Measures { get; } = new List<IDtoMeasure>();
    }
}
