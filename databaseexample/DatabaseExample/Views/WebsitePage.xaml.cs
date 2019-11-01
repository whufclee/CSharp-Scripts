using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseExample.Models;
using Xamarin.Forms;

namespace DatabaseExample.Views
{
    public partial class WebsitePage : ContentPage
    {
        public WebsitePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            LoadWebsitesAsync();

            base.OnAppearing();
        }

        private async void LoadWebsitesAsync()
        {
            websitesListView.IsRefreshing = true;
            var news = GetWebsites();

            this.BindingContext = news;
            websitesListView.IsRefreshing = false;
        }

        public static List<Links> GetWebsites()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                return conn.Table<Links>().OrderBy(x => x.Name).Where(x => x.Enabled == 1).ToList();
            }
        }
    }
}
