using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWF_BOT
{
    public class TweetRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;

        [JsonProperty("media_ids")]
        public List<string> Medias { get; set; } = new List<string>();
    }
}
