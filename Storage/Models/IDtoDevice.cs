using System.Collections.Generic;

namespace Storage.Models
{
    public interface IDtoDevice
    {
        int DeviceId { get; set; }
        string Name { get; set; }
        IList<IDtoSensor> Sensors { get; }
    }
}
