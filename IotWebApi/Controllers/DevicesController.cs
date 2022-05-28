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
	[Route("api/devices")]
	[ApiController]
	public class DevicesController : ControllerBase
	{
		private readonly DbSensorsContext _context;

		/// <summary>
		/// Creates a new instance of <see cref="DevicesController"./>
		/// </summary>
		/// <param name="context">Context of the DB</param>
		public DevicesController(DbSensorsContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Gets all devices.
		/// </summary>
		/// <returns>List of devices.</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetDevices()
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
		public async Task<IActionResult> GetDevice(int id)
		{
			Device device = await _context.Devices
				.Include(d => d.Sensors)
				.FirstAsync(d => d.DeviceId == id);

			if (device == null)
			{
				return NotFound();
			}

			return Ok(device);
		}

		/// <summary>
		/// Gets a device by name.
		/// </summary>
		/// <param name="name">Name of the requested device.</param>
		/// <returns>Device corresponding to the name.</returns>
		[HttpGet("name/{name}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetDeviceByName(string name)
		{
			Device device = await _context.Devices
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
