using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Database;
using api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace api.Processors {
    [ApiController]
    [Route("[controller]")]
    public class InstructionProcessor {

        /// <summary>
        /// Method gets all instructions added to a given recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <returns>List of instructions</returns>
        static async public Task<List<Instruction>> GetInstructionsByRecipe(int recipeId) {
            List<Instruction> instructions = new List<Instruction>();
            try {
                var query = $"SELECT step,description FROM recipe JOIN instruction WHERE recipe.id = instruction.recipe and id = {recipeId};";
                var reader = await DbConnection.ExecuteQuery(query);

                if(reader.HasRows) {
                    while(await reader.ReadAsync()) {
                        var step = (int)reader.GetValue(0);
                        var description = (string)reader.GetValue(1);
                        instructions.Add(new Instruction(step, description));
                    }
                }
            }
            catch { }
            return instructions;
        }

        /// <summary>
        /// removes all references to instructions from a given recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <returns>Response Message that specifies if the instruction was successful</returns>
        static public async Task<Response> RemoveAllInstructionsFromRecipe(int recipeId) {
            try {
                var query = @$"DELETE FROM instruction
                                WHERE
                                    recipe = {recipeId};";
                await DbConnection.ExecuteQuery(query);
                return new Response(1, "");
            }
            catch { return new Response(0, "Anweisung konnte nicht ausgeführt werden"); }
        }

        /// <summary>
        /// Method adds instructions to a given recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <param name="instructions">List of instructions</param>
        /// <returns>Response Message that specifies if the instruction was successful</returns>
        static public async Task<Response> AddInstructionsToRecipe(int recipeId, List<Instruction> instructions) {
            try {
                for(int i = 0; i < instructions.Count; i++) {

                    var query = @$"INSERT INTO instruction (recipe, step, description)
                                    VALUES ({recipeId}, {instructions[i].Step}, '{instructions[i].Description}');";
                    await DbConnection.ExecuteQuery(query);
                }
                return new Response(1, "");
            }
            catch { return new Response(0, "Anweisung konnte nicht ausgeführt werden"); }
        }
    }
}
