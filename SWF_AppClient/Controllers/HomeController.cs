﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWF_AppClient.Models;
using System.Diagnostics;
using System.Text;

namespace SWF_AppClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _client;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ResponseContent = TempData["ResponseContent"] as string;

            var httpClient = _client.CreateClient("SWF_API");

            var response = await httpClient.GetAsync("Listar");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var tweets = JsonConvert.DeserializeObject<List<TweetViewModel>>(responseContent);
                
                return View(tweets);
            }
            else
            {
                return View();
            }
        }
        public async Task<IActionResult> Put_EditJugador([FromForm]JugadorViewModel jugador,[FromForm] int campeonatoId)
        {
            var httpClient = _client.CreateClient("SWF_API");

            var datos = new
            {
                id = jugador.Id,
                IdCampeonato = campeonatoId
            };

            var jsonContent = JsonConvert.SerializeObject(datos);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync("EditJugador", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["ResponseContent"] = responseContent;
                return RedirectToAction("Index");
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }

        public async Task<IActionResult> Post_InsertJugador(JugadorViewModel jugador)
        {
            var httpClient = _client.CreateClient("SWF_API");

            var datos = new
            {
                Nombre = jugador.Nombre,
                Camiseta = jugador.Camiseta
            };

            var jsonContent = JsonConvert.SerializeObject(datos);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("InsertJugador", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TempData["ResponseContent"] = responseContent;
                return RedirectToAction("Index");
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }


        public IActionResult Privacy()
        {
            ViewBag.ResponseContent = TempData["ResponseContent"] as string;
            return View();
        }    
    }
}
