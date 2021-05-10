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
    public class UnitController : Controller {
        [HttpGet]
        async public Task<List<Unit>> GetAllUnits() {
            return await UnitProcessor.GetAllUnits();
        }
    }
}
