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
        private const double BreakTime    = 0.75;
        private const double TargetTime   = 8.75;
        private const double MinTimeStart = 6.0;
        private const double MaxTime      = 10.75;

        public WorkProgressUserControl()
        {
            InitializeComponent();
        }

        public void Init()
        {
            InitHourLines();
            InitTargetLine(TargetTime);
            InitMinTimes(MinTimeStart);
        }

        public void UpdateCurrentPos(TimeSpan timeSpent)
        {
            var currentPos = GetPos(timeSpent.TotalMinutes / 60.0);
            rctCurrent.Width = currentPos;
        }


        private void InitMinTimes(double minTimeStart)
        {
            rctMinTime.Margin     = SetElementPos(GetPos(minTimeStart), rctMinTime);
            rctMinTime.Width      = GetPos(BreakTime);
            rctMinTime.Visibility = Visibility.Visible;
        }

        private void InitHourLines()
        {
            const int numHours = 10;
            for (var i = 1; i <= numHours; i++) {
                var xPos  = GetPos(i);
                AddLine(xPos);
                AddLabel(i, xPos);
            }
        }

        private void AddLabel(int i, double xPos)
        {
            const double offsetX = 8;
            const double offsetY = 17;
            var label = new Label
                        {
                            Margin = new Thickness(xPos - offsetX, GetHeightTop() - offsetY, 0, 0),
                            Content = i.ToString(),
                            FontSize = 8,
                            Height = offsetY + 5,
                            VerticalAlignment = VerticalAlignment.Top,
                        };
            gridMain.Children.Add(label);
        }

        private void AddLine(double xPos)
        {
            var line = new Line
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
            gridMain.Children.Add(line);
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
        

        private double GetHeightBottom()
        {
            return rctMain.Margin.Top + rctMain.Height;
        }
        private double GetHeightTop()
        {
            return rctMain.Margin.Top;
        }

        private double GetPos(double d)
        {
            var maxPos = rctMain.Width;
            var zeroPos = rctMain.Margin.Left;
            return zeroPos + Math.Round(maxPos / MaxTime * d, 2, MidpointRounding.AwayFromZero);
        }

        private static Thickness SetElementPos(double pos, FrameworkElement element)
        {
            return new Thickness(pos, element.Margin.Top, element.Margin.Right, element.Margin.Bottom);
        }
    }
}
