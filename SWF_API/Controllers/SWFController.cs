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

        [HttpPost]
        [Route("InsertCampeonato")]
        public ActionResult Insertar_Campeonatos([FromBody]Campeonato campeonato)
        {
            try
            {
                _dbcontext.Campeonatos.Add(campeonato);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Campeonato con id: " + campeonato.Id + " descripcion: " + campeonato.Descripcion + " insert ok"});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditCampeonato")]
        public ActionResult Edit_Campeonatos([FromBody]Campeonato campeonato)
        {
            Campeonato edit = _dbcontext.Campeonatos.Find(campeonato.Id);
            var old = edit.Descripcion;

            if (edit == null)
            {
                return BadRequest("Campeonato no encontrado");
            }

            try
            {
                edit.Descripcion = campeonato.Descripcion;
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Campeonato con id: " + campeonato.Id + " descripcion: " + old + " editado ok a: " + edit.Descripcion});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }
    }
}
