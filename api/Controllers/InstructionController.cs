using api.Model;
using api.Database;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class InstructionController : ControllerBase {

        /// <summary>
        /// Method gets all instructions added to a given recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <returns>List of instructions</returns>
        [HttpGet("Recipe/{recipeId}")]
        async public Task<List<Instruction>> GetInstructionsByRecipe(int recipeId) {
            List<Instruction> instructions = new List<Instruction>();
            DbConnection db = new DbConnection();
            try {
                var query = $"SELECT step,description FROM recipe JOIN instruction WHERE recipe.id = instruction.recipe and id = {recipeId};";
                var reader = await db.ExecuteQuery(query);

                if(reader.HasRows) {
                    while(await reader.ReadAsync()) {
                        var step = (int)reader.GetValue(0);
                        var description = (string)reader.GetValue(1);
                        instructions.Add(new Instruction(step, description));
                    }
                }
            }
            catch { }
            finally { db.CloseConnection(); }
            return instructions;
        }

        /// <summary>
        /// removes all references to instructions from a given recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <returns>Response Message that specifies if the instruction was successful</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<CustomResponse> RemoveAllInstructionsFromRecipe(int recipeId) {
            DbConnection db = new DbConnection();
            try {
                var query = @$"DELETE FROM instruction
                                WHERE
                                    recipe = {recipeId};";
                await db.ExecuteQuery(query);
                return new CustomResponse(1, "");
            }
            catch { return CustomResponse.ErrorMessage(); }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method adds instructions to a given recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <param name="instructions">List of instructions</param>
        /// <returns>Response Message that specifies if the instruction was successful</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<CustomResponse> AddInstructionsToRecipe(int recipeId, List<Instruction> instructions) {
            for(int i = 0; i < instructions.Count; i++) {
                CustomResponse response = await AddInstructionToRecipe(recipeId, instructions[i]);
                if(response.Value == 0) { return response; }
            }
            return CustomResponse.SuccessMessage();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<CustomResponse> AddInstructionToRecipe(int recipeId, Instruction instruction) {
            DbConnection db = new DbConnection();
            try {
                var query = @$"INSERT INTO instruction (recipe, step, description)
                                    VALUES ({recipeId}, {instruction.Step}, '{instruction.Description}');";
                await db.ExecuteQuery(query);
                return CustomResponse.SuccessMessage();
            }
            catch { return CustomResponse.ErrorMessage(); }
            finally { db.CloseConnection(); }
        }
    }
}
