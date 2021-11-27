using Microsoft.EntityFrameworkCore;
using Storage.Models;

namespace Storage
{
    public interface ISensorsContext
    {
        DbSet<IDtoDevice> Devices { get; set; }
        DbSet<IDtoMeasure> Measures { get; set; }
        DbSet<IDtoSensor> Sensors { get; set; }
    }
}