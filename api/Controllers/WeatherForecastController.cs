using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        [HttpGet]
        async public Task<List<string>> Get()
        {
            using var connection = new MySqlConnection("server=127.0.0.1; user=root; password=nico; database=project_9275184");
            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT * FROM recipe;", connection);
            using var reader = await command.ExecuteReaderAsync();
            List<string> recipes = new List<string>();
            while (await reader.ReadAsync()) {
                recipes.Add(reader.GetValue(1).ToString());
            }
            return recipes;
        }
    }
}
