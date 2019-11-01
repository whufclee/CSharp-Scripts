using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseExample.Models;
using Xamarin.Forms;

namespace DatabaseExample.Views
{
    public partial class DisabledWebsitesPage : ContentPage
    {
        public DisabledWebsitesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            LoadDisabledWebsitesAsync();

            base.OnAppearing();
        }

        private async void LoadDisabledWebsitesAsync()
        {
            disabledWebsitesListView.IsRefreshing = true;
            var news = GetDisabledWebsites();

            this.BindingContext = news;
            disabledWebsitesListView.IsRefreshing = false;
        }

        public static List<Links> GetDisabledWebsites()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                return conn.Table<Links>().OrderBy(x => x.Name).Where(x => x.Enabled == 0).ToList();
            }
        }
    }
}
