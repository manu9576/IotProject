using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IotWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storage.Models;

namespace IotWebApi.Controllers
{
    [Route("api/Sensor")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly DbSensorsContext _context;

        public SensorsController(DbSensorsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Return the sensors list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensors()
        {
            return await _context.Sensors.ToListAsync();
        }

        // GET: api/Sensor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sensor>> GetSensor(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);

            if (sensor == null)
            {
                return NotFound();
            }

            return sensor;
        }

        // GET: api/Sensor/5/Measures
        [HttpGet("{id}/Measures")]
        public async Task<ActionResult<IEnumerable<Measure>>> GetMersuresBySensorId(int id)
        {
            return await _context.Measures.Where(mes => mes.SensorId == id).ToListAsync();

        }
    }
}