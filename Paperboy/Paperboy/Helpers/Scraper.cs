using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Net;
using System.IO;
using System.Collections;
using Paperboy.News;
using Xamarin.Forms;

namespace Paperboy.Helpers
{
    public class Scraper
    {
        static public string StringFromUrl(string url)
        {
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36";
            request.ContentType = "text/json";
            try
            {
                WebResponse response = request.GetResponse();
                return new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd().Trim();
            }
            catch (WebException e)
            {
                Console.WriteLine($"Failed to parse URL:\n{url}\n{e.Message}\n");
            }
            catch (Exception e)
            {
                Console.WriteLine($"The following Exception was raised : {e.Message}\n");
            }
            return "";
        }


        static public List<List<string>> RSSResults(string contents)
        {
            List<List<string>> masterList = new List<List<string>>();
            if (contents != "")
            {
                var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment, IgnoreWhitespace = true, IgnoreComments = true };
                var reader = XmlReader.Create(new StringReader(contents), settings);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                reader.Close();
                string title = feed.Title.Text;
                foreach (SyndicationItem item in feed.Items)
                {
                    List<string> feedList = new List<string>();
                    feedList.Add(item.Title.Text);
                    feedList.Add(item.PublishDate.ToString());
                    feedList.Add(item.Id);
                    masterList.Add(feedList);
                }
            }
            return masterList;
        }

        static private string[] ReadLines(string content)
        {
            string[] lineArray = content.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);
            return lineArray;
        }

        private static string Validate(string link, string linkType)
        {
            string title = "";
            string content = StringFromUrl(link);
            try
            {
                if (linkType == "rss")
                {
                    var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment, IgnoreWhitespace = true, IgnoreComments = true };
                    var reader = XmlReader.Create(new StringReader(content), settings);
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();
                    title = feed.Title.Text;
                }
            }
            catch
            {
                // Failed to parse page, will dismiss this one
            }
            return title;
        }

        static private string CleanLine(string content)
        {
            return content.Replace("\n", "").Replace("\r", "").Replace("\t", "");
        }

        public static void LinksToDb(string url)
        {
            string content = StringFromUrl(url);
            string[] lines = ReadLines(content);
            List<Links> validLinks = new List<Links>();

            foreach (var line in lines)
            {
                if (line.StartsWith("rss:", StringComparison.CurrentCultureIgnoreCase))
                {
                    string link = CleanLine(line.Substring(4));
                    string title = Validate(link, "rss");
                    using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                    {
                        conn.CreateTable<Links>();
                        if (title != "")
                        {
                            Links dbEntries = new Links
                            {
                                Url = link,
                                Name = title,
                                Type = "rss",
                                Enabled = 1
                            };
                            conn.Insert(dbEntries);
                        }
                    }
                }                
            }
        }

        //static void Main(string[] args)
        //{
        //    new Dictionary<string, FeedDetails>();
        //    //string url = "https://www.tr.news/feed/";
        //    //string url = "https://www.youtube.com/feeds/videos.xml?channel_id=UCTj-2nCE8B_3AvEGAKVyn1g";
        //    string url = "http://feeds.bbci.co.uk/news/video_and_audio/uk/rss.xml";
        //    string response = StringFromUrl(url);
        //    List<List<string>> feedList = RSSResults(response);
        //    foreach (var feed in feedList)
        //    {
        //        Console.WriteLine($"{feed[0]} - {feed[1]} - {feed[2]}");
        //    }
        //    Console.ReadLine();
        //}
    }
}
