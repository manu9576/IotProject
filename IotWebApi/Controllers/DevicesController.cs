using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<Device>>> GetTodoItems()
        {
            return await _context.Devices.ToListAsync();
        }

        // GET: api/Device/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetSensor(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return device;
        }
    }
}
