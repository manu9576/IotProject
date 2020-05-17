using Dapper;
using IotWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IotWebApi.Controllers
{
    [Route("api/Measurments")]
    [ApiController]
    public class MeasuresController : ControllerBase
    {
        private readonly ConnectionStrings _connectionString;

        public MeasuresController(ConnectionStrings connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Return the sensors list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Measure vm)
        {
            return await Task.Run(() =>
            {
                using (var connection = new MySqlConnection(_connectionString.MySql))
                {
                    var sql = @"SELECT * FROM Measures";

                    var query = connection.Query<Measure>(sql, vm, commandTimeout: 30);

                    return Ok(query);
                }

            });
        }
    }
}
