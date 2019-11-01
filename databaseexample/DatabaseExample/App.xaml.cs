using System;
using DatabaseExample.Views;
using DatabaseExample.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;

namespace DatabaseExample
{
    public partial class App : Application
    {
        public static string DB_PATH = string.Empty;
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage( new MainPage());
        }

        public App(string DB_Path)
        {
            InitializeComponent();
            DB_PATH = DB_Path;
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            using (SQLiteConnection db = new SQLiteConnection(App.DB_PATH))
            {
                db.CreateTable<Articles>();
                db.CreateTable<Links>();
            }
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
