using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace api.Processors {
    [ApiController]
    [Route("[controller]")]
    public class ComponentProcessor {

        static async public Task<List<Component>> GetComponentsOfRecipe(int recipeId) {
            List<Component> components = new List<Component>();
            try {
                var query = @$"SELECT component.id, component.name, amount, unit.name, unit.shortname FROM recipe JOIN component_in_recipe JOIN component JOIN unit
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

        static public async Task<Component> GetComponentById(int id) {
            
            try {
                var query = $"SELECT * FROM component WHERE id = {id};";
                MySqlDataReader reader = await DbConnection.ExecuteQuery(query);

                if(reader.HasRows) {
                    await reader.ReadAsync();
                    var name = (string)reader.GetValue(1);
                    return new Component(id, name);
                }
            }
            catch { }
            return null;
        }

        static public async Task<Component> GetComponentByName(string name) {
            try {
                var query = $"SELECT * FROM component WHERE name = \"{name}\";";
                MySqlDataReader reader = await DbConnection.ExecuteQuery(query);

                if(reader.HasRows) {
                    await reader.ReadAsync();
                    var id = (int)reader.GetValue(0);
                    return new Component(id, name);
                }
            }
            catch { }
            return null;
        }
        
        //id of component object gets ignored
        static public async Task<bool> AddComponent(Component newComponent) {
            try {
                var query = $"INSERT INTO component (name) VALUES ('{newComponent.Name}');";
                await DbConnection.ExecuteQuery(query);
                await GetComponentByName(newComponent.Name);
                return true;
            }
            catch { return false; }
        }

        //id of component object gets ignored
        static public async Task<bool> AddComponentIfNotExist(Component newComponent) {
            Component existingComponent = await GetComponentByName(newComponent.Name);
            if(existingComponent != null) { return false; }
            return await AddComponent(newComponent);
        }

        static public async Task<bool> UpdateComponentById(int id, Component updatedComponent) {
            try {
                Component existingComponent = await GetComponentById(id);
                if(existingComponent != null) {
                    var query = @$"UPDATE components SET 
                                name = {updatedComponent.Name}
                                WHERE id = {id};";
                    await DbConnection.ExecuteQuery(query);
                    return true;
                }
            }
            catch { }
            return false;
        }
    }
}
