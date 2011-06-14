using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
            InitHourLines();

            const double targetTime = 8.75;
            InitTargetLine(targetTime);
            
            const double minTimeStart = 6.0;
            InitMinTimes(minTimeStart);
        }

        private void InitMinTimes(double minTimeStart)
        {
            rctMinTime.Margin = SetElementPos(GetPos(minTimeStart), rctMinTime);
            rctMinTime.Width = GetPos(0.75);
            rctMinTime.Visibility = Visibility.Visible;
        }

        private void InitHourLines()
        {
            const int numHours = 10;
            for (var i = 1; i <= numHours; i++)
            {
                var xPos = GetPos(i);
                var line = AddLine(xPos);
                var label = AddLabel(i, xPos);
                gridMain.Children.Add(line);
                gridMain.Children.Add(label);
            }
        }

        private Label AddLabel(int i, double xPos)
        {
            const double offsetX = 8;
            const double offsetY = 17;
            return new Label
                   {
                       Margin = new Thickness(xPos - offsetX, GetHeightTop()-offsetY, 0, 0),
                       Content = i.ToString(),
                       FontSize = 8,
                       Height = offsetY+5,
                       VerticalAlignment = VerticalAlignment.Top,
                   };
        }

        private Line AddLine(double xPos)
        {
            return new Line
                   {
                       Stroke = Brushes.LightSteelBlue,
                       X1 = xPos,
                       X2 = xPos,
                       Y1 = GetHeightTop(),
                       Y2 = GetHeightBottom(),
                       StrokeThickness = 1,
                       StrokeDashArray = new DoubleCollection {2, 2},
                       Opacity = 50
                   };
        }

        private void InitTargetLine(double targetTime)
        {
            var targetPos = GetPos(targetTime);
            var line = new Line
            {
                Stroke = Brushes.Green,
                X1 = targetPos,
                X2 = targetPos,
                Y1 = GetHeightTop(),
                Y2 = GetHeightBottom(),
                StrokeThickness = 2
            };
            gridMain.Children.Add(line);
        }
        

        public void UpdateCurrentPos(TimeSpan timeSpent)
        {
            var currentPos = rctMain.Margin.Left + GetPos(timeSpent.TotalMinutes / 60.0);
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

        private double GetHeightBottom()
        {
            return rctMain.Margin.Top + rctMain.Height;
        }
        private double GetHeightTop()
        {
            return rctMain.Margin.Top;
        }
    }
}
