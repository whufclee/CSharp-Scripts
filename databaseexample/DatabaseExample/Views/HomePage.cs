using System;
using System.IO;
using SQLite;
using Xamarin.Forms;
using DatabaseExample.Models;

namespace DatabaseExample.Views
{
    public class HomePage : ContentPage
    {
        public HomePage()
        {
            using (SQLiteConnection db = new SQLiteConnection(App.DB_PATH))
            {
                db.CreateTable<Articles>();
            }

            StackLayout stackLayout = new StackLayout();

            Button button = new Button();
            button.Text = "Add Company";
            button.Clicked += Button_Clicked;
            stackLayout.Children.Add(button);

            button = new Button();
            button.Text = "Get Details";
            button.Clicked += Button_Get_Clicked;
            stackLayout.Children.Add(button);

            button = new Button();
            button.Text = "Edit Details";
            button.Clicked += Button_Edit_Clicked;
            stackLayout.Children.Add(button);

            button = new Button();
            button.Text = "Delete";
            button.Clicked += Button_Delete_Clicked;
            stackLayout.Children.Add(button);

            button = new Button();
            button.Text = "Add Link";
            button.Clicked += Add_Link_Clicked;
            stackLayout.Children.Add(button);

            button = new Button();
            button.Text = "Delete Link";
            button.Clicked += Delete_Link_Clicked;
            stackLayout.Children.Add(button);

            button = new Button();
            button.Text = "Get Article Details";
            button.Clicked += Button_Get_Article_Clicked;
            stackLayout.Children.Add(button);

            button = new Button();
            button.Text = "View Trending";
            button.Clicked += Button_Trending_Clicked;
            stackLayout.Children.Add(button);


            button = new Button();
            button.Text = "Delete Database";
            button.Clicked += Button_DeleteDB_Clicked;
            stackLayout.Children.Add(button);

            button = new Button();
            button.Text = "Read Local File";
            button.Clicked += Button_ReadLocal_Clicked;
            stackLayout.Children.Add(button);

            button = new Button();
            button.Text = "Websites";
            button.Clicked += Button_Websites_Clicked;
            stackLayout.Children.Add(button);

            button = new Button();
            button.Text = "Disabled Websites";
            button.Clicked += Button_DisabledWebsites_Clicked;
            stackLayout.Children.Add(button);

            Content = stackLayout;
        }

        private async void Button_DeleteDB_Clicked(object sender, EventArgs e)
        {
            if (File.Exists(App.DB_PATH))
            {
                File.Delete(App.DB_PATH);
                await DisplayAlert(null, "Database deleted", "OK");
            }
            using (SQLiteConnection db = new SQLiteConnection(App.DB_PATH))
            {
                db.CreateTable<Articles>();// Make sure Article table has been created
                db.CreateTable<Links>(); // Make sure Links database has been created
                db.CreateTable<SocialFeeds>(); // Make sure SocialFeeds database has been created
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCompanyPage());
        }

        private async void Button_Get_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GetAllCompaniesPage());
        }

        private async void Button_Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditCompanyPage());
        }

        private async void Button_Delete_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeleteCompanyPage());
        }
        private async void Add_Link_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddLinkPage());
        }
        private async void Delete_Link_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeleteLinkPage());
        }
        private async void Button_Get_Article_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GetAllArticlesPage());
        }
        private async void Button_Trending_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UnreadPage());
        }
        private async void Button_ReadLocal_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReadLocalPage());
        }
        private async void Button_Websites_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WebsitePage());
        }
        private async void Button_DisabledWebsites_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DisabledWebsitesPage());
        }
    }
}

