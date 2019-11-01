using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using DatabaseExample.Models;
using SQLite;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace DatabaseExample.Views
{
    public class AddLinkPage : ContentPage
    {
        private Entry _linkEntry;
        private Button _saveButton;

        public AddLinkPage()
        {
            this.Title = "Add News Link";
            StackLayout stackLayout = new StackLayout();

            _linkEntry = new Entry();
            _linkEntry.Keyboard = Keyboard.Text;
            _linkEntry.Placeholder = "bit.ly/2LRwRHB";
            stackLayout.Children.Add(_linkEntry);

            _saveButton = new Button();
            _saveButton.Text = "Save";
            _saveButton.Clicked += _saveButton_Clicked;
            stackLayout.Children.Add(_saveButton);

            Content = stackLayout;
        }

        // BITCHUTE PARSER
        static private string Bitchute(string url)
        {
            string channel = url.Split(new string[] { "channel" }, StringSplitOptions.None)[1];
            return "https://www.bitchute.com/feeds/rss/channel" + channel;
        }

        // YOUTUBE PARSER
        static private string YT(string url)
        {
            WebResponse response = Response(url);
            string _contents = new System.IO.StreamReader(
                response.GetResponseStream()).ReadToEnd().Trim();
            string pattern = "channelId\":\"(.+?)\"";
            Regex myRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match m = myRegex.Match(_contents);
            string s = m.Groups[1].Value;
            return "https://www.youtube.com/feeds/videos.xml?channel_id=" + s;
        }

        // RETURN TRUE FINAL URL
        static private string FinalUrl(WebResponse url)
        {
            string _finalUrl = url.ResponseUri.ToString();
            string _domain = new Uri(_finalUrl).Host.Replace("www.","");
            if (_domain=="youtube.com")
                _finalUrl = YT(_finalUrl);
            if (_domain == "bitchute.com")
                _finalUrl = Bitchute(_finalUrl);
            return _finalUrl;
        }


        static private string FindInText(string text, string regex)
        {
            string _response = "";
            Regex myRegex = new Regex(regex, RegexOptions.IgnoreCase);
            Match m = myRegex.Match(text);
            if (m.Success)
            {
                _response = m.Groups[1].Value;
            }
            return _response;
        }

        static public string CleanString(string text)
        {
            text = WebUtility.HtmlDecode(text);
            return text;
        }
        static public List<Articles> RSSParser(string contents, string icon = "DefaultIcon.png", string website="")
        {
            contents = contents.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", ""); ; // Get contents of url and strip whitespace
            string _regex = "<entry>(.+?)</entry>";
            if (contents.Contains("<item>"))
                _regex = "<item>(.+?)</item>";
            string _summary = "description>(.+?)description>";
            string _icon = "enclosure.+?url=\"(.+?)\"";
            if (contents.Contains("<icon"))
                _icon = "<icon>(.+?)</icon>";
            if (contents.Contains("thumbnail url=\""))
                _icon = "thumbnail.+?url=\"(.+?)\"";

            Regex myRegex = new Regex(_regex, RegexOptions.IgnoreCase);
            Match m = myRegex.Match(contents);
            string article = "";
            if (m.Success)
                article = m.Groups[1].Value;
            try
            {
                var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment, IgnoreWhitespace = true, IgnoreComments = true };
                var reader = XmlReader.Create(new StringReader(contents), settings);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                reader.Close();
                List<Articles> articleResponse = new List<Articles>();
                foreach (SyndicationItem item in feed.Items)
                {
                    string _desc = "";
                    string _finalIcon = "";
                    if (article != "")
                        _finalIcon = FindInText(article, _icon);
                    if (_finalIcon == "")
                        _finalIcon = icon;

                    string _title = item.Title.Text;
                    DateTime _date = item.PublishDate.DateTime;
                    string _uri = item.Links[0].Uri.AbsoluteUri;
                    if (item.Summary != null)
                        _desc = item.Summary.Text;
                    else if (article != "")
                        _desc = FindInText(article, _summary);
                    else
                        _desc = item.Title.Text;
                    articleResponse.Add(new Articles
                    {
                        Title = CleanString(_title),
                        Date = _date,
                        Uri = _uri,
                        Content = CleanString(_desc),
                        Icon = _finalIcon,
                        Website = website
                    });
                    if (m.Success)
                    {
                        m = m.NextMatch();
                        article = m.Groups[1].Value;
                    }
                }
                return articleResponse;
            }
            catch // If the RSS reader fails then fallback to webparser (badly formatted bitchute for example)
            {
                return WebParser(contents, icon, website);
            }
        }


        static public List<Articles> WebParser(string contents, string icon = "DefaultIcon.png", string website = "")
        {
            contents = contents.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", ""); ; // Get contents of url and strip whitespace
            string article = "";
            string _finalIcon = "";
            string _regex = "<item>(.+?)</item>";
            string _link = "<link>(.+?)</link>";
            string _date = "<pubDate>(.+?)</pubDate>";
            string _desc = "description>(.+?)description>";
            string _icon = "enclosure.+?url=\"(.+?)\"";
            string _title = "<title>(.+?)</title>";
            if (contents.Contains("<entry>"))
            {
                _regex = "<entry>(.+?)</entry>";
                _link = "<link.+?href=\"(.+?)\"";
                _date = "<updated>(.+?)</updated>";
                _desc = "<summary>(.+?)</summary>";
                _icon = "<icon>(.+?)</icon>";
            }
            Regex myRegex = new Regex(_regex, RegexOptions.IgnoreCase);
            Match m = myRegex.Match(contents);
            List<Articles> articleResponse = new List<Articles>();
            if (m.Success)
            {
                article = m.Groups[1].Value;
                while (article != "")
                {
                    _finalIcon = FindInText(article, _icon);
                    if (_finalIcon == "")
                        _finalIcon = icon;
                    articleResponse.Add(new Articles
                    {
                        Title = CleanString(FindInText(article, _title)),
                        Date = Convert.ToDateTime(FindInText(article, _date)),
                        Uri = FindInText(article, _link),
                        Content = CleanString(FindInText(article, _desc)),
                        Icon = _finalIcon,
                        Website = website
                    });
                    m = m.NextMatch();
                    article = m.Groups[1].Value;
                }
            }
            return articleResponse;
        }

        static public string PageTitle(string contents)
        {
            string _title = "";
            string _regex = "<title>(.+?)<";
            contents = contents.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
            Regex myRegex = new Regex(_regex, RegexOptions.IgnoreCase);
            Match m = myRegex.Match(contents);
            if (m.Success)
                _title = m.Groups[1].Value;
            return CleanString(_title);
        }


        // RETURN THE WEB RESPONSE AS AN OBJECT
        static private WebResponse Response(string url)
        {
            if (!url.StartsWith("https://"))
                url = "https://" + url.Replace("http://","");
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36";
            request.ContentType = "text/json";
            return request.GetResponse();
        }

        // RETURN A 2 STRINGS - FINAL URL & CONTENTS
        static public UrlDetails GetUrlAndContents(string url)
        {
            string _contents = "";
            string _domain = "";
            string _iconUrl = "";

            WebResponse response = Response(url); // Get object of original url
            string _finalUrl = FinalUrl(response); // Get the (potentially) new url

            if (_finalUrl != url)
                response = Response(_finalUrl); // Get a new response based on the final url
            try
            {
                _contents = new System.IO.StreamReader(
                    response.GetResponseStream()).ReadToEnd().Trim(); // Get final contents of url
                _domain = new Uri(_finalUrl).Host.Replace("www.","");
            }
            catch { }
         
            if (_domain == "youtube.com" || _domain == "bitchute.com")
                _iconUrl = url;
            else
                _iconUrl = _domain;
            return new UrlDetails
            {
                Contents = _contents,
                IconUrl = _iconUrl,
                OriginalUrl = url,
                FinalUrl = _finalUrl,
                Domain = _domain,
                Title = PageTitle(_contents),
                Icon = GrabIcon(_iconUrl)
        };
        }

        // CONVERT A STRING INTO AN ARRAY OF LINES
        static private string[] ReadLines(string content)
        {
            string[] lineArray = content.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);
            return lineArray;
        }

        // GET AN ICON (FAVICON/AVATAR) FROM URI
        private static string GrabIcon(string url)
        {
            if (!url.StartsWith("https://"))
                url = "https://" + url.Replace("http://", "");
            string icon = "DefaultIcon.png";
            if (url == "")
                return icon;
            else
            {
                string _regex = "linkrel=\".+?icon\".+?href=\"(.+?)\"";
                Uri _uri = new Uri(url);
                string _domain = _uri.Host.Replace("www.","");

                switch (_domain)
                {
                    case "youtube.com":
                        _regex = "avatar\":{.+?url\":\"(.+?)\"";
                        break;
                    case "bitchute.com":
                        var _segments = _uri.Segments;
                        string _channel = _segments[_segments.Length-1].Replace("/","");
                        _regex = $"ahref=\"/channel/{_channel}.+?data-src=\"(.+?)\"";
                        break;
                    default:
                        _regex = "linkrel=\".+?icon\".+?href=\"(.+?)\"";
                        break;
                }
                WebResponse response = Response(url); // Get object of original url
                string _contents = new System.IO.StreamReader(
                    response.GetResponseStream()).ReadToEnd().Trim().Replace(" ", "").Replace("\r\n", "").Replace("\n", "").Replace("\r", ""); ; // Get contents of url and strip whitespace
                Regex myRegex = new Regex(_regex, RegexOptions.IgnoreCase);
                Match m = myRegex.Match(_contents);
                if (m.Groups.Count >= 1)
                    icon = m.Groups[1].Value;
                return icon;
            }
        }

        // SAVE CLICKED - ADD CONTENT TO DB
        private async void _saveButton_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection db = new SQLiteConnection(App.DB_PATH))
            {
                db.CreateTable<Links>(); // Make sure database has been created
                db.CreateTable<SocialFeeds>(); // Make sure database has been created

                UrlDetails content = GetUrlAndContents(_linkEntry.Text);
                string[] lines = ReadLines(content.Contents);
                foreach (var line in lines) // Loop through all links in social feed
                {
                    string link = line.Replace("\n", "").Replace("\r", "").Replace("\t", "");
                    Links linkExists = (from p in db.Table<Links>()
                                        where p.OrigUrl == line
                                        select p).FirstOrDefault();
                    if (linkExists == null) // If social feed doesn't exist in db
                    {
                        UrlDetails _validated = GetUrlAndContents(link);
                        string _title = _validated.Title;
                        if (_title != "")
                        {
                            var answer = await DisplayAlert("New Feed Detected", "A new feed has been found, would you like to add: " + _title, "Yes", "No");
                            if (answer)
                                AddLinks(_validated, line, 1);
                            else
                                AddLinks(_validated, line, 0);
                        }
                    }
                }
            }
            await Navigation.PopAsync();
        }


        private void AddLinks(UrlDetails urlDetails, string url, int enabled)
        {
            using (SQLiteConnection db = new SQLiteConnection(App.DB_PATH))
            {
                Links linkEntry = new Links()
                {
                    Url = urlDetails.FinalUrl,
                    OrigUrl = url,
                    Name = urlDetails.Title,
                    Offline = 0,
                    Icon = urlDetails.Icon,
                    Enabled = enabled
                };
                db.Insert(linkEntry);
                if (enabled ==1)
                    AddArticles(urlDetails);
            }
        }

        // Add articles to the db
        public void AddArticles(UrlDetails urlDetails)
        {
            string _contents = urlDetails.Contents;
            string _icon = urlDetails.Icon;
            string _website = urlDetails.Title;
            List<Articles> articleList = new List<Articles>();
            articleList = RSSParser(_contents, _icon, _website);
            foreach (var item in articleList)
            {
                Articles articleExists;
                using (SQLiteConnection db = new SQLiteConnection(App.DB_PATH))
                {
                    articleExists = (from p in db.Table<Articles>()
                                     where p.Uri == item.Uri
                                     select p).FirstOrDefault();
                    if (articleExists == null) // If social feed doesn't exist in db
                    {
                        try
                        {
                            db.Insert(item);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                } // Close DB connection
            } // End looping through the articles
        }
    }
}
