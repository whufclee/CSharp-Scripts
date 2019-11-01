using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseExample.Models;
using Xamarin.Forms;

namespace DatabaseExample.Views
{
    public partial class UnreadPage : ContentPage
    {
        public UnreadPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            LoadNewsAsync();

            base.OnAppearing();
        }

        private async void LoadNewsAsync()
        {
            newsListView.IsRefreshing = true;
            var news = GetTrending();

            this.BindingContext = news;
            newsListView.IsRefreshing = false;
        }

        public static List<Articles> GetTrending()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                return conn.Table<Articles>().OrderBy(x => x.Date).Where(x => x.Viewed == 0).ToList();
            }
        }

        private void newsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}
