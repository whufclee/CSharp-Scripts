//using System.ComponentModel;
//using Xamarin.Forms;

//namespace Paperboy.Pages
//{
//    public partial class AddLinks : ContentPage
//    {
//        public AddLinks()
//        {
//            InitializeComponent();
//        }

//        void Save_Clicked(object sender, System.EventArgs e)
//        {
//            Articles book = new Articles()
//            {
//                Name = nameEntry.Text,
//            };

//            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
//            {
//                conn.CreateTable<Articles>();
//                var numRows = conn.Insert(book);

//                if (numRows > 0)
//                    DisplayAlert("Success", "Book successfully inserted", "Great");
//                else
//                    DisplayAlert("Failure", "Book failed to insert", "Dang it!");
//            }
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Paperboy.Helpers;
using Xamarin.Forms;
using Paperboy.News;

namespace Paperboy.Pages
{
    public partial class AddLinks : ContentPage
    {
        public AddLinks()
        {
            InitializeComponent();
        }

        void Save_Clicked(object sender, System.EventArgs e)
        {
            string fullLink = uriEntry.Text;
            if (!fullLink.StartsWith("http", StringComparison.Ordinal))
                fullLink = "http://" + fullLink;

            Scraper.LinksToDb(fullLink);

            //using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            //{
            //    conn.CreateTable<Articles>();
            //    foreach (var link in links)
            //    {

            //    }
            //    var numRows = conn.Insert(book);

            //    if (numRows > 0)
            //        DisplayAlert("Success", "Social Feed successfully added", "OK");
            //    else
            //        DisplayAlert("Failure", "Invalid URL, please check you typed the link in correctly", "OK!");
            //}
        }


        //public async static Task<List<Links>> GrabLinks(string fullLink)
        //{
        //    List<Links> results = new List<Links>();
        //    var content = Scraper.StringFromUrl(fullLink);
        //    if (content != "")
        //    {
        //        // Almost working, not setting object items into list though
        //        XDocument xdoc = new XDocument();
        //        xdoc = XDocument.Parse(content);
        //        results = (from feed in xdoc.Element("root").Elements("link")
        //                   select new Links()
        //                    {
        //                        Url = feed.Element("url").Value,
        //                        Icon = feed.Element("icon").Value,
        //                        Name = feed.Element("name").Value
        //                   }).ToList();
        //    }
        //    return results.ToList();
        //}
    }
}
