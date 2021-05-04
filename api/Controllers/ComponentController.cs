using api.Processors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentController : Controller {
        [HttpGet]
        async public Task<List<Component>> GetAllComponents() {
            return await ComponentProcessor.GetAllComponents();
        }

        [HttpGet("{componentId}")]
        async public Task<Component> GetComponentById(int componentId) {
            return await ComponentProcessor.GetComponentById(componentId);
        }

        [HttpPost]
        async public Task<bool> AddNewComponent(Component component) {
            return await ComponentProcessor.AddComponentIfNotExist(component);
        }
        /*
        async public Task<Component> UpdateComponent(int id, Component updatedComponent) {

        }

        //delete component and remove references in all Recipes
        async public Task<bool> DeleteComponent(Component component) {

        }*/
    }
}
