using api.Model;
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

        [HttpGet("{id}")]
        async public Task<Component> GetComponentById(int id) {
            return await ComponentProcessor.GetComponentById(id);
        }

        [HttpPost]
        async public Task<Response> AddNewComponent(Component component) {
            return await ComponentProcessor.AddComponentIfNotExist(component);
        }

        [HttpPut]
        async public Task<Response> UpdateComponent(Component updatedComponent) {
            return await ComponentProcessor.UpdateComponent(updatedComponent);
        }
        
        [HttpDelete("{id}")]
        async public Task<Response> DeleteComponent(int id) {
            return await ComponentProcessor.DeleteComponentById(id);
        }
    }
}
