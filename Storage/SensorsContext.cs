using Microsoft.EntityFrameworkCore;
using Storage.Models;
using System.Linq;


namespace Storage
{
    public class SensorsContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Sensor> Posts { get; set; }
        public DbSet<Mesurment> Blogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql("Server=rasp4;User Id=SensorsUser;Password=15973;Database=Sensors");
        }
    }
}
