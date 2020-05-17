using Microsoft.EntityFrameworkCore;
using Storage.Models;

namespace IotWebApi.Models
{
    public class DbSensorsContext : DbContext
    {
        public DbSensorsContext(DbContextOptions<DbSensorsContext> options)
          : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }

        public DbSet<Sensor> Sensors { get; set; }

        public DbSet<Measure> Measures { get; set; }
    }
}
