using System;
using DatabaseExample.Models;
using SQLite;
using Xamarin.Forms;
using System.IO;

namespace DatabaseExample.Views
{
    public class GetAllCompaniesPage : ContentPage
    {
        private ListView _listView;

        public GetAllCompaniesPage()
        {
            this.Title = "DB Contents";

            using(SQLiteConnection db = new SQLiteConnection(App.DB_PATH))
            {
                StackLayout stackLayout = new StackLayout();

                _listView = new ListView();
                _listView.ItemsSource = db.Table<Company>().OrderBy(x => x.Name).ToList();
                stackLayout.Children.Add(_listView);

                Content = stackLayout;
            }
        }
    }
}

