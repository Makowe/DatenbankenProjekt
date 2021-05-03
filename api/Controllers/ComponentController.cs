using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers {
    public class ComponentController : Controller {
        async public Task<List<Component>> GetAllComponents() {

        }
        async public Task<Component> GetComponentById(int id) {

        }
        async public Task<Component> AddNewComponent(Component component) {

        }
        async public Task<Component> UpdateComponent(int id, Component updatedComponent) {

        }

        //delete component and remove references in all Recipes
        async public Task<bool> DeleteComponent(Component component) {

        }
    }
}
