using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IotWebApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storage.Models;

namespace IotWebApi.Controllers
{
    /// <summary>
    /// Class that manage the request about device to the DB
    /// </summary>
    [EnableCors("MyPolicy")]
    [Route("api/Device")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly DbSensorsContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">context of the DB</param>
        public DevicesController(DbSensorsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Return the devices list
        /// </summary>
        /// <returns>List of existing devices</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            return await _context.Devices.ToListAsync();
        }

        /// <summary>
        ///  GET: api/Device/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Device corresponding to the id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IDtoDevice>> GetDevice(int id)
        {
            ActionResult<IDtoDevice> device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return device;
        }

        /// <summary>
        /// GET: api/Device/Name/name
        /// </summary>
        /// <param name="name">name of the device</param>
        /// <returns>Device corresponding to the name</returns>
        [HttpGet("Name/{name}")]
        public async Task<ActionResult<IDtoDevice>> GetDeviceByName(string name)
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
