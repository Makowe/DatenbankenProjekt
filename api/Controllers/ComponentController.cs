using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ComponentController : ControllerBase {
        [HttpGet("{recipeId}")]
        static async public Task<List<Component>> GetComponentsOfRecipe(int recipeId) {
            List<Component> components = new List<Component>();
            try {
                var query = @$"SELECT component.id, component.name, amount, unit.name, unit.shortname FROM recipe JOIN component_in_recipe JOIN component JOIN unit
                            WHERE component.id = component_in_recipe.component
                              and recipe.id = component_in_recipe.recipe
                              and unit.id = component_in_recipe.unit
                              and recipe.id = {recipeId};";
                MySqlDataReader reader = await DbConnection.ExecuteQuery(query);

                if(reader.HasRows) {
                    while(await reader.ReadAsync()) {
                        var id = (int)reader.GetValue(0);
                        var name = (string)reader.GetValue(1);
                        var amount = (double)reader.GetValue(2);
                        var unitName = (string)reader.GetValue(3);
                        var unitShortname = (string)reader.GetValue(4);
                        components.Add(new Component(id, name, amount, unitName, unitShortname));
                    }
                }
            }
            catch { }
            return components;
        }
    }
}
