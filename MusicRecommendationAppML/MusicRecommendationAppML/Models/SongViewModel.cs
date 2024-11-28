using Newtonsoft.Json;

namespace MusicRecommendationAppML.Models
{
    public class SongViewModel
    {
        [JsonProperty("songs")]
        public List<Song2> SongList { get; set; }  // Şarkılar listesi
    }

 

   
}
