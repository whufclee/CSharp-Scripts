using System;
using System.IO;
using DatabaseExample.Models;
using SQLite;
using Xamarin.Forms;

namespace DatabaseExample.Views
{
    public class DeleteLinkPage : ContentPage
    {
        private ListView _listView;
        private Button _button;

        Links _link = new Links();

        public DeleteLinkPage()
        {
            this.Title = "Delete Link";

            using (SQLiteConnection db = new SQLiteConnection(App.DB_PATH))
            {
                db.CreateTable<Links>();

                StackLayout stackLayout = new StackLayout();
                _listView = new ListView();
                var linkList = db.Table<Links>().OrderBy(x => x.Name).ToList();
                _listView.ItemsSource = linkList;
                _listView.ItemSelected += _listView_ItemSelected;
                stackLayout.Children.Add(_listView);

                _button = new Button();
                _button.Text = "Delete";
                _button.Clicked += _button_Clicked;
                stackLayout.Children.Add(_button);

                Content = stackLayout;
            }
        }

        private async void _button_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(App.DB_PATH);
            db.Table<Links>().Delete(x => x.Id == _link.Id);
            await Navigation.PopAsync();
        }

        private void _listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _link = (Links)e.SelectedItem;
        }
    }
}

