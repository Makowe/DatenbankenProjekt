using api.Model;
using api.Database;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase {

        private readonly ComponentController componentController;
        private readonly InstructionController instructionController;

        public RecipeController() {
            this.componentController = new ComponentController();
            this.instructionController= new InstructionController();
        }

        /// <summary>
        /// Method gets all recipes stored in the database
        /// </summary>
        /// <returns>List of all Recipe. components and instructions are set to null for performance</returns>
        [HttpGet]
        async public Task<List<Recipe>> GetAllRecipes() {
            List<Recipe> recipes = new List<Recipe>();
            try {
                string query = "SELECT * FROM recipe;";
                var reader = await DbConnection.ExecuteQuery(query);
                
                if(reader.HasRows) {
                    while(await reader.ReadAsync()) {
                        var id = (int)reader.GetValue(0);
                        var name = (string)reader.GetValue(1);
                        var people = (int)reader.GetValue(2);
                        recipes.Add(new Recipe(id, name, people));
                    }
                }
                return recipes;
            }
            catch { return null; }
        }

        /// <summary>
        /// Method gets a single recipe by a given id
        /// </summary>
        /// <param name="id">id of the recipe</param>
        /// <returns>object of the recipe. Returns null if the recipe does not exist</returns>
        [HttpGet("{id}")]
        async public Task<Recipe> GetRecipeById(int id) {
            Recipe recipe = new Recipe();
            try {
                string query = $"SELECT * FROM recipe WHERE id = {id};";
                var reader = await DbConnection.ExecuteQuery(query);

                if(reader.HasRows) {
                    await reader.ReadAsync();
                    recipe.Id = (int)reader.GetValue(0);
                    recipe.Name = (string)reader.GetValue(1);
                    recipe.People = (int)reader.GetValue(2);
                    recipe.Instructions = await this.instructionController.GetInstructionsByRecipe(id);
                    recipe.Components = await this.componentController.GetComponentsOfRecipe(id);
                    return recipe;
                }
                else { return null; }
            }
            catch { return null;}
        }

        /// <summary>
        /// Method gets a single recipe by a given name
        /// </summary>
        /// <param name="name">name of the recipe</param>
        /// <returns>object of the recipe. Returns null if the recipe does not exist</returns>
        async public Task<Recipe> GetRecipeByName(string name) {
            Recipe recipe = new Recipe();
            try {
                string query = $"SELECT * FROM recipe WHERE name = '{name}';";
                var reader = await DbConnection.ExecuteQuery(query);

                if(reader.HasRows) {
                    await reader.ReadAsync();
                    recipe.Id = (int)reader.GetValue(0);
                    recipe.Name = (string)reader.GetValue(1);
                    recipe.People = (int)reader.GetValue(2);
                    recipe.Instructions = await this.instructionController.GetInstructionsByRecipe((int)recipe.Id);
                    recipe.Components = await this.componentController.GetComponentsOfRecipe((int)recipe.Id);
                    return recipe;
                }
                else { return null; }
            }
            catch { return null; }
        }

        /// <summary>
        /// Method adds a Recipe to the database
        /// </summary>
        /// <param name="recipe">Recipe</param>
        /// <returns>Response Message that specifies if the instruction was successful</returns>
        [HttpPost]
        async public Task<CustomResponse> AddRecipe(Recipe recipe) {
            CustomResponse response = await CheckNewRecipeValid(recipe);
            if(response.Value == 0) { return response; }

            try {
                var query1 = @$"INSERT INTO recipe (name, people)
                                VALUES
                                    ('{recipe.Name.Trim()}', {recipe.People});";
                await DbConnection.ExecuteQuery(query1);

                int id = (int)(await GetRecipeByName(recipe.Name)).Id;

                await this.componentController.AddComponentsToRecipe(id, recipe.Components);
                await this.instructionController.AddInstructionsToRecipe(id, recipe.Instructions);

                return new CustomResponse(id, $"Rezept {recipe.Name} erfolgreich hinzugefügt");
            }
            catch { return CustomResponse.ErrorMessage(); }
        }

        /// <summary>
        /// Method updated a given Recipe
        /// </summary>
        /// <param name="recipe">updated Recipe</param>
        /// <returns>Response Message that specifies if the instruction was successful</returns>
        [HttpPut]
        async public Task<CustomResponse> UpdateRecipe(Recipe recipe) {

            CustomResponse response = await CheckExistingRecipeValid(recipe);
            if(response.Value == 0) { return response; }

            try {
                var query1 = @$"UPDATE recipe
                            SET 
                                name = '{recipe.Name.Trim()}',
                                people = {recipe.People}
                            WHERE 
                                id = {recipe.Id};";
                await DbConnection.ExecuteQuery(query1);

                await this.componentController.RemoveAllComponentsFromRecipe((int)recipe.Id);
                await this.componentController.AddComponentsToRecipe((int)recipe.Id, recipe.Components);
                await this.instructionController.RemoveAllInstructionsFromRecipe((int)recipe.Id);
                await this.instructionController.AddInstructionsToRecipe((int)recipe.Id, recipe.Instructions);

                return new CustomResponse((int)recipe.Id, $"Zutat {recipe.Name} erfolgreich bearbeitet");
            }
            catch { return CustomResponse.ErrorMessage(); }
        }

        /// <summary>
        /// Method deletes a recipe by its id
        /// </summary>
        /// <param name="id">id of the recipe</param>
        /// <returns>Response Message if the deletion was successful</returns>
        [HttpDelete("{id}")]
        async public Task<CustomResponse> DeleteRecipeById(int id) {
            Recipe existingRecipe = await GetRecipeById(id);
            if(existingRecipe == null) {
                return new CustomResponse(0, "Rezept exisitert nicht");
            }
            try {
                await this.instructionController.RemoveAllInstructionsFromRecipe(id);
                await this.componentController.RemoveAllComponentsFromRecipe(id);
                var query3 = @$"DELETE FROM recipe 
                                WHERE
                                    id = {id};";
                await DbConnection.ExecuteQuery(query3);

                return new CustomResponse(id, $"Rezept erfolgreich gelöscht");
            }
            catch { return CustomResponse.ErrorMessage(); }
        }
        
        /// <summary>
        /// method takes a recipe object and checks if the recipe can be added as a new recipe to the DB.
        /// </summary>
        /// <returns>Returns a Response object that specifies if the recipe is valid</returns>
        async public Task<CustomResponse> CheckNewRecipeValid(Recipe recipe) {
            try {
                if(recipe.Name.Trim() == "" || recipe.Components.Count == 0) {
                    return new CustomResponse(0, "Rezept ist unvollständig");
                }

                Recipe sameNameRecipe = await GetRecipeByName(recipe.Name.Trim());
                if(sameNameRecipe != null) {
                    return new CustomResponse(0, "Der Name exisitert bereits für ein anderes Rezept");
                }

                //check if all components are valid
                int i = 0;
                while(i<recipe.Components.Count) {
                    CustomResponse componentValid = await this.componentController.CheckExistingComponentValid(recipe.Components[i]);
                    if(componentValid.Value == 0) {
                        return componentValid;
                    }
                    i++;
                }
                // all checks passed. The recipe is valid
                return CustomResponse.SuccessMessage();
            }
            catch { return CustomResponse.ErrorMessage(); }
        }

        /// <summary>
        /// methods takes a recipe object and checks if the recipe can be updated in the DB.
        /// </summary>
        /// <returns>Returns a Response object that specifies if the recipe is valid</returns>
        async public Task<CustomResponse> CheckExistingRecipeValid(Recipe recipe) {
            try {
                if(recipe.Id == null || await GetRecipeById((int)recipe.Id) == null) {
                    return new CustomResponse(0, "Rezept ist nicht vorhanden");
                }
            
                if(recipe.Name == "" || recipe.Components.Count == 0) {
                    return new CustomResponse(0, "Rezept ist unvollständig");
                }

                Recipe sameNameRecipe = await GetRecipeByName(recipe.Name);
                if(sameNameRecipe != null && sameNameRecipe.Id != recipe.Id) {
                    return new CustomResponse(0, "Der Name exisitert bereits für ein anderes Rezept");
                }

                //check if all components are valid
                int i = 0;
                while(i < recipe.Components.Count) {
                    CustomResponse response = await this.componentController.CheckExistingComponentValid(recipe.Components[i]);
                    if(response.Value == 0) {
                        return response;
                    }
                    i++;
                }
                // all checks passed. The recipe is valid
                return CustomResponse.SuccessMessage();
            }
            catch { return CustomResponse.ErrorMessage(); }
        }
    }
}
