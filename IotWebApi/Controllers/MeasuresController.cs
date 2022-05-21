using IotWebApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace IotWebApi.Controllers
{
	/// <summary>
	/// Class that manage requests about measure to the DB
	/// </summary>
	[EnableCors("MyPolicy")]
	[Produces(MediaTypeNames.Application.Json)]
	[Consumes(MediaTypeNames.Application.Json)]
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
		[HttpGet("skip/{skip}/take/{take}")]
		public async Task<IActionResult> GetMeasures(int skip, int take)
		{
			List<Measure> measures = await _context.Measures.Skip(skip).Take(take).ToListAsync();

			if (measures is null || measures.Count == 0)
			{
				NotFound();
			}

			return Ok(measures);
		}

		/// <summary>
		/// GET: api/Measure/5
		/// </summary>
		/// <param name="id">id of a measure</param>
		/// <returns>measure with the corresponding id</returns>
		[HttpGet("{id}")]
		public async Task<ActionResult<Measure>> GetMeasure(int id)
		{
			ActionResult<Measure> sensor = await _context.Measures.FindAsync(id);

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
		[HttpGet("sensor/{sensorId}")]
		public async Task<ActionResult<IEnumerable<Measure>>> GetMeasureBySensorId(int sensorId)
		{
			IQueryable<Measure> measures = _context.Measures.Where(mes => mes.SensorId == sensorId);

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
		[HttpGet("sensor/{sensorId}/date/{date}")]
		public async Task<ActionResult<IEnumerable<Measure>>> GetSensorsBySensorIdAndDate(int sensorId, DateTime date)
		{

			IEnumerable<Measure> measures = await _context.Measures
				.Where(
					mes => mes.SensorId == sensorId &&
					mes.DateTime.Date == date.Date)
				.ToListAsync();

			if (measures == null)
			{
				return NotFound();
			}

			return Ok(measures);
		}

		/// <summary>
		/// GET: api/Measure/Sensor/26/From/2020-05-26/To/2020-06-01
		/// return measure for a sensor for an interval (the dates are included)
		/// </summary>
		/// <param name="sensorId">sensor id</param>
		/// <param name="startDate">date of beginning</param>
		/// <param name="endDate">date of end</param>
		/// <returns>List of measures for the interval</returns>
		[HttpGet("sensor/{sensorId}/from/{startDate}/to/{endDate}")]
		public async Task<IActionResult> GetSensorsBySensorIdFromDateToDate(int sensorId, DateTime startDate, DateTime endDate)
		{
			var intervalEnd = endDate.AddDays(1);

			IEnumerable<Measure> measures = await _context.Measures
				.Where(
					mes => mes.SensorId == sensorId &&
					mes.DateTime >= startDate &&
					mes.DateTime <= intervalEnd)
				.ToListAsync();

			if (measures == null)
			{
				return NotFound();
			}

			return Ok(measures);
		}

		/// <summary>
		/// GET: api/Measure/Sensor/26/Date/2020-05-26
		/// </summary>
		/// <param name="sensorId">sensor id</param>
		/// <param name="month">indice of the month (1: January ..)</param>
		/// <returns>List of measure for a sensor during a month</returns>
		[HttpGet("sensor/{sensorId}/month/{month}")]
		public async Task<IActionResult> GetSensorsBySensorIdAndMonth(int sensorId, int month)
		{

			IEnumerable<Measure> measures = await _context.Measures
				.Where(
					mes => mes.SensorId == sensorId &&
					mes.DateTime.Month == month)
				.ToListAsync();

			if (measures == null)
			{
				return NotFound();
			}

			return Ok(measures);
		}


		/// <summary>
		/// GET: api/Measure/Sensor/18/GetLastMeasure
		/// </summary>
		/// <param name="sensorId">sensor id</param>
		/// <returns>the last measure of a sensor</returns>
		[HttpGet("sensor/{sensorId}/getLastMeasure")]
		public async Task<IActionResult> GetLastMeasure(int sensorId)
		{
			Measure sensorMeasure = await _context.Measures
				.Where(mes => mes.SensorId == sensorId)
				.OrderByDescending(mes => mes.DateTime)
				.FirstOrDefaultAsync();

			if(sensorMeasure is null)
			{
				return NotFound();
			}

			return Ok(sensorMeasure);
		}

	}
}
