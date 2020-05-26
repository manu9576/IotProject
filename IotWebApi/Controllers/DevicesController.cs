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

        /// <summary>
        ///  GET: api/Device/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// GET: api/Device/Name/name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("Name/{name}")]
        public async Task<ActionResult<Device>> GetDeviceByName(string name)
        {
            var device = _context.Devices.Where(dev => dev.Name == name);

            if (device == null)
            {
                return NotFound();
            }

            return await device.FirstOrDefaultAsync();
        }
    }
}
