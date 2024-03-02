using log4net;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;

namespace SWF_BOT
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string eventLogName = "SWF_BOT_EventLog";
            if (!EventLog.SourceExists(eventLogName))
                EventLog.CreateEventSource(eventLogName, "Application");

            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = eventLogName;
                eventLog.WriteEntry("Se inicia la aplicación SWF_BOT", EventLogEntryType.Information);

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
                var rightNow = DateTime.Now.ToString("yyyy-MM-dd");
                bool tweetFound = false;

                try
                {
                    var response = await httpClient.GetAsync("http://localhost:9095/api/SWF/Listar");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var tweets = JsonConvert.DeserializeObject<List<TweetData>>(responseContent);

                        foreach (var tw in tweets)
                        {
                            var parsed = tw.Fecha.Substring(0, 10);
                            if (parsed == rightNow)
                            {
                                tweetFound = true;
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
                                    eventLog.WriteEntry(result.Content);
                                    Console.WriteLine(result.Content);
                                    Console.ReadLine();

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
                                    eventLog.WriteEntry($"Error: {ex.Message}");
                                    Console.WriteLine($"Error: {ex.Message}");
                                    Console.ReadLine();
                                }
                            }
                        }
                        if (!tweetFound)
                        {
                            eventLog.WriteEntry($"No hay tweets programados para hoy {rightNow}");
                        }
                    }
                    else
                    {
                        eventLog.WriteEntry($"Error al obtener los tweets. Código de estado: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    eventLog.WriteEntry($"Error get client: {ex.Message}");
                    Console.WriteLine($"Error get client: {ex.Message}");
                    Console.ReadLine();
                }
            }
        }
    }
}
