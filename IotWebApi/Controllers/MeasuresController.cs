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
	[Route("api/measures")]
	[ApiController]
	public class MeasuresController : ControllerBase
	{
		private readonly DbSensorsContext _context;

		/// <summary>
		/// Creates a new instance of <see cref="MeasuresController"./>
		/// </summary>
		/// <param name="context">Context of the DB.</param>
		public MeasuresController(DbSensorsContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Returns the measures list.
		/// </summary>
		/// <returns>List of stored measures</returns>
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
		/// Gets a measure by its id.
		/// </summary>
		/// <param name="id">Id of a measure.</param>
		/// <returns>Measure with the corresponding id.</returns>
		[HttpGet("{id}")]
		public async Task<IActionResult> GetMeasure(int id)
		{
			Measure sensor = await _context.Measures.FirstOrDefaultAsync(m => m.MeasureId == id);
			 
			if (sensor is null)
			{
				return NotFound();
			}

			return Ok(sensor);
		}

		/// <summary>
		/// GET: api/Measure/sensor/5
		/// </summary>
		/// <param name="sensorId"></param>
		/// <returns></returns>
		[HttpGet("sensor/{sensorId}")]
		public async Task<IActionResult> GetMeasureBySensorId(int sensorId)
		{
			List<Measure> measures = await _context.Measures.Where(mes => mes.SensorId == sensorId).ToListAsync();

			if (measures is null || measures.Count == 0)
			{
				return NotFound();
			}

			return Ok(measures);
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

			List<Measure> measures = await _context.Measures
				.Where(
					mes => mes.SensorId == sensorId &&
					mes.DateTime.Date == date.Date)
				.ToListAsync();

			if (measures is  null || measures.Count == 0)
			{
				return NotFound();
			}

			return Ok(measures);
		}

		/// <summary>
		/// Returns measures for a sensor for an interval (the dates are included).
		/// </summary>
		/// <param name="sensorId">Sensor id</param>
		/// <param name="startDate">Start date.</param>
		/// <param name="endDate">End date.</param>
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
		/// Gets the measure of a sensor during a mouth.
		/// </summary>
		/// <param name="sensorId">Sensor id.</param>
		/// <param name="month">Indice of the month (1: January ..)</param>
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
		/// Gets the last measure of a sensor.
		/// </summary>
		/// <param name="sensorId">Sensor id.</param>
		/// <returns>Last measure of a sensor.</returns>
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
