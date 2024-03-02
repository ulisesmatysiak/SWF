using log4net;
using Newtonsoft.Json;
using System.Configuration;
using System.Reflection;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;

namespace SWF_BOT
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static async Task Main(string[] args)
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["Config.log4Net"]);
            System.IO.FileInfo arch = new System.IO.FileInfo(ruta);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(arch);

            log.Info("Se inicia la aplicación SWF_BOT");

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
                                log.Info(result.Content);
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
                                log.Error($"Error: {ex.Message}");
                            }
                        }
                    }
                    if (!tweetFound)
                    {
                        log.Info($"No hay tweets programados para hoy {rightNow}");
                    }
                }
                else
                {
                    log.Info($"Error al obtener los tweets. Código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                log.Info($"Error get client: {ex.Message}");
                Console.ReadLine();
            }
        }
    }
}
