using System.Threading.Tasks;
using Dapper;
using IotWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Storage.Models;

namespace IotWebApi.Controllers
{
    [Route("api/Sensor")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly ConnectionStrings _connectionString;

        public SensorsController(ConnectionStrings connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Return the sensors list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Sensor vm)
        {
            return await Task.Run(() =>
            {
                using (var connection = new MySqlConnection(_connectionString.MySql))
                {
                    var sql = @"SELECT * FROM Sensors";

                    var query = connection.Query<Sensor>(sql, vm, commandTimeout: 30);

                    return Ok(query);
                }

            });
        }
    }
}