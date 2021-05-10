using api.Controllers;
using api.Database;
using api.Model;
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

        static async public Task<Recipe> GetRecipeById(int id) {
            Recipe recipe = new Recipe();
            try {
                string query = $"SELECT * FROM recipe WHERE id = {id};";
                var reader = await DbConnection.ExecuteQuery(query);

                if(reader.HasRows) {
                    await reader.ReadAsync();
                    recipe.Id = (int)reader.GetValue(0);
                    recipe.Name = (string)reader.GetValue(1);
                    recipe.People = (int)reader.GetValue(2);
                    recipe.Instructions = await InstructionProcessor.GetInstructionsByRecipe(id);
                    recipe.Components = await ComponentProcessor.GetComponentsOfRecipe(id);
                    return recipe;
                }
                else {
                    return null;
                }

            }
            catch { return null;}
        }

        static async public Task<Recipe> GetRecipeByName(string name) {
            Recipe recipe = new Recipe();
            try {

                string query = $"SELECT * FROM recipe WHERE name = '{name}';";
                var reader = await DbConnection.ExecuteQuery(query);

                if(reader.HasRows) {
                    await reader.ReadAsync();
                    recipe.Id = (int)reader.GetValue(0);
                    recipe.Name = (string)reader.GetValue(1);
                    recipe.People = (int)reader.GetValue(2);
                    recipe.Instructions = await InstructionProcessor.GetInstructionsByRecipe((int)recipe.Id);
                    recipe.Components = await ComponentProcessor.GetComponentsOfRecipe((int)recipe.Id);
                    return recipe;
                }
                else {
                    return null;
                }

            }
            catch { return null; }

        }

        static async public Task<Response> AddRecipe(Recipe newRecipe) {
            if(newRecipe.Name == "" || newRecipe.Components.Count == 0) {
                return new Response(0, "Rezept ist unvollständig");
            }

            var sameNameRecipe = await GetRecipeByName(newRecipe.Name);
            if(sameNameRecipe != null) {
                return new Response(0, "Der Name exisitert bereits");
            }

            try {
                var query1 = @$"INSERT INTO recipe (name, people)
                                VALUES
                                    ('{newRecipe.Name}', {newRecipe.People});";
                await DbConnection.ExecuteQuery(query1);

                int id = (int)(await GetRecipeByName(newRecipe.Name)).Id;

                bool success = true;

                success = success && await ComponentProcessor.AddComponentsToRecipe(id, newRecipe.Components);
                success = success && await InstructionProcessor.AddInstructionsToRecipe(id, newRecipe.Instructions);

                if(success) { return new Response(id, $"Zutat {newRecipe.Name} erfolgreich hinzugefügt"); }
                else { return new Response(0, "Anweisung konnte nicht ausgeführt werden"); }
            }
            catch { return new Response(0, "Anweisung konnte nicht ausgeführt werden"); }
        }

        static async public Task<Response> UpdateRecipe(Recipe updatedRecipe) {
            int id = (int)updatedRecipe.Id;

            if(updatedRecipe.Name == "" || updatedRecipe.Components.Count == 0) {
                return new Response(0, "Rezept ist unvollständig"); 
            }
            
            var sameNameRecipe = await GetRecipeByName(updatedRecipe.Name);
            if(sameNameRecipe != null && sameNameRecipe.Id != id) {
                return new Response(0, "Der Name exisitert bereits");
            }

            try {
                Recipe existingRecipe = await GetRecipeById(id);
                if(existingRecipe != null) {
                    var query1 = @$"UPDATE recipe
                                SET 
                                    name = '{updatedRecipe.Name}',
                                    people = {updatedRecipe.People}
                                WHERE 
                                    id = {id};";
                    await DbConnection.ExecuteQuery(query1);

                    bool success = true;

                    success = success && await ComponentProcessor.RemoveAllComponentsFromRecipe(id);
                    success = success && await ComponentProcessor.AddComponentsToRecipe(id, updatedRecipe.Components);
                    success = success && await InstructionProcessor.RemoveAllInstructionsFromRecipe(id);
                    success = success && await InstructionProcessor.AddInstructionsToRecipe(id, updatedRecipe.Instructions);

                    if(success) { return new Response(id, $"Zutat {updatedRecipe.Name} erfolgreich bearbeitet"); }
                    else { return new Response(0, "Anweisung konnte nicht ausgeführt werden"); }
                }  
                else {
                    return new Response(0, "Zutat exisitert nicht");
                }
            }
            catch { return new Response(0, "Anweisung konnte nicht ausgeführt werden"); }
        }

        static async public Task<Response> DeleteRecipeById(int id) {
            try {
                Recipe existingRecipe = await GetRecipeById(id);
                if(existingRecipe != null) {
                    var query1 = @$"DELETE FROM instruction
                                    WHERE 
                                        recipe = {id};";
                    await DbConnection.ExecuteQuery(query1);
                    var query2 = @$"DELETE FROM recipe 
                                    WHERE
                                        id = {id};";
                    await DbConnection.ExecuteQuery(query2);

                    return new Response(id, $"Rezept erfolgreich gelöscht");
                }
                else {
                    return new Response(0, "Rezept exisitert nicht");
                }
            }
            catch { }
            return new Response(0, "Anweisung konnte nicht ausgeführt werden");
        }
    }
}
