using Newtonsoft.Json;
using SWF_BOT;
using System.Configuration;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;

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

HttpClient httpClient = new HttpClient();
var response = await httpClient.GetAsync("http://localhost:5076/api/SWF/Listar");

if (response.IsSuccessStatusCode)
{
    var responseContent = await response.Content.ReadAsStringAsync();
    var tweets = JsonConvert.DeserializeObject<List<TweetData>>(responseContent);

    foreach (var tw in tweets)
    {
        var parsed = tw.Fecha.Substring(0, 10);
        var rightNow = DateTime.Now.ToString("yyyy-MM-dd");
        if (parsed == rightNow)
        {
            var imageBytes = File.ReadAllBytes(tw.Imagen);
            var uploaded = await client.Upload.UploadTweetImageAsync(imageBytes);
            var mediaId = uploaded.Id.ToString();

            var tweetParams = new TweetRequest()
            {
                Text = $"#LasCallesNoOlvidan{tw.Camiseta} {tw.Nombre} x {tw.Campeonato}",
                Medias = new List<string> { mediaId }
            };

            try
            {
                var result = await client.Execute.AdvanceRequestAsync(BuildTwitterRequest(client, tweetParams));

                static Action<ITwitterRequest> BuildTwitterRequest(TwitterClient client, TweetRequest tweetParams)
                {
                    return (ITwitterRequest request) =>
                    {
                        var jsonBody = client.Json.Serialize(new { text = tweetParams.Text, media = new { media_ids = tweetParams.Medias } });
                        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                        request.Query.Url = "https://api.twitter.com/2/tweets";
                        request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                        request.Query.HttpContent = content;
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"No hay tweets programados para hoy {rightNow}");
        }
    }
}
else
{
    Console.WriteLine($"Error al obtener los tweets. Código de estado: {response.StatusCode}");
}