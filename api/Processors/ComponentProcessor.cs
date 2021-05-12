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
    public class ComponentProcessor {

        /// <summary>
        /// Method generates a list of all components included in a recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <returns>List of the components</returns>
        static async public Task<List<Component>> GetComponentsOfRecipe(int recipeId) {
            List<Component> components = new List<Component>();
            try {
                var query = @$"SELECT component.id, component.name, amount, unit.name, unit.shortname 
                            FROM recipe JOIN component_in_recipe JOIN component JOIN unit
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

        /// <summary>
        /// Method generates a list of all components stored in the database
        /// </summary>
        /// <returns>List of the components</returns>
        static public async Task<List<Component>> GetAllComponents() {
            List<Component> components = new List<Component>();
            try {
                var query = $"SELECT * FROM component;";
                MySqlDataReader reader = await DbConnection.ExecuteQuery(query);


                if(reader.HasRows) {
                    while(await reader.ReadAsync()) {
                        var id = (int)reader.GetValue(0);
                        var name = (string)reader.GetValue(1);
                        components.Add(new Component(id, name));
                    }
                }
            }
            catch { }
            return components;
        }

        /// <summary>
        /// Method searches for a single component by its id</summary>
        /// <param name="id">Id to search for</param>
        /// <returns>The component with the id. Returns <c>null</c> if the component does not exist</returns>
        static public async Task<Component> GetComponentById(int id) {
            
            try {
                var query = $"SELECT * FROM component WHERE id = {id};";
                MySqlDataReader reader = await DbConnection.ExecuteQuery(query);

                if(reader.HasRows) {
                    await reader.ReadAsync();
                    var name = (string)reader.GetValue(1);
                    return new Component(id, name);
                }
                else { return null; }
            }
            catch { return null; }
        }

        /// <summary>
        /// Method searches for a component by its name
        /// </summary>
        /// <param name="name">name of the component</param>
        /// <returns>The component with the id. Returns <c>null</c> if the component does not exist</returns>
        static public async Task<Component> GetComponentByName(string name) {
            try {
                var query = $"SELECT * FROM component WHERE name = \"{name}\";";
                MySqlDataReader reader = await DbConnection.ExecuteQuery(query);

                if(reader.HasRows) {
                    await reader.ReadAsync();
                    var id = (int)reader.GetValue(0);
                    return new Component(id, name);
                }
                else { return null; }
            }
            catch { return null; }
        }
        
        /// <summary>
        /// Method Adds a single component to the database
        /// </summary>
        /// <param name="newComponent">component object to add. The id of the object gets ignored because the DB assigns the id automatically</param>
        /// <returns>Response Message with the id of the new stored component. Returns a Response Message with value 0 if the component could not be added</returns>
        static public async Task<Response> AddComponent(Component newComponent) {
            try {
                var query = $"INSERT INTO component (name) VALUES ('{newComponent.Name}');";
                await DbConnection.ExecuteQuery(query);
                var storedComponent = await GetComponentByName(newComponent.Name);
                return new Response(storedComponent.Id, $"Zutat {newComponent.Name} erfolgreich hinzugefügt");
            }
            catch { return new Response(0, "Anweisung konnte nicht ausgeführt werden"); }
        }

        /// <summary>
        /// Method checks if the given component already exists and adds it to the DB if not.
        /// </summary>
        /// <param name="newComponent">component object to add</param>
        /// <returns>Response Message with the id of the new stored component. Returns a Response Message with value 0 if the component could not be added</returns>
        static public async Task<Response> AddComponentIfNotExist(Component newComponent) {
            if(newComponent.Name == "") { return new Response(0, "Zutat ist unvollständig");  }
            
            Component existingComponent = await GetComponentByName(newComponent.Name);
            if(existingComponent != null) { return new Response(0, "Der Name exisitert bereits"); }
            return await AddComponent(newComponent);
        }

        /// <summary>
        /// Method changes an existing recipe
        /// </summary>
        /// <param name="updatedComponent"></param>
        /// <returns></returns>
        static public async Task<Response> UpdateComponent(Component updatedComponent) {
            int id = (int)updatedComponent.Id;

            if(id == 0) { return new Response(0, "Diese Zutat kann nicht verändert werden"); }
            if(updatedComponent.Name == "") { return new Response(0, "Zutat ist unvollständig"); }
            var sameNameComponent = await GetComponentByName(updatedComponent.Name);
            if(sameNameComponent != null && sameNameComponent.Id != id) {
                return new Response(0, "Der Name exisitert bereits");
            }

            try {
                Component existingComponent = await GetComponentById(id);
                if(existingComponent != null) {
                    var query = @$"UPDATE component 
                                SET 
                                    name = '{updatedComponent.Name}'
                                WHERE 
                                    id = {id};";
                    await DbConnection.ExecuteQuery(query);
                    return new Response(id, $"Zutat {updatedComponent.Name} erfolgreich bearbeitet");
                }
                else {
                    return new Response(0, "Zutat exisitert nicht");
                }
            }
            catch { return new Response(0, "Anweisung konnte nicht ausgeführt werden"); }
        }

        /// <summary>
        /// Method deletes a component by its id. It sets all references to this component to "gelöschte Zutat" in the recipes.
        /// </summary>
        /// <param name="id">id of the component</param>
        /// <returns>Response Message that specifies if the Deletion was successful</returns>
        static public async Task<Response> DeleteComponentById(int id) {
            if (id == 0) { return new Response(0, "Diese Zutat kann nicht gelöscht werden"); }
            try {
                Component existingComponent = await GetComponentById(id);
                if(existingComponent != null) {
                    var query1 = @$"UPDATE component_in_recipe
                                    SET
                                        component = 0
                                    WHERE
                                        component = {id};";
                    await DbConnection.ExecuteQuery(query1);
                    var query2 = @$"SET FOREIGN_KEY_CHECKS=0;
                                    DELETE FROM component 
                                    WHERE
                                        name = '{existingComponent.Name}';
                                    SET FOREIGN_KEY_CHECKS=1;";
                    await DbConnection.ExecuteQuery(query2);
                    
                    return new Response(id, $"Zutat erfolgreich gelöscht");
                }
                else {
                    return new Response(0, "Zutat exisitert nicht");
                }
            }
            catch { return new Response(0, "Anweisung konnte nicht ausgeführt werden"); }
        }

        /// <summary>
        /// Method removes all references to components in a given recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <returns>Respones Message that specifies if the deleteion was successful</returns>
        static public async Task<Response> RemoveAllComponentsFromRecipe(int recipeId) {
            try {
                var query = @$"DELETE FROM component_in_recipe
                                WHERE
                                    recipe = {recipeId};";
                await DbConnection.ExecuteQuery(query);
                return new Response(1, "");
            }
            catch { return new Response(0, "Anweisung konnte nicht ausgeführt werden"); }
        }

        /// <summary>
        /// Adds reference to components to a given recipe
        /// </summary>
        /// <param name="recipeId">id of the recipe</param>
        /// <param name="components">List of the components to add to the recipe</param>
        /// <returns>Respones Message that specifies if the update was successful</returns>
        static public async Task<Response> AddComponentsToRecipe(int recipeId, List<Component> components) {
            try {
                for(int i = 0; i < components.Count; i++) {
                    if(components[i].Id == null) { return new Response(0, $"Unbekannte Zutat {components[i].Name}"); }
                    int componentId = (int)components[i].Id;
                    var unit = await UnitProcessor.GetUnitByName(components[i].UnitName);

                    var query = @$"INSERT INTO component_in_recipe (recipe, component, amount, unit)
                                    VALUES ({recipeId}, {componentId}, {(int)components[i].Amount}, {(int)unit.Id})";
                    await DbConnection.ExecuteQuery(query);
                }
                return new Response(1, "");
            }
            catch { return new Response(0, "Anweisung konnte nicht ausgeführt werden"); }
        }
    }
}
