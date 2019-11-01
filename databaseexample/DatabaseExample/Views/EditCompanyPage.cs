using System;
using System.IO;
using DatabaseExample.Models;
using SQLite;

using Xamarin.Forms;

namespace DatabaseExample.Views
{
    public class EditCompanyPage : ContentPage
    {
        private ListView _listView;
        private Entry _idEntry;
        private Entry _nameEntry;
        private Entry _addressEntry;
        private Button _button;
        Company _company = new Company();

        public EditCompanyPage()
        {
            this.Title = "Edit Company";

            var db = new SQLiteConnection(App.DB_PATH);

            StackLayout stackLayout = new StackLayout();
            _listView = new ListView();
            _listView.ItemsSource = db.Table<Company>().OrderBy(x => x.Name).ToList();
            _listView.ItemSelected += _listView_ItemSelected;
            stackLayout.Children.Add(_listView);

            _idEntry = new Entry();
            _idEntry.Placeholder = "ID";
            _idEntry.IsVisible = false;
            stackLayout.Children.Add(_idEntry);

            _nameEntry = new Entry();
            _nameEntry.Keyboard = Keyboard.Text;
            _nameEntry.Placeholder = "Company Name";
            stackLayout.Children.Add(_nameEntry);

            _addressEntry = new Entry();
            _addressEntry.Keyboard = Keyboard.Text;
            _addressEntry.Placeholder = "Address";
            stackLayout.Children.Add(_addressEntry);

            _button = new Button();
            _button.Text = "Update";
            _button.Clicked += _button_Clicked;
            stackLayout.Children.Add(_button);

            Content = stackLayout;
        }

        private async void _button_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(App.DB_PATH);
            Company company = new Company()
            {
                Id = Convert.ToInt32(_idEntry.Text),
                Name = _nameEntry.Text,
                Address = _addressEntry.Text
            };
            db.Update(company);
            await Navigation.PopAsync();

        }

        private void _listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _company = (Company)e.SelectedItem;
            _idEntry.Text = _company.Id.ToString();
            _nameEntry.Text = _company.Name;
            _addressEntry.Text = _company.Address;
        }
    }
}

