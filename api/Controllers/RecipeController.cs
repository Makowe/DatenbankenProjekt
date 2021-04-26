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
    public class RecipeController : ControllerBase {

        [HttpGet]
        async public Task<object> GetAllRecipes() {
            try {
                string query = "SELECT * FROM recipe;";
                var reader = await DbConnection.ExecuteQuery(query);

                List<Recipe> recipes = new List<Recipe>();
                if(reader.HasRows) {
                    while(await reader.ReadAsync()) {
                        var id = (int)reader.GetValue(0);
                        var name = (string)reader.GetValue(1);
                        var people = (int)reader.GetValue(2);
                        recipes.Add(new Recipe(id, name, people));
                    }
                }
                return Ok(recipes);
            }
            catch {
                return StatusCode(500);
            }
        }

        [HttpGet("{recipeId}")]
        async public Task<object> GetRecipeDetails(int recipeId) {
            try {
                Recipe recipe = new Recipe();
                recipe.Instructions = await InstructionController.GetInstructionsOfRecipe(recipeId);
                recipe.Components = await ComponentController.GetComponentsOfRecipe(recipeId);
                return Ok(recipe);
            }
            catch {
                return StatusCode(500);
            }
        }
    }
}
