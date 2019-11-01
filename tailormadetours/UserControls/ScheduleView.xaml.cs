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

namespace TailorMadeTours.UserControls {
  /// <summary>
  /// Interaction logic for ScheduleView.xaml
  /// </summary>
  public partial class ScheduleView : UserControl {
    public ScheduleView() {
      InitializeComponent();
      this.Loaded += ScheduleView_Loaded;
    }
    private const int HourCount = 9;
    private const int DayCount = 7;

    private void ScheduleView_Loaded(object sender, RoutedEventArgs e) {
      AddHoursColumn();
      AddBusyBlock();
    }

    private void AddHoursColumn() {
      for (int i = 0; i < HourCount; i++)
      {
        var timeBlock = new TextBlock()
        {
          Text = String.Format("{0:D2}:00 ", i + HourCount - 1)
        };

        Grid.SetColumn(timeBlock, 0);
        Grid.SetRow(timeBlock, i + 1);
        ScheduleGrid.Children.Add(timeBlock);
      }
    }

    private void AddBusyBlock() {
      var ran = new Random();


      for (int colCounter = 0; colCounter < DayCount; colCounter++)
      {
        for (int rowCounter = 0; rowCounter < HourCount; rowCounter++)
        {
          var crowdPercent = ran.NextDouble();
          var box = new Rectangle()
          {
            Margin = new Thickness(1, 2, 1, 2),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Height = 60,
            Fill = new SolidColorBrush(Color.FromScRgb(1.0f, (float)crowdPercent, .3f, (float)(1 - crowdPercent)))
          };

          Grid.SetColumn(box, colCounter + 1);
          Grid.SetRow(box, rowCounter + 1);
          ScheduleGrid.Children.Add(box);
        }
      }
    }
  }
}
