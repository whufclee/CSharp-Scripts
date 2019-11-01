using System;
using DatabaseExample.Models;
using SQLite;
using Xamarin.Forms;
using System.IO;

namespace DatabaseExample.Views
{
    public class GetAllArticlesPage : ContentPage
    {
        private ListView _listView;

        public GetAllArticlesPage()
        {
            this.Title = "Articles";

            using (SQLiteConnection db = new SQLiteConnection(App.DB_PATH))
            {
                StackLayout stackLayout = new StackLayout();
                _listView = new ListView
                {
                    ItemsSource = db.Table<Articles>().OrderBy(x => x.Date).ToList()
                };
                stackLayout.Children.Add(_listView);
                Content = stackLayout;
            }
        }
    }
}