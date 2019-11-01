using Newtonsoft.Json;
using Paperboy.News;
using Paperboy.News.Trending;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Paperboy.Helpers
{
    public static class NewsHelper
    {
        public static async Task<List<NewsInformation>> GetByCategoryAsync(NewsCategoryType category)
        {
            List<NewsInformation> results = new List<NewsInformation>();

            string searchUrl = $"https://api.cognitive.microsoft.com/bing/v5.0/news/?Category={category}";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Common.CoreConstants.NewsSearchApiKey);

            var uri = new Uri(searchUrl);
            var result = await client.GetStringAsync(uri);
            var newsResult = JsonConvert.DeserializeObject<NewsResult>(result);

            results = (from item in newsResult.value
                       select new NewsInformation()
                       {
                           Title = item.name,
                           Description = item.description,
                           CreatedDate = item.datePublished,
                           ImageUrl = item.image?.thumbnail?.contentUrl,

                       }).ToList();

            return results.Where(w => !string.IsNullOrEmpty(w.ImageUrl)).Take(10).ToList();
        }

        public static async Task<List<NewsInformation>> GetAsync(string searchQuery)
        {
            List<NewsInformation> results = new List<NewsInformation>();

            string searchUrl = $"https://api.cognitive.microsoft.com/bing/v5.0/news/search?q={searchQuery}&count=10&offset=0&mkt=en-us&safeSearch=Moderate";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Common.CoreConstants.NewsSearchApiKey);

            var uri = new Uri(searchUrl);
            var result = await client.GetStringAsync(uri);
            var newsResult = JsonConvert.DeserializeObject<NewsResult>(result);

            results = (from item in newsResult.value
                       select new NewsInformation()
                       {
                           Title = item.name,
                           Description = item.description,
                           CreatedDate = item.datePublished,
                           ImageUrl = item.image?.thumbnail?.contentUrl,

                       }).ToList();

            return results.Where(w => !string.IsNullOrEmpty(w.ImageUrl)).Take(10).ToList();
        }

        //public async static Task<List<NewsInformation>> GetTrendingAsync()
        //{
        //    List<NewsInformation> results = new List<NewsInformation>();

        //    var stream = Android.App.Application.Context.Assets.Open("bing.json");
        //    StreamReader sr = new StreamReader(stream);
        //    string result = sr.ReadToEnd();
        //    sr.Close();

        //    var newsResult = JsonConvert.DeserializeObject<TrendingNewsResult>(result);

        //    results = (from item in newsResult.value
        //               select new NewsInformation
        //               {
        //                   Title = item.name,
        //                   Description = item.description,
        //                   CreatedDate = DateTime.Now,
        //                   ImageUrl = item.contentUrl
        //               }).ToList();

        //    return results.Where(w => string.IsNullOrEmpty(w.ImageUrl)).Take(10).ToList();

        //}

        public async static Task<List<NewsInformation>> GetTrendingAsync()
        {
            List<NewsInformation> results = new List<NewsInformation>();

            string result = Scraper.StringFromUrl("http://feeds.bbci.co.uk/news/video_and_audio/uk/rss.xml");
            List<List<string>> newsResult = Scraper.RSSResults(result);
            //    foreach (var feed in feedList)
            //    {
            //        Console.WriteLine($"{feed[0]} - {feed[1]} - {feed[2]}");
            //    }
            results = (from item in newsResult
                       select new NewsInformation
                       {
                           Title = item[0],
                           Description = item[1],
                           CreatedDate = DateTime.Now,
                           ImageUrl = item[2]
                       }).ToList();

            //return results.Where(w => string.IsNullOrEmpty(w.ImageUrl)).Take(10).ToList();
            return results.Take(10).ToList();

        }
    }
}
