using IotWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storage.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace IotWebApi.Controllers
{
    [Route("api/Measures")]
    [ApiController]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MeasuresController : ControllerBase
    {
        private readonly DbSensorsContext _context;

        public MeasuresController(DbSensorsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Return the sensors list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Measure>>> GetMeasures()
        {
            return await _context.Measures.ToListAsync();
        }

        // GET: api/Measures/5
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
    }
}
