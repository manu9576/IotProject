using IotWebApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IotWebApi.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/Measure")]
    [ApiController]
    public class MeasuresController : ControllerBase
    {
        private readonly DbSensorsContext _context;

        public MeasuresController(DbSensorsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Measure
        /// Return the measures list
        /// ALL MEASURES!! SHOULD BE LONG
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Measure>>> GetMeasures()
        {
            return await _context.Measures.ToListAsync();
        }

        /// <summary>
        /// GET: api/Measure/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Measure>> GetMeasure(int id)
        {
            var sensor = await _context.Measures.FindAsync(id);

            if (sensor == null)
            {
                return NotFound();
            }

            return sensor;
        }

        /// <summary>
        /// GET: api/Measures/sensor/5
        /// </summary>
        /// <param name="sensorId"></param>
        /// <returns></returns>
        [HttpGet("Sensor/{sensorId}")]
        public async Task<ActionResult<IEnumerable<Measure>>> GetMeasureBySensorId(int sensorId)
        {
            var measures =  _context.Measures.Where(mes => mes.SensorId == sensorId);

            if (measures == null)
            {
                return NotFound();
            }

            return await measures.ToListAsync();
        }

        /// <summary>
        /// GET: api/Sensor/26/Date/2020-05-26
        /// </summary>
        /// <param name="sensorId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("Sensor/{sensorId}/Date/{date}")]
        public async Task<ActionResult<IEnumerable<Measure>>> GetSensorsBySensorIdAndDate(int sensorId, DateTime date)
        {

            var measures = _context.Measures.Where(mes => mes.SensorId == sensorId && mes.DateTime.Date == date.Date);

            if (measures == null)
            {
                return NotFound();
            }

            return await measures.ToListAsync();
        }

        /// <summary>
        /// GET: api/Sensor/26/Date/2020-05-26
        /// </summary>
        /// <param name="sensorId"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpGet("Sensor/{sensorId}/Month/{month}")]
        public async Task<ActionResult<IEnumerable<Measure>>> GetSensorsBySensorIdAndMonth(int sensorId, int month)
        {

            var measures = _context.Measures.Where(mes => mes.SensorId == sensorId && mes.DateTime.Month == month);

            if (measures == null)
            {
                return NotFound();
            }

            return await measures.ToListAsync();
        }

    }
}
