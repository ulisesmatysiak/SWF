using Newtonsoft.Json;
using SWF_BOT;
using System.Text.Json;

HttpClient httpClient = new HttpClient();

var response = await httpClient.GetAsync("http://localhost:5076/api/SWF/Listar");

if (response.IsSuccessStatusCode)
{
    var responseContent = await response.Content.ReadAsStringAsync();
    var tweets = JsonConvert.DeserializeObject<List<Tweet>>(responseContent);

    foreach (var tweet in tweets)
    {
        Console.WriteLine($"Nombre: {tweet.Nombre}, Camiseta: {tweet.Camiseta}, Imagen: {tweet.Imagen}, Campeonato: {tweet.Campeonato}, Fecha: {tweet.Fecha}");
    }
}
else
{
    Console.WriteLine($"Error al obtener los tweets. Código de estado: {response.StatusCode}");
}