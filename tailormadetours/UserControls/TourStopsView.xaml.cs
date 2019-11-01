using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TailorMadeTours.Models;

namespace TailorMadeTours.UserControls
{
    /// <summary>
    /// Interaction logic for TourStopsView.xaml
    /// </summary>
    public partial class TourStopsView : UserControl
    {
        public TourStopsView()
        {
            InitializeComponent();
            ToursListBox.ItemsSource = TourSource.GetAllTourStops();
        }

        private void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            var q = from stops in TourSource.GetAllTourStops()
                    where stops.Selected == true
                    select stops.EstimatedMinutes;

            MessageTextBlock.Text = String.Format("{0} minutes", q.Sum());
        }
    }
}
