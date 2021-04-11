using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SowaLabsChallenge.Models
{
    public class OrderBook
    {
        [JsonPropertyName("lastUpdateId")] public int LastUpdateId { get; set; }
        [JsonPropertyName("bids")] public List<List<string>> Bids { get; set; }
        [JsonPropertyName("asks")] public List<List<string>> Asks { get; set; }
    }
}