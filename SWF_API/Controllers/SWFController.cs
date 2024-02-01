using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWF_API.Models;
using System.Security.Cryptography;

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
                return BadRequest("Campeonato no encontrado");

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

        [HttpGet]
        [Route("ListarCampeonatos")]
        public ActionResult List_Campeonatos()
        {
            try
            {
                var campeonatos = _dbcontext.Campeonatos.ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Campeonatos: ", campeonatos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
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

                while (fechaInicio < fechaFin)
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

        [HttpPost]
        [Route("InsertJugador")]
        public ActionResult Insert_Jugador(Jugador jugador)
        {
            try
            {
                _dbcontext.Jugadores.Add(jugador);
                _dbcontext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new
                {
                    mensaje =
                    "Jugador id: " + jugador.Id + " Nombre: " + jugador.Nombre + " Camiseta: " + jugador.Camiseta
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditJugador")]
        public ActionResult Edit_Jugador(Jugador jugador)
        {
            Jugador edit = _dbcontext.Jugadores.Find(jugador.Id);

            if (edit == null)
                return BadRequest("Jugador no encontrado");

            try
            {
                edit.Nombre = jugador.Nombre;             
                edit.IdCampeonato = jugador.IdCampeonato;
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new
                {
                    mensaje =
                    "Jugador con id: " + jugador.Id + " Nombre: " + edit.Nombre +  " Campeonato: " + jugador.IdCampeonato + " editado ok a: " + edit.IdCampeonato
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("ListarJugadores")]
        public ActionResult List_Jugadores()
        {
            try
            {
                var jugadores = _dbcontext.Jugadores.ToList();
                //return StatusCode(StatusCodes.Status200OK, new { mensaje = "Jugadores: ", jugadores });
                return Ok(jugadores);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("ListarCombo")]
        public ActionResult Listar_Combo()
        {
            try
            {
                var jugadores = _dbcontext.Jugadores
                    .Select(jugador => new
                {
                    Id = jugador.Id,
                    Nombre = jugador.Nombre,
                    Camiseta = jugador.Camiseta,
                    IdCampeonato = jugador.IdCampeonato,
                })
                    .ToList();

                var campeonatos = _dbcontext.Campeonatos.ToList();

                var model = new
                {
                    Jugadores = jugadores,
                    Campeonatos = campeonatos
                };

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        #endregion

        #region Tweets
        [HttpPost]
        [Route("InsertFechaTweet")]
        public ActionResult Insert_FechaTweet()
        {
            try
            {
                var fechas = _dbcontext.Fechas.ToList();

                foreach (var fecha in fechas)
                {
                    var tweet = new Tweet
                    {
                        IdFecha = fecha.Id
                    };
                    _dbcontext.Tweets.Add(tweet);
                }

                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Tweets generados correctamente por fecha." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("InsertJugadorTweet")]
        public ActionResult Edit_JugadorTweet()
        {
            try
            {
                var jugadores = _dbcontext.Jugadores.ToList();
                var tweets = _dbcontext.Tweets.ToList();

                for (int i = 0; i < tweets.Count; i++)
                {
                    tweets[i].IdJugador = jugadores[(i) % jugadores.Count].Id;
                }

                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Tweets generados correctamente por jugador." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public ActionResult Delete()
        {
            try
            {
                var tweets = _dbcontext.Tweets.Where(t => t.Id > 184).ToList();

                _dbcontext.Tweets.RemoveRange(tweets);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Tweets eliminados." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("Listar")]
        public ActionResult Listar()
        {
            try
            {
                var tweets = _dbcontext.Tweets.ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Tweets" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        #endregion 
    }
}