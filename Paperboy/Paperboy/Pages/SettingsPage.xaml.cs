using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Paperboy.Pages
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            InitializeSettings();

            base.OnAppearing();
        }

        private void InitializeSettings()
        {
            displayNameEntry.Text = "Scott";
            bioEditor.Text = "Scott has been developing Microsoft Enterprise solutions for organizations around the world for the last 28 years, and is the Senior Architect & Developer behind Liquid Daffodil.";
            articleCountSlider.Value = 10;
            categoryPicker.SelectedIndex = 1;
        }

    }
}
