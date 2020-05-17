using System.Threading.Tasks;
using Dapper;
using IotWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Storage.Models;

namespace IotWebApi.Controllers
{
    [Route("api/Device")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly ConnectionStrings _connectionString;

        public DevicesController(ConnectionStrings connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Return the sensors list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Device vm)
        {
            return await Task.Run(() =>
            {
                using (var connection = new MySqlConnection(_connectionString.MySql))
                {
                    var sql = @"SELECT * FROM Devices";

                    var query = connection.Query<Device>(sql, vm, commandTimeout: 30);

                    return Ok(query);
                }

            });
        }
    }
}
