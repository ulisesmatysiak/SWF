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

        #region Campeonatos
        [HttpPost]
        [Route("InsertCampeonato")]
        public ActionResult Insertar_Campeonatos([FromBody] Campeonato campeonato)
        {
            try
            {
                _dbcontext.Campeonatos.Add(campeonato);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Campeonato con id: " + campeonato.Id + " descripcion: " + campeonato.Descripcion + " insert ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditCampeonato")]
        public ActionResult Edit_Campeonatos([FromBody] Campeonato campeonato)
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
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Campeonato con id: " + campeonato.Id + " descripcion: " + old + " editado ok a: " + edit.Descripcion });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }
        #endregion

        #region Fechas
        [HttpPost]
        [Route("InsertFecha")]
        public ActionResult Insert_Fecha()
        {
            try
            {
                DateTime fechaInicio = new DateTime(2024, 3, 1, 19, 0, 0);
                DateTime fechaFin = fechaInicio.AddDays(184); // 6 meses

                while(fechaInicio < fechaFin)
                {
                    _dbcontext.Fechas.Add(new Fecha { Descripcion = fechaInicio.ToString("yyyy-MM-dd HH:mm:ss") });
                    fechaInicio = fechaInicio.AddDays(1);
                }

                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Registros insertados correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }
        #endregion

        #region Jugadores
        #endregion
    }
}
