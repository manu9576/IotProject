using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using IotWebApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storage.Models;

namespace IotWebApi.Controllers
{
	/// <summary>
	/// Class that manage the request about device to the DB
	/// </summary>
	[EnableCors("MyPolicy")]
	[Produces(MediaTypeNames.Application.Json)]
	[Consumes(MediaTypeNames.Application.Json)]
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
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
		{
			return Ok(await _context.Devices.ToListAsync());
		}

		/// <summary>
		/// Gets a device by its Id.
		/// </summary>
		/// <param name="id">Device's Id.</param>
		/// <returns>Device corresponding to the id</returns>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IDtoDevice>> GetDevice(int id)
		{
			IDtoDevice device = await _context.Devices
				.Include(d => d.Sensors)
				.FirstAsync(d => d.DeviceId == id);

			if (device == null)
			{
				return NotFound();
			}

			return Ok(device);
		}

		/// <summary>
		/// GET: api/Device/Name/name
		/// </summary>
		/// <param name="name">name of the device</param>
		/// <returns>Device corresponding to the name</returns>
		[HttpGet("Name/{name}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IDtoDevice>> GetDeviceByName(string name)
		{
			IDtoDevice device = await _context.Devices
				.Include(d => d.Sensors)
				.FirstAsync(dev => dev.Name == name);

			if (device == null)
			{
				return NotFound();
			}

			return Ok(device);
		}
	}
}
