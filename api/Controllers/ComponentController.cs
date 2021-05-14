using api.Model;
using api.Database;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;

namespace api.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentController : ControllerBase {

        private readonly UnitController unitController;

        public ComponentController() {
            this.unitController = new UnitController();
        }

        /// <summary>
        /// Method generates a list of all components included in a recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <returns>List of the components</returns>
        [HttpGet("Recipe/{id}")]
        async public Task<List<Component>> GetComponentsOfRecipe(int recipeId) {
            List<Component> components = new List<Component>();
            DbConnection db = new DbConnection();
            try {
                var query = @$"SELECT component.id, component.name, amount, unit.name, unit.shortname 
                            FROM recipe JOIN component_in_recipe JOIN component JOIN unit
                            WHERE component.id = component_in_recipe.component
                              and recipe.id = component_in_recipe.recipe
                              and unit.id = component_in_recipe.unit
                              and recipe.id = {recipeId};";
                
                MySqlDataReader reader = await db.ExecuteQuery(query);

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
                return components;
            }
            catch { return null; }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method generates a list of all components stored in the database
        /// </summary>
        /// <returns>List of the components</returns>
        [HttpGet]
        public async Task<List<Component>> GetAllComponents() {
            List<Component> components = new List<Component>();
            DbConnection db = new DbConnection();
            try {
                var query = $"SELECT * FROM component;";
                MySqlDataReader reader = await db.ExecuteQuery(query);

                if(reader.HasRows) {
                    while(await reader.ReadAsync()) {
                        var id = (int)reader.GetValue(0);
                        var name = (string)reader.GetValue(1);
                        components.Add(new Component(id, name));
                    }
                }
                return components;
            }
            catch { return null; }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method searches for a single component by its id</summary>
        /// <param name="id">Id to search for</param>
        /// <returns>The component with the id. Returns <c>null</c> if the component does not exist</returns>
        [HttpGet("{id}")]
        public async Task<Component> GetComponentById(int id) {
            DbConnection db = new DbConnection();
            try {
                var query = $"SELECT * FROM component WHERE id = {id};";
                MySqlDataReader reader = await db.ExecuteQuery(query);

                if(reader.HasRows) {
                    await reader.ReadAsync();
                    var name = (string)reader.GetValue(1);
                    return new Component(id, name);
                }
                else { return null; }
            }
            catch { return null; }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method searches for a component by its name
        /// </summary>
        /// <param name="name">name of the component</param>
        /// <returns>The component with the id. Returns <c>null</c> if the component does not exist</returns>
        public async Task<Component> GetComponentByName(string name) {
            DbConnection db = new DbConnection();
            try {
                var query = $"SELECT * FROM component WHERE name = \"{name}\";";
                MySqlDataReader reader = await db.ExecuteQuery(query);

                if(reader.HasRows) {
                    await reader.ReadAsync();
                    var id = (int)reader.GetValue(0);
                    return new Component(id, name);
                }
                else { return null; }
            }
            catch { return null; }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method Adds a single component to the database
        /// </summary>
        /// <param name="component">component object to add. The id of the object gets ignored because the DB assigns the id automatically</param>
        /// <returns>Response Message with the id of the new stored component. Returns a Response Message with value 0 if the component could not be added</returns>
        [HttpPost]
        public async Task<CustomResponse> AddComponent(Component component) {
            CustomResponse response = await CheckNewComponentValid(component);
            if(response.Value == 0) {
                // component is invalid. return respones message
                return response;
            }
            DbConnection db = new DbConnection();
            try {
                var query = $"INSERT INTO component (name) VALUES ('{component.Name.Trim()}');";
                await db.ExecuteQuery(query);
                var storedComponent = await GetComponentByName(component.Name);
                return new CustomResponse((int)storedComponent.Id, $"Zutat {component.Name} erfolgreich hinzugefügt");
            }
            catch { return CustomResponse.ErrorMessage(); }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method changes an existing recipe
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<CustomResponse> UpdateComponent(Component component) {
            CustomResponse response = await CheckExistingComponentValid(component);
            if(response.Value == 0) {
                //component is invalid -> return response message
                return response;
            }
            if(component.Id == 0) { return new CustomResponse(0, "Diese Zutat kann nicht verändert werden"); }
            DbConnection db = new DbConnection();
            try {
                var query = @$"UPDATE component 
                            SET 
                                name = '{component.Name.Trim()}'
                            WHERE 
                                id = {component.Id};";
                await db.ExecuteQuery(query);
                return new CustomResponse((int)component.Id, $"Zutat {component.Name} erfolgreich bearbeitet");
            }
            catch { return CustomResponse.ErrorMessage(); }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method deletes a component by its id. It sets all references to this component to "gelöschte Zutat" in the recipes.
        /// </summary>
        /// <param name="id">id of the component</param>
        /// <returns>Response Message that specifies if the Deletion was successful</returns>
        [HttpDelete("{id}")]
        public async Task<CustomResponse> DeleteComponentById(int id) {
            if (id == 0) { return new CustomResponse(0, "Diese Zutat kann nicht gelöscht werden"); }

            Component existingComponent = await GetComponentById(id);
            if(existingComponent == null) {
                return new CustomResponse(0, "Zutat exisitert nicht");
            }
            DbConnection db = new DbConnection();
            try {
                {
                    var query1 = @$"UPDATE component_in_recipe
                                SET
                                    component = 0
                                WHERE
                                    component = {id};";
                    await db.ExecuteQuery(query1);
                }
                {
                    var query2 = @$"SET FOREIGN_KEY_CHECKS=0;
                                DELETE FROM component 
                                WHERE
                                    name = '{existingComponent.Name}';
                                SET FOREIGN_KEY_CHECKS=1;";
                    await db.ExecuteQuery(query2);
                }
                    
                return new CustomResponse(id, $"Zutat erfolgreich gelöscht");
            }
            catch { return CustomResponse.ErrorMessage(); }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Method removes all references to components in a given recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <returns>Respones Message that specifies if the deleteion was successful</returns>
        public async Task<CustomResponse> RemoveAllComponentsFromRecipe(int recipeId) {
            DbConnection db = new DbConnection();
            try {
                var query = @$"DELETE FROM component_in_recipe
                                WHERE
                                    recipe = {recipeId};";
                
                await db.ExecuteQuery(query);
                return new CustomResponse(1, "");
            }
            catch { return new CustomResponse(0, "Anweisung konnte nicht ausgeführt werden"); }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// Adds reference to components to a given recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <param name="components">List of the components to add to the recipe</param>
        /// <returns>Respones Message that specifies if the update was successful</returns>
        public async Task<CustomResponse> AddComponentsToRecipe(int recipeId, List<Component> components) {
            DbConnection db = new DbConnection();
            try {
                for(int i = 0; i < components.Count; i++) {
                    int componentId = (int)components[i].Id;
                    var unit = await this.unitController.GetUnitByName(components[i].UnitName);

                    var query = @$"INSERT INTO component_in_recipe (recipe, component, amount, unit)
                                    VALUES ({recipeId}, {componentId}, {(int)components[i].Amount}, {(int)unit.Id})";
                    
                    await db.ExecuteQuery(query);
                }
                return CustomResponse.SuccessMessage();
            }
            catch { return CustomResponse.ErrorMessage(); }
            finally { db.CloseConnection(); }
        }

        /// <summary>
        /// method checks if a given component is valid to update in the DB.
        /// </summary>
        /// <param name="component"></param>
        /// <returns>Response Message</returns>
        public async Task<CustomResponse> CheckExistingComponentValid(Component component) {

            Component existingComponent = await GetComponentById((int)component.Id);
            if(component.Id == null|| existingComponent == null) {
                return new CustomResponse(0, $"Zutat {component.Name} mit Id {component.Id} exisitert nicht");
            }
            if(component.Name.Trim() == "") {
                return new CustomResponse(0, $"Zutat mit Id {component.Id} ist unvollständig");
            }

            Component sameNameComponent = await GetComponentByName(component.Name);
            if(sameNameComponent != null && sameNameComponent.Id != component.Id) {
                return new CustomResponse(0, "Der Name exisitert bereits für eine andere Zutat");
            }
            return CustomResponse.SuccessMessage();
        }

        /// <summary>
        /// method checks if the given component is valid to add to DB
        /// </summary>
        /// <returns>Response Message that specifies if the component is valid</returns>
        public async Task<CustomResponse> CheckNewComponentValid(Component component) {

            if(component.Name.Trim() == "") {
                return new CustomResponse(0, $"Zutat ist unvollständig");
            }

            Component sameNameComponent = await GetComponentByName(component.Name);
            if(sameNameComponent != null) {
                return new CustomResponse(0, "Der Name exisitert bereits für eine andere Zutat");
            }

            return CustomResponse.SuccessMessage();
        }
    }
}
