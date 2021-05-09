using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Database;
using api.Model;
using api.Processors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace api.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase {

        [HttpGet]
        async public Task<List<Recipe>> GetAllRecipes() {
            return await RecipeProcessor.GetAllRecipes();
        }
        
        [HttpGet("{recipeId}")]
        async public Task<Recipe> GetRecipeDetails(int recipeId) {
            return await RecipeProcessor.GetRecipeById(recipeId);
        }
        /*

        async public Task<Recipe> PostNewRecipe() {

        }
        */

        [HttpPut]
        async public Task<Response> UpdateRecipe(Recipe recipe) {
            return await RecipeProcessor.UpdateRecipe(recipe);
        }

        [HttpDelete("{id}")]
        async public Task<Response> DeleteRecipe(int id) {
            return await RecipeProcessor.DeleteRecipeById(id);
        }
        /*
        async public Task<Recipe> UpdateRecipe(int id, Recipe updatedRecipe) {

        }
        */
    }
}
