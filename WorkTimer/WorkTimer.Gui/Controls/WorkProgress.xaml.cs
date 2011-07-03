using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WorkTimer.Domain;
using Rect = WorkTimer.Gui.Shapes.Rect;

namespace WorkTimer.Gui.Controls
{
    public partial class WorkProgress
    {
        private Config _config;
        private Rect _rectTimeSpent;
        private Rect _rectMinTime;
        private Line _lineTargetTime;

        public WorkProgress()
        {
            InitializeComponent();
        }

        public void Init(Config config)
        {
            _config = config;
            InitHourLines();
            InitTargetLine(_config.TargetTimeNum);
            InitMinTimes(_config.MinTimeStartNum);
        }

        public void UpdateCurrentPos(TimeSpan timeSpent)
        {
            var maxTimeSpan = _config.MaxTimeSpan;
            if (timeSpent > maxTimeSpan) {
                timeSpent = maxTimeSpan;
            }

            _rectTimeSpent = new Rect(rctCurrent)
                             {
                                 Color = _config.TimeSpentColor,
                                 Width = GetPos(timeSpent.TotalMinutes/60.0)
                             };
        }


        private void InitMinTimes(double minTimeStart)
        {
            _rectMinTime = new Rect(rctMinTime)
                           {
                               Margin = SetElementPos(GetPos(minTimeStart), rctMinTime),
                               Width = GetPos(_config.BreakTimeNum),
                               Visibility = true,
                               Color = _config.MinTimeColor
                           };
        }

        private void InitTargetLine(double targetTime)
        {
            var targetPos = GetPos(targetTime);
            _lineTargetTime = new Line
            {
                Stroke = new SolidColorBrush {Color =_config.TargetTimeColor},
                X1 = targetPos,
                X2 = targetPos,
                Y1 = GetHeightTop(),
                Y2 = GetHeightBottom(),
                StrokeThickness = 2
            };
            gridMain.Children.Add(_lineTargetTime);
        }
        


        public void ToggleMinTimeDisplay(bool isChecked)
        {
            if (_rectMinTime != null) _rectMinTime.Visibility = isChecked;
        }

        public void ToggleMaxTimeDisplay(bool isChecked)
        {
            //if (_rectMaxTime != null) _rectMaxTime.Visibility = isChecked;
        }

        public void ToggleTargetTimeDisplay(bool isChecked)
        {
            if (_lineTargetTime != null)
                _lineTargetTime.Visibility = isChecked ? Visibility.Visible : Visibility.Hidden;
        }

        public void ToggleTimeSpentDisplay(bool isChecked)
        {
            if (_rectTimeSpent != null) _rectTimeSpent.Visibility = isChecked;
        }



        #region Hour Lines

        private void InitHourLines()
        {
            const int numHours = 10;
            for (var i = 1; i <= numHours; i++)
            {
                var xPos = GetPos(i);
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
                StrokeDashArray = new DoubleCollection { 2, 2 },
                Opacity = 50
            };
            gridMain.Children.Add(line);
        }

        #endregion

        #region Calc Methods

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
            return zeroPos + Math.Round(maxPos / _config.MaxTimeNum * d, 2, MidpointRounding.AwayFromZero);
        }

        private static Thickness SetElementPos(double pos, FrameworkElement element)
        {
            return new Thickness(pos, element.Margin.Top, element.Margin.Right, element.Margin.Bottom);
        } 

        #endregion
    }
}
