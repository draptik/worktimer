using System;
using System.Windows;
using System.Windows.Controls;

namespace WorkTimer
{
    /// <summary>
    /// Interaction logic for WorkProgressUserControl.xaml
    /// </summary>
    public partial class WorkProgressUserControl : UserControl
    {
        public WorkProgressUserControl()
        {
            InitializeComponent();
        }

        public void Init()
        {
            var zeroPosX = rctMain.Margin.Left;

            // Target time
            const double targetTime = 8.0;
            var targetPos = GetPos(targetTime);

            rctTargetTime.Margin = SetElementPos(zeroPosX + targetPos, rctTargetTime);
            rctTargetTime.Visibility = Visibility.Visible;

            // Min times
            const double minTimeStart = 6.0;
            rctMinTime.Margin = SetElementPos(zeroPosX + GetPos(minTimeStart), rctMinTime);
            rctMinTime.Width = GetPos(0.75);
            rctMinTime.Visibility = Visibility.Visible;
        }


        public void UpdateCurrentPos(TimeSpan timeSpent)
        {
            var currentPos = GetPos(timeSpent.TotalMinutes / 60.0);
            rctCurrent.Width = currentPos;
        }
        
        private double GetPos(double d)
        {
            var maxPos = rctMain.Width;
            const double maxTime = 10.75;
            return Math.Round(maxPos / maxTime * d, 2, MidpointRounding.AwayFromZero);
        }

        private static Thickness SetElementPos(double pos, FrameworkElement element)
        {
            return new Thickness(pos, element.Margin.Top, element.Margin.Right, element.Margin.Bottom);
        }
    }
}
