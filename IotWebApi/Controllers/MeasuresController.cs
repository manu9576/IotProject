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
    /// <summary>
    /// Class that manage requests about measure to the DB
    /// </summary>
    [EnableCors("MyPolicy")]
    [Route("api/Measure")]
    [ApiController]
    public class MeasuresController : ControllerBase
    {
        private readonly DbSensorsContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">context of the DB</param>
        public MeasuresController(DbSensorsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Measure
        /// Return the measures list
        /// ALL MEASURES!! SHOULD BE LONG
        /// </summary>
        /// <returns>List of all stored measures</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IDtoMeasure>>> GetMeasures()
        {
            return await _context.Measures.ToListAsync();
        }

        /// <summary>
        /// GET: api/Measure/5
        /// </summary>
        /// <param name="id">id of a measure</param>
        /// <returns>measure with the corresponding id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IDtoMeasure>> GetMeasure(int id)
        {
            ActionResult<IDtoMeasure> sensor = await _context.Measures.FindAsync(id);

            if (sensor == null)
            {
                return NotFound();
            }

            return sensor;
        }

        /// <summary>
        /// GET: api/Measure/sensor/5
        /// </summary>
        /// <param name="sensorId"></param>
        /// <returns></returns>
        [HttpGet("Sensor/{sensorId}")]
        public async Task<ActionResult<IEnumerable<IDtoMeasure>>> GetMeasureBySensorId(int sensorId)
        {
            IQueryable<IDtoMeasure> measures = _context.Measures.Where(mes => mes.SensorId == sensorId);

            if (measures == null)
            {
                return NotFound();
            }

            return await measures.ToListAsync();
        }

        /// <summary>
        /// GET: api/Measure/Sensor/26/Date/2020-05-26
        /// </summary>
        /// <param name="sensorId">sensor id</param>
        /// <param name="date">date</param>
        /// <returns>List of measures for the sensor at the requested date</returns>
        [HttpGet("Sensor/{sensorId}/Date/{date}")]
        public async Task<ActionResult<IEnumerable<IDtoMeasure>>> GetSensorsBySensorIdAndDate(int sensorId, DateTime date)
        {

            IQueryable<IDtoMeasure> measures = _context.Measures.Where(mes => mes.SensorId == sensorId && mes.DateTime.Date == date.Date);

            if (measures == null)
            {
                return NotFound();
            }

            return await measures.ToListAsync();
        }

        /// <summary>
        /// GET: api/Measure/Sensor/26/From/2020-05-26/To/2020-06-01
        /// return measure for a sensor for an interval (the dates are included)
        /// </summary>
        /// <param name="sensorId">sensor id</param>
        /// <param name="startDate">date of begining</param>
        /// <param name="endDate">date of end</param>
        /// <returns>List of measures for the interval</returns>
        [HttpGet("Sensor/{sensorId}/From/{startDate}/To/{endDate}")]
        public async Task<ActionResult<IEnumerable<IDtoMeasure>>> GetSensorsBySensorIdFromDateToDate(int sensorId, DateTime startDate, DateTime endDate)
        {
            var intervalEnd = endDate.AddDays(1);

            IQueryable<IDtoMeasure> measures = _context.Measures.Where
                (
                mes => mes.SensorId == sensorId &&
                mes.DateTime >= startDate &&
                mes.DateTime <= intervalEnd
                );

            if (measures == null)
            {
                return NotFound();
            }

            return await measures.ToListAsync();
        }

        /// <summary>
        /// GET: api/Measure/Sensor/26/Date/2020-05-26
        /// </summary>
        /// <param name="sensorId">sensor id</param>
        /// <param name="month">indice of the month (1: Juanary ..)</param>
        /// <returns>List of measure for a sensor during a month</returns>
        [HttpGet("Sensor/{sensorId}/Month/{month}")]
        public async Task<ActionResult<IEnumerable<IDtoMeasure>>> GetSensorsBySensorIdAndMonth(int sensorId, int month)
        {

            IQueryable<IDtoMeasure> measures = _context.Measures.Where(mes => mes.SensorId == sensorId && mes.DateTime.Month == month);

            if (measures == null)
            {
                return NotFound();
            }

            return await measures.ToListAsync();
        }


        /// <summary>
        /// GET: api/Measure/Sensor/18/GetLastMeasure
        /// </summary>
        /// <param name="sensorId">sensor id</param>
        /// <returns>the last measure of a sensor</returns>
        [HttpGet("Sensor/{sensorId}/GetLastMeasure")]
        public async Task<ActionResult<IDtoMeasure>> GetLastMeasure(int sensorId)
        {
            var sensorMeasures = _context.Measures.Where(mes => mes.SensorId == sensorId).OrderByDescending(mes => mes.DateTime);

            return await sensorMeasures.FirstOrDefaultAsync();
        }

    }
}
