using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using IotWebApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storage.Models;

namespace IotWebApi.Controllers
{
    /// <summary>
    /// Class that manage the requests to the DB for Sensor
    /// </summary>
    [EnableCors("MyPolicy")]
	[Produces(MediaTypeNames.Application.Json)]
	[Consumes(MediaTypeNames.Application.Json)]
	[Route("api/sensors")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly DbSensorsContext _context;

        /// <summary>
        /// Creates a new instance of <see cref="SensorsController"/>. 
        /// </summary>
        /// <param name="context">Context of the DB.</param>
        public SensorsController(DbSensorsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Sensor
        /// </summary>
        /// <returns>List of all sensors stored</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensors()
        {
            return await _context.Sensors.ToListAsync();
        }

        /// <summary>
        /// Gets a sensor details from its id.
        /// </summary>
        /// <param name="id">Id of sensor.</param>
        /// <returns>Sensors corresponding to the id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Sensor>> GetSensorById(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);

            if (sensor == null)
            {
                return NotFound();
            }

            return sensor;
        }

		/// <summary>
		/// Gets all sensors of a device.
		/// </summary>
		/// <param name="deviceId">Device id.</param>
		/// <returns>all the sensor stored for a device</returns>
		[HttpGet("device/{deviceId}")]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensorsByDeviceId(int deviceId)
        {
            if (!_context.Sensors.Any(sen => sen.DeviceId == deviceId))
            {
                return NotFound();
            }

            return await _context.Sensors.Where(sen => sen.DeviceId == deviceId).ToListAsync();
        }
    }
}