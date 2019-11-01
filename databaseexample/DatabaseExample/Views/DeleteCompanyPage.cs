using System;
using System.IO;
using DatabaseExample.Models;
using SQLite;
using Xamarin.Forms;

namespace DatabaseExample.Views
{
    public class DeleteCompanyPage : ContentPage
    {
        private ListView _listView;
        private Button _button;
        Company _company = new Company();

        public DeleteCompanyPage()
        {
            this.Title = "Delete Company";

            var db = new SQLiteConnection(App.DB_PATH);

            StackLayout stackLayout = new StackLayout();
            _listView = new ListView();
            var companyList = db.Table<Company>().OrderBy(x => x.Name).ToList();
            _listView.ItemsSource = companyList;
            _listView.ItemSelected += _listView_ItemSelected;
            stackLayout.Children.Add(_listView);

            _button = new Button();
            _button.Text = "Delete";
            _button.Clicked += _button_Clicked;
            stackLayout.Children.Add(_button);

            Content = stackLayout;
        }

        private async void _button_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(App.DB_PATH);

            db.Table<Company>().Delete(x => x.Id == _company.Id);
            await Navigation.PopAsync();

        }

        private void _listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _company = (Company)e.SelectedItem;
        }
    }
}

