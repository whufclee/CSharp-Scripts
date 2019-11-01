
using Xamarin.Forms;

namespace Paperboy.Pages
{
    public partial class TrendingPage : ContentPage
    {
        public TrendingPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            LoadNewsAsync();

            base.OnAppearing();
        }

        private async void LoadNewsAsync()
        {
            newsListView.IsRefreshing = true;
            var news = await Helpers.NewsHelper.GetTrendingAsync();

            this.BindingContext = news;
            newsListView.IsRefreshing = false;
        }
    }
}
