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

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // at startup the network should be not started
            // we wait to detect server before create 
            WaitServerDetection("rasp4");

            options.UseMySql("Server=rasp4;User Id=SensorsUser;Password=15973;Database=Sensors");
        }

        private void WaitServerDetection(string serverName)
        {
            while(!ComputerIsPresent(serverName))
            {
                Thread.Sleep(5000);
            }

        }

        private bool ComputerIsPresent(string serverName)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

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
