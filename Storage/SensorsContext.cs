using Microsoft.EntityFrameworkCore;
using Storage.Models;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace Storage
{
    public class SensorsContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Measure> Measures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // at startup the network should be not started
            // we wait to detect server before create 
            WaitServerDetection("192.168.1.23");

            optionsBuilder.UseMySql("Server=192.168.1.23;User Id=SensorsUser;Password=cdsfdsfklhjlgd\"443_; Database=Sensors",
                                options => options.EnableRetryOnFailure());
        }

        private void WaitServerDetection(string serverName)
        {
            while(!ServerIsPresent(serverName))
            {
                Thread.Sleep(5000);
            }

        }

        private bool ServerIsPresent(string serverName)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions
            {
                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                DontFragment = true
            };

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;

            try
            {
                PingReply reply = pingSender.Send(serverName, timeout, buffer, options);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
