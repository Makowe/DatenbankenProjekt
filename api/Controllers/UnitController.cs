using api.Model;
using api.Database;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class UnitController : ControllerBase {

        public UnitController() { }

        /// <summary>
        /// Method gets a Unit by its name
        /// </summary>
        /// <param name="unitName">name of the unit</param>
        /// <returns>unit object</returns>
        public async Task<Unit> GetUnitByName(string unitName) {
            try {
                var query = $@"SELECT id, name, shortname
                            FROM unit 
                            WHERE name = '{unitName}'";
                var reader = await DbConnection.ExecuteQuery(query);
                if(reader.HasRows) {
                    await reader.ReadAsync();
                    var id = (int?)reader.GetValue(0);
                    var name = (string)reader.GetValue(1);
                    var shortname = (string)reader.GetValue(2);
                    return new Unit(id, name, shortname);
                }
                else {
                    return new Unit();
                }
            }
            catch { return new Unit(); }
        }

        /// <summary>
        /// Method gets a list of all units stored in the database
        /// </summary>
        /// <returns>List of all units</returns>
        [HttpGet]
        public async Task<List<Unit>> GetAllUnits() {
            List<Unit> components = new List<Unit>();
            try {
                var query = $"SELECT id, name, shortname FROM unit;";
                var reader = await DbConnection.ExecuteQuery(query);

                if(reader.HasRows) {
                    while(await reader.ReadAsync()) {
                        var id = (int)reader.GetValue(0);
                        var name = (string)reader.GetValue(1);
                        var shortname = (string)reader.GetValue(2);
                        components.Add(new Unit(id, name, shortname));
                    }
                }
            }
            catch { }
            return components;
        }
    }
}
