using Newtonsoft.Json;

namespace AssuredBid.DTOs
{
    public class ModelResult<T>
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("entity")]
        public List<T> Data { get; set; }

        public Metadata MetaData { get; set; }
    }
}
