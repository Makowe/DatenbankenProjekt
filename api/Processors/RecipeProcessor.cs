using api.Controllers;
using api.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Processors {
    public class RecipeProcessor {
        static async public Task<List<Recipe>> GetAllRecipes() {
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
            }
            catch { }
            return recipes;
        }

        static async public Task<Recipe> GetRecipeDetails(int recipeId) {
            Recipe recipe = new Recipe();
            try {
                string query = $"SELECT * FROM recipe WHERE id = {recipeId};";
                var reader = await DbConnection.ExecuteQuery(query);

                if(reader.HasRows) {
                    await reader.ReadAsync();
                    recipe.Id = (int)reader.GetValue(0);
                    recipe.Name = (string)reader.GetValue(1);
                    recipe.People = (int)reader.GetValue(2);
                }

                recipe.Instructions = await InstructionProcessor.GetInstructionsByRecipe(recipeId);
                recipe.Components = await ComponentProcessor.GetComponentsOfRecipe(recipeId);
            }
            catch { }
            return recipe;
        }
    }
}
