using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
        private const double RadiusMinTime = 125d;
        private const double RadiusTimeSpent = 135d;
        private const double RadiusTargetTime = 135d;
        private const double RadiusMaxTime = 135d;
        private readonly Point _zeroPos = new Point(150, 150);
        readonly System.Timers.Timer _timer = new System.Timers.Timer(1000);

        public AnalogClockUserControl()
        {
            InitializeComponent();
            _timer.Elapsed += timer_Elapsed;
            _timer.Enabled = true;
        }

        public void Update(WorkTime workTime)
        {
            timeSpentStartOnCircle.Point = TransformDate(workTime.StartTime, RadiusTimeSpent);
            timeSpentArc.IsLargeArc = workTime.TimeSpent > new TimeSpan(6, 0, 0);
            timeSpentArc.Point = TransformDate(DateTime.Now, RadiusTimeSpent);

            lbClockTop.Content = "time spent: " + workTime.TimeSpent.ToDisplayString();
            lbClockBottom.Content = "remaining: " + workTime.RemainingTillTarget.ToDisplayString();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                              (Action) (() =>
                                        {
                                            secondHand.Angle = DateTime.Now.Second*6;
                                            minuteHand.Angle = DateTime.Now.Minute*6;
                                            hourHand.Angle = GetAngle(DateTime.Now);
                                        }));
        }
        
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // TODO
            //throw new NotImplementedException();
        }

        public void Init(WorkTime workTime)
        {
            if (workTime == null) { return; }

            InitStartTime(workTime);
            InitTargetTime(workTime, RadiusTargetTime);
            InitMinTime(workTime, RadiusMinTime);
            InitMaxTime(workTime, RadiusMaxTime);
        }

        private void InitStartTime(WorkTime workTime)
        {
            StartTimeRotation.Angle = GetAngle(workTime.StartTime);
            rectangleStartTime.Visibility = Visibility.Hidden;
        }

        private void InitTargetTime(WorkTime workTime, double radius)
        {
            TargetTimeRotation.Angle = GetAngle(workTime.TargetTime);
            rectangleTargetTime.Visibility = Visibility.Hidden;

            InitArc(targetTimeStartOnCircle, targetTimeArc, workTime.StartTime, workTime.TargetTime, radius, true);
        }

        private void InitMaxTime(WorkTime workTime, double radius)
        {
            InitArc(maxTimeStartOnCircle, maxTimeArc, workTime.TargetTime, workTime.MaxTime, radius, false);
        }

        private void InitMinTime(WorkTime workTime, double radius)
        {
            var isLargeArc = workTime.MinTimeEnd.Subtract(workTime.MinTimeStart) > new TimeSpan(6, 0, 0);
            InitArc(minTimeStartOnCircle, minTimeArcSegment, workTime.MinTimeStart, workTime.MinTimeEnd, radius, isLargeArc);
        }

        private void InitArc(LineSegment lineSegment, ArcSegment arcSegment, DateTime startTime, DateTime endTime, double radius, bool isLargeArc)
        {
            lineSegment.Point = TransformDate(startTime, radius);
            arcSegment.Size = new Size(radius, radius);
            arcSegment.IsLargeArc = isLargeArc;
            arcSegment.Point = TransformDate(endTime, radius);
        }

        #region Calc Methods
		
        private Point TransformDate(DateTime dateTime, double radius)
        {
            return TransformPoint(GetPointRelative(dateTime, radius));
        }

        private Point TransformPoint(Point point)
        {
            return new Point(_zeroPos.X + point.X, _zeroPos.Y - point.Y);
        }

        private static Point GetPointRelative(DateTime dateTime, double radius)
        {
            return new Point
                   {
                       X = radius*Math.Sin(GetRadians(dateTime)), 
                       Y = radius*Math.Cos(GetRadians(dateTime))
                   };
        }

        private static double GetRadians(DateTime dateTime)
        {
            return GetAngle(dateTime)*(Math.PI/180);
        }

        private static double GetAngle(DateTime dateTime)
        {
            return dateTime.Hour * 30 + dateTime.Minute * 0.5;
        }

        #endregion    
    }
}
