using System;
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
        /// GET: api/Sensor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensors()
        {
            return await _context.Sensors.ToListAsync();
        }

        /// <summary>
        /// GET: api/Sensor/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// GET: api/Sensor/device/5
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpGet("Device/{deviceId}")]
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