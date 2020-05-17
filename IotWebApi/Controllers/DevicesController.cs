using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IotWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storage.Models;

namespace IotWebApi.Controllers
{
    [Route("api/Device")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly DbSensorsContext _context;

        public DevicesController(DbSensorsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Return the devices list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            return await _context.Devices.ToListAsync();
        }

        // GET: api/Device/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return device;
        }

        // GET: api/Device/5/Sensors
        [HttpGet("{id}/Sensors")]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensorsByDeviceId(int id)
        {
            return await _context.Sensors.Where(sens => sens.DeviceId == id).ToListAsync();

        }

    }
}
