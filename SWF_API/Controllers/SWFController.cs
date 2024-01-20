using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWF_API.Models;

namespace SWF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SWFController : ControllerBase
    {
        public readonly SWFDBContext _dbcontext;

        public SWFController(SWFDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult Listar()
        {
            
        }
    }
}
