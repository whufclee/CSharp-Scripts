using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Paperboy
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;

            base.OnAppearing();
            //using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            //{
            //    conn.CreateTable<Articles>();
            //    var articles = conn.Table<Articles>().ToList();
            //}
        }

        private void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {

        }

        private async void OnCodesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.AddLinks());
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.SettingsPage());
        }

    }
}
