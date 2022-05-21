using Microsoft.EntityFrameworkCore;
using Storage.Models;

namespace Storage
{
    public interface ISensorsContext
    {
        DbSet<Device> Devices { get; set; }
        DbSet<Measure> Measures { get; set; }
        DbSet<Sensor> Sensors { get; set; }
    }
}