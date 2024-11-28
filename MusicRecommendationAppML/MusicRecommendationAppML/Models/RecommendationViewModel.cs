using Newtonsoft.Json;
using System.Reflection;

namespace MusicRecommendationAppML.Models
{
    public class RecommendationViewModel
    {
        [JsonProperty("recommended_songs")]
        public List<string> RecommendedSongs { get; set; }
    }
}
