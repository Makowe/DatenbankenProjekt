using api.Model;
using api.Database;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase {
        
        public TagController() { }

        /// <summary>
        /// Method generates a list of all tags included in a recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <returns>List of the tags</returns>
        [HttpGet("Recipe/{recipeId}")]
        async public Task<List<Tag>> GetTagsOfRecipe(int recipeId) {
            List<Tag> tags = new List<Tag>();
            DbConnection db = new DbConnection();
            try {
                //TODO Update Query
                var query = @$"SELECT tag.id, tag.name
                            FROM recipe JOIN tag_in_recipe JOIN tag
                            WHERE tag.id = tag_in_recipe.tag
                              and recipe.id = tag_in_recipe.recipe
                              and recipe.id = {recipeId};";
                
                var reader = await db.ExecuteQuery(query);

                if(reader.HasRows) {
                    while(await reader.ReadAsync()) {
                        var id = (int)reader.GetValue(0);
                        var name = (string)reader.GetValue(1);
                        tags.Add(new Tag(id, name));
                    }
                }                
                return tags;
            }
            catch { return null; }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method generates a list of all tags stored in the database
        /// </summary>
        /// <returns>List of the tags</returns>
        [HttpGet]
        public async Task<List<Tag>> GetAllTags() {
            List<Tag> tags = new List<Tag>();
            DbConnection db = new DbConnection();
            try {
                var query = $"SELECT * FROM tag;";
                var reader = await db.ExecuteQuery(query);

                if(reader.HasRows) {
                    while(await reader.ReadAsync()) {
                        var id = (int)reader.GetValue(0);
                        var name = (string)reader.GetValue(1);
                        tags.Add(new Tag(id, name));
                    }
                }
                return tags;
            }
            catch { return null; }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method searches for a single tags by its id</summary>
        /// <param name="id">Id to search for</param>
        /// <returns>The tag with the id. Returns <c>null</c> if the tag does not exist</returns>
        [HttpGet("{id}")]
        public async Task<Tag> GetTagById(int id) {
            DbConnection db = new DbConnection();
            try {
                var query = $"SELECT * FROM tag WHERE id = {id};";
                var reader = await db.ExecuteQuery(query);

                if(reader.HasRows) {
                    await reader.ReadAsync();
                    var name = (string)reader.GetValue(1);
                    return new Tag(id, name);
                }
                else { return null; }
            }
            catch { return null; }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method searches for a tag by its name
        /// </summary>
        /// <param name="name">name of the tag</param>
        /// <returns>The tag with the id. Returns <c>null</c> if the tag does not exist</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Tag> GetTagByName(string name) {
            DbConnection db = new DbConnection();
            try {
                var query = $"SELECT * FROM tag WHERE name = \"{name}\";";
                var reader = await db.ExecuteQuery(query);

                if(reader.HasRows) {
                    await reader.ReadAsync();
                    var id = (int)reader.GetValue(0);
                    return new Tag(id, name);
                }
                else { return null; }
            }
            catch { return null; }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method Adds a single tag to the database
        /// </summary>
        /// <param name="component">tag object to add. The id of the object gets ignored because the DB assigns the id automatically</param>
        /// <returns>Response Message with the id of the new stored tag. Returns a Response Message with value 0 if the tag could not be added</returns>
        [HttpPost]
        public async Task<CustomResponse> AddTag(Tag tag) {
            CustomResponse response = await CheckNewTagValid(tag);
            if(response.Value == 0) {
                // component is invalid. return respones message
                return response;
            }
            DbConnection db = new DbConnection();
            try {
                var query = $"INSERT INTO tag (name) VALUES ('{tag.Name.Trim()}');";
                await db.ExecuteQuery(query);
                var storedComponent = await GetTagByName(tag.Name);
                return new CustomResponse((int)storedComponent.Id, $"Tag {tag.Name} erfolgreich hinzugefügt");
            }
            catch { return CustomResponse.ErrorMessage(); }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method changes an existing tag
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<CustomResponse> UpdateTag(Tag tag) {
            CustomResponse response = await CheckExistingTagValid(tag);
            if(response.Value == 0) {
                //component is invalid -> return response message
                return response;
            }
            DbConnection db = new DbConnection();
            try {
                var query = @$"UPDATE tag 
                            SET 
                                name = '{tag.Name.Trim()}'
                            WHERE 
                                id = {tag.Id};";
                await db.ExecuteQuery(query);
                return new CustomResponse((int)tag.Id, $"Tag {tag.Name} erfolgreich bearbeitet");
            }
            catch { return CustomResponse.ErrorMessage(); }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method deletes a tag by its id. It deletes all references in recipes to this tag
        /// </summary>
        /// <param name="id">id of the tag</param>
        /// <returns>Response Message that specifies if the Deletion was successful</returns>
        [HttpDelete("{id}")]
        public async Task<CustomResponse> DeleteTagById(int id) {
            
            Tag existingTag = await GetTagById(id);
            if(existingTag == null) {
                return new CustomResponse(0, "Tag exisitert nicht");
            }
            DbConnection db1 = new DbConnection();
            DbConnection db2 = new DbConnection();
            try {
                var query1 = @$"DELETE FROM tag_in_recipe
                            WHERE
                                tag = {id};";
                await db1.ExecuteQuery(query1);
                var query2 = @$"SET FOREIGN_KEY_CHECKS=0;
                            DELETE FROM tag
                            WHERE
                                id = {id};
                            SET FOREIGN_KEY_CHECKS=1;";
                await db2.ExecuteQuery(query2);
                    
                return new CustomResponse(id, $"Tag erfolgreich gelöscht");
            }
            catch { return CustomResponse.ErrorMessage(); }
            finally { db1.CloseConnection(); db2.CloseConnection(); }
        }

        /// <summary>
        /// Method removes all references to tags in a given recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <returns>Respones Message that specifies if the deleteion was successful</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<CustomResponse> RemoveAllTagsFromRecipe(int recipeId) {
            DbConnection db = new DbConnection(); 
            try {
                var query = @$"DELETE FROM tag_in_recipe
                                WHERE
                                    recipe = {recipeId};";
                
                await db.ExecuteQuery(query);
                return new CustomResponse(1, "");
            }
            catch { return new CustomResponse(0, "Anweisung konnte nicht ausgeführt werden"); }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Adds reference to tags to a given recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <param name="components">List of the tags to add to the recipe</param>
        /// <returns>Respones Message that specifies if the update was successful</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<CustomResponse> AddTagsToRecipe(int recipeId, List<Tag> tags) {
            for(int i = 0; i < tags.Count; i++) {
                CustomResponse response = await AddTagToRecipe(recipeId, tags[i]);
                if(response.Value == 0) { return response; }
            }
            return CustomResponse.SuccessMessage();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<CustomResponse> AddTagToRecipe(int recipeId, Tag tag) {
            DbConnection db = new DbConnection();
            try {
                Tag existingTag = await GetTagByName(tag.Name);
                var query = @$"INSERT INTO tag_in_recipe (recipe, tag)
                                    VALUES ({recipeId}, {existingTag.Id});";

                await db.ExecuteQuery(query);
                return CustomResponse.SuccessMessage();
            }
            catch { return CustomResponse.ErrorMessage(); }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// method checks if a given tag is valid to update in the DB.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>Response Message that specifies if the tag is valid</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<CustomResponse> CheckExistingTagValid(Tag tag) {
            Tag existingTag = await GetTagById((int)tag.Id);
            if(tag.Id == null|| existingTag == null) {
                return new CustomResponse(0, $"Tag {tag.Name} mit Id {tag.Id} exisitert nicht");
            }
            if(tag.Name.Trim() == "") {
                return new CustomResponse(0, $"Tag mit Id {tag.Id} ist unvollständig");
            }
            Tag sameNameTag = await GetTagByName(tag.Name);
            if(sameNameTag != null && sameNameTag.Id != tag.Id) {
                return new CustomResponse(0, "Der Name exisitert bereits für einen anderen Tag");
            }
            return CustomResponse.SuccessMessage();
        }

        /// <summary>
        /// method checks if the given tag is valid to add to DB
        /// </summary>
        /// <returns>Response Message that specifies if the tag is valid</returns>
        [ApiExplorerSettings(IgnoreApi =true)]
        public async Task<CustomResponse> CheckNewTagValid(Tag tag) {
            if(tag.Name.Trim() == "") {
                return new CustomResponse(0, $"Tag ist unvollständig");
            }
            Tag sameNameTag = await GetTagByName(tag.Name);
            if(sameNameTag != null) {
                return new CustomResponse(0, "Der Name exisitert bereits für einen anderen Tag");
            }
            return CustomResponse.SuccessMessage();
        }
    }
}
