using System;
using System.IO;
using DatabaseExample.Models;
using SQLite;
using Xamarin.Forms;

namespace DatabaseExample.Views
{
    public class AddCompanyPage : ContentPage
    {
        private Entry _nameEntry;
        private Entry _addressEntry;
        private Button _saveButton;
        //private string osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
        
           public AddCompanyPage()
        {
            this.Title = "Add Company";
            StackLayout stackLayout = new StackLayout();

            _nameEntry = new Entry();
            _nameEntry.Keyboard = Keyboard.Text;
            _nameEntry.Placeholder = "Company Name";
            stackLayout.Children.Add(_nameEntry);

            _addressEntry = new Entry();
            _addressEntry.Keyboard = Keyboard.Text;
            _addressEntry.Placeholder = "Address";
            stackLayout.Children.Add(_addressEntry);

            _saveButton = new Button();
            _saveButton.Text = "Add";
            _saveButton.Clicked += _saveButton_Clicked;
            stackLayout.Children.Add(_saveButton);

            Content = stackLayout;
        }

        private async void _saveButton_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(App.DB_PATH);
            db.CreateTable<Company>();

            var maxPk = db.Table<Company>().OrderByDescending(c => c.Id).FirstOrDefault();

            Company company = new Company()
            {
                Id = (maxPk == null ? 1 : maxPk.Id + 1),
                Name = _nameEntry.Text,
                Address = _addressEntry.Text
            };
            db.Insert(company);
            await DisplayAlert(null, company.Name + " Saved", "OK");
            await Navigation.PopAsync();
        }
    }
}

