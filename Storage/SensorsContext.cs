using Microsoft.EntityFrameworkCore;
using Storage.Models;
using System.Linq;


namespace Storage
{
    public class SensorsContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Mesurment> Mesurments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql("Server=rasp4;User Id=SensorsUser;Password=15973;Database=Sensors");
        }
    }
}
