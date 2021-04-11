using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SowaLabsChallenge.Models
{
    public class OrderBookDepthDto
    {
        [JsonPropertyName("top10Bids")] public List<List<decimal>> Top10Bids { get; set; }
        [JsonPropertyName("top10Asks")] public List<List<decimal>> Top10Asks { get; set; }
        [JsonPropertyName("bids")]
        public List<List<decimal>> Bids { get; set; }
        [JsonPropertyName("asks")]
        public List<List<decimal>> Asks { get; set; }
    }
}