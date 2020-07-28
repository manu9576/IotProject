using Microsoft.EntityFrameworkCore;
using Storage.Models;

namespace IotWebApi.Models
{
    /// <summary>
    /// Class for the DbContect
    /// </summary>
    public class DbSensorsContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public DbSensorsContext(DbContextOptions<DbSensorsContext> options)
          : base(options)
        {
        }

        /// <summary>
        /// DbSet of the devices
        /// </summary>
        public DbSet<Device> Devices { get; set; }

        /// <summary>
        /// DbSet of the sensors
        /// </summary>
        public DbSet<Sensor> Sensors { get; set; }

        /// <summary>
        /// DbSet of the measures
        /// </summary>
        public DbSet<Measure> Measures { get; set; }
    }
}
