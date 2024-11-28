using Microsoft.AspNetCore.Mvc;
using MusicRecommendationAppML.Models;
using Newtonsoft.Json;
using System.Reflection;

namespace MusicRecommendationAppML.Controllers
{
    public class MusicController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        public IActionResult _Layout()
        {
            return View();
        }



        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Search(SearchViewModel model)
        {
            if (string.IsNullOrEmpty(model.SearchQuery))
            {
                return View("Error", new { Message = "Search query is empty." });
            }

            string url = $"http://127.0.0.1:5000/recommend?song={model.SearchQuery}";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseData = await response.Content.ReadAsStringAsync();
           
            if(!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Recommendation", "Music");
            }
            return RedirectToAction("Recommendation","Music",new { responseData });
        }


        public IActionResult Recommendation(string responseData)
        {

            if(responseData==null)
            {
                ViewBag.hata = "Böyle bir şarkı bulunamamakta.";
                return View();
            }



            // JSON verisini 'MusicRecommendation' modeline dönüştür
            var musicRecommendation = JsonConvert.DeserializeObject<RecommendationViewModel>(responseData);


            if (musicRecommendation == null || musicRecommendation.RecommendedSongs == null)
            {
                return View("Error", new { Message = "No recommended songs found or deserialization failed." });
            }


            // 'RecommendedSongs' listesinde müzikleri tek tek al
            List<string> songs = musicRecommendation.RecommendedSongs;

            // Şarkıları string olarak birleştirip View'a gönderebiliriz
            string songList = string.Join(", ", songs);

            // Modeli oluşturup View'a gönder
            var model = new Recommendation2ViewModel
            {
                ResponseData = songList, // Şarkı listesini View'a gönderiyoruz
               Songs=songs
            };

            return View(model);
        }


        public async Task<IActionResult> Home()
        {
            string url = $"http://127.0.0.1:5000/songlist";
            HttpResponseMessage response = await client.GetAsync(url);
            string responseData = await response.Content.ReadAsStringAsync();

            var songListResponse = JsonConvert.DeserializeObject<SongViewModel>(responseData);

            // 'SongList' listesinde şarkıları tek tek al
            List<Song2> songs = songListResponse.SongList;

            // Şarkıları birleştirip View'a gönderebiliriz
            string songList = string.Join(", ", songs.Select(s => s.Song));

            // Modeli oluşturup View'a gönder
            var model = new Song2ViewModel
            {
                ResponseData = songList, // Şarkı isimlerini View'a gönderiyoruz
                Songs = songs  // Şarkı bilgilerini View'a gönderiyoruz
            };

            return View(model);

        }
    }
}
