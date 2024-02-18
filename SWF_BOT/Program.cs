using Newtonsoft.Json;
using SWF_BOT;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Windows.Markup;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

//HttpClient httpClient = new HttpClient();

//var response = await httpClient.GetAsync("http://localhost:5076/api/SWF/Listar");

//if (response.IsSuccessStatusCode)
//{
//    var responseContent = await response.Content.ReadAsStringAsync();
//    var tweets = JsonConvert.DeserializeObject<List<Tweet>>(responseContent);

//    foreach (var tweet in tweets)
//    {
//        Console.WriteLine($"Nombre: {tweet.Nombre}, Camiseta: {tweet.Camiseta}, Imagen: {tweet.Imagen}, Campeonato: {tweet.Campeonato}, Fecha: {tweet.Fecha}");
//    }
//}
//else
//{
//    Console.WriteLine($"Error al obtener los tweets. Código de estado: {response.StatusCode}");
//}

var consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
var consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];

var accessToken = ConfigurationManager.AppSettings["AccessToken"];
var accessTokenSecret = ConfigurationManager.AppSettings["AccessTokenSecret"];

var bearerToken = ConfigurationManager.AppSettings["BearerToken"];

var clientId = ConfigurationManager.AppSettings["ClientId"];
var clientSecret = ConfigurationManager.AppSettings["ClientSecret"];

var credentials = new TwitterCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);

var client = new TwitterClient(credentials);

var authenticatedUser = await client.Users.GetAuthenticatedUserAsync();
Console.WriteLine(authenticatedUser);

TweetRequest tweet = new TweetRequest();
tweet.Text = "El fin de ésta cuenta será postear cada día y por fecha = número de camiseta de jugadores icónicos del fútbol argentino durante las últimas dos décadas (2003-2023) #LasCallesNoOlvidan";

try
{
    var result = await client.Execute.AdvanceRequestAsync(BuildTwitterRequest(client, tweet));
    Console.WriteLine($"{result}");
}
catch (Exception ex)
{
    Console.WriteLine($"{ex.Message}");
}
static Action<ITwitterRequest> BuildTwitterRequest(TwitterClient client, TweetRequest tweet)
{
    return (ITwitterRequest request) =>
    {
        var jsonBody = client.Json.Serialize(tweet);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        request.Query.Url = "https://api.twitter.com/2/tweets";
        request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
        request.Query.HttpContent = content;
    };
}


//try
//{
//    var publishedTweet = await client.Tweets.PublishTweetAsync(tweet);
//    Console.WriteLine($"Tweet publicado con exito: {publishedTweet}");
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"Tweet error: {ex.Message}");
//}