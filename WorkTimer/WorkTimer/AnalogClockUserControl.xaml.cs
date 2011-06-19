using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WorkTimer
{
    /// <summary>
    /// Interaction logic for AnalogClockUserControl.xaml
    /// 
    /// http://www.codeproject.com/KB/WPF/WpfClock.aspx
    /// </summary>
    public partial class AnalogClockUserControl
    {
        private const double RadiusMinTime = 110d;
        private const double RadiusTimeSpent = 135d;
        private const double RadiusTargetTime = 110d;
        private const double RadiusMaxTime = 110d;
        private readonly Point _zeroPos = new Point(150, 150);
        readonly System.Timers.Timer _timer = new System.Timers.Timer(1000);
        private Arc _minTimeArc;
        private Arc _targetTimeArc;
        private Arc _maxTimeArc;
        private Arc _timeSpentArc;
        private DateTime _startTime;
        private Config _config;

        public AnalogClockUserControl()
        {
            InitializeComponent();
            _timer.Elapsed += timer_Elapsed;
            _timer.Enabled = true;
        }

        public void Update(WorkTime workTime, bool isChecked)
        {
            _timeSpentArc = new Arc(timeSpentPath, timeSpentStartOnCircle, timeSpentArc, _zeroPos, _config.TimeSpentBrush);
            _timeSpentArc.Update(workTime.StartTime, DateTime.Now, RadiusTimeSpent, workTime.TimeSpent > new TimeSpan(6, 0, 0));
            _timeSpentArc.Visibility = isChecked;

            lbClockTop.Content = "time spent: " + workTime.TimeSpent.ToDisplayString();
            lbClockBottom.Content = "remaining: " + workTime.RemainingTillTarget.ToDisplayString();

            if (workTime.StartTime != _startTime) {
                Init(workTime, _config);
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                              (Action) (() =>
                                        {
                                            secondHand.Angle = DateTime.Now.Second*6;
                                            minuteHand.Angle = DateTime.Now.Minute*6;
                                            hourHand.Angle = DateTime.Now.Hour * 30 + DateTime.Now.Minute * 0.5;
                                        }));
        }
        
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // TODO
            //throw new NotImplementedException();
        }

        public void Init(WorkTime workTime, Config config)
        {
            if (workTime == null || config == null) { throw new ArgumentException("bla"); }

            _startTime = workTime.StartTime;
            _config = config;

            InitStartTime(workTime);
            InitTargetTime(workTime, RadiusTargetTime);
            InitMinTime(workTime, RadiusMinTime);
            InitMaxTime(workTime, RadiusMaxTime);
        }

        private void InitStartTime(WorkTime workTime)
        {
            var rect = new Rect(StartTimeRotation, rectangleStartTime);
            rect.Update(workTime.StartTime);
            rect.Visibility = false;
        }

        private void InitTargetTime(WorkTime workTime, double radius)
        {
            var rect = new Rect(TargetTimeRotation, rectangleTargetTime);
            rect.Update(workTime.TargetTime);
            rect.Visibility = false;
            
            _targetTimeArc = InitArc(targetTimePath, targetTimeStartOnCircle, targetTimeArc, workTime.StartTime, workTime.TargetTime, radius, true, _config.TargetTimeBrush);
        }

        private void InitMaxTime(WorkTime workTime, double radius)
        {
            _maxTimeArc = InitArc(maxTimePath, maxTimeStartOnCircle, maxTimeArc, workTime.TargetTime, workTime.MaxTime, radius, false, _config.MaxTimeBrush);
        }

        private void InitMinTime(WorkTime workTime, double radius)
        {
            var isLargeArc = workTime.MinTimeEnd.Subtract(workTime.MinTimeStart) > new TimeSpan(6, 0, 0);
            _minTimeArc = InitArc(minTimePath, minTimeStartOnCircle, minTimeArcSegment, workTime.MinTimeStart, workTime.MinTimeEnd, radius, isLargeArc, _config.MinTimeBrush);
        }

        private Arc InitArc(Path path, LineSegment lineSegment, ArcSegment arcSegment, 
            DateTime startTime, DateTime endTime, double radius, bool isLargeArc, Brush brush)
        {
            var arc = new Arc(path, lineSegment, arcSegment, _zeroPos, brush);
            arc.Update(startTime, endTime, radius, isLargeArc);
            return arc;
        }

        public void ToggleMinTimeDisplay(bool isChecked)
        {
            if (_minTimeArc != null) _minTimeArc.Visibility = isChecked;
        }

        public void ToggleMaxTimeDisplay(bool isChecked)
        {
            if (_maxTimeArc != null) _maxTimeArc.Visibility = isChecked;
        }

        public void ToggleTimeSpentDisplay(bool isChecked)
        {
            if (_timeSpentArc != null) _timeSpentArc.Visibility = isChecked;
        }

        public void ToggleTargetTimeDisplay(bool isChecked)
        {
            if (_targetTimeArc != null) _targetTimeArc.Visibility = isChecked;
        }
    }
}
