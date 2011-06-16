using System;
using System.Windows;
using System.Windows.Input;
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
        private const double Radius = 100d;
        readonly System.Timers.Timer _timer = new System.Timers.Timer(1000);

        public AnalogClockUserControl()
        {
            InitializeComponent();
            _timer.Elapsed += timer_Elapsed;
            _timer.Enabled = true;
        }

        public void Update(WorkTime workTime)
        {
            // TODO
            
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
            throw new NotImplementedException();
        }

        public void Init(WorkTime workTime)
        {
            if (workTime == null) { return; }
            StartTimeRotation.Angle = GetAngle(workTime.StartTime);
            TargetTimeRotation.Angle = GetAngle(workTime.TargetTime);

            InitMinTime(workTime);
        }

        private void InitMinTime(WorkTime workTime)
        {
            minTimeArcSegment.IsLargeArc = workTime.MinTimeEnd.Subtract(workTime.MinTimeStart) > new TimeSpan(6, 0, 0);
            InitMinTimeStart(workTime);
            InitMinTimeEnd(workTime);
        }

        private void InitMinTimeStart(WorkTime workTime)
        {
            var x = GetX(workTime.MinTimeStart);
            var y = GetY(workTime.MinTimeStart);
            minTimeStartOnCircle.Point = TransformPoint(x, y);
        }

        private void InitMinTimeEnd(WorkTime workTime)
        {
            var x = GetX(workTime.MinTimeEnd);
            var y = GetY(workTime.MinTimeEnd);
            minTimeArcSegment.Point = TransformPoint(x, y);
        }

        private static Point TransformPoint(double x, double y)
        {
            return new Point(FixX(x), FixY(y));
        }

        private static double FixX(double x)
        {
            return Radius + x;
        }

        private static double FixY(double y)
        {
            return Radius - y;
        }

        private static double GetY(DateTime time)
        {
            return Radius*Math.Cos(GetRadians(time));
        }

        private static double GetX(DateTime time)
        {
            return Radius*Math.Sin(GetRadians(time));
        }

        private static double GetRadians(DateTime dateTime)
        {
            return GetAngle(dateTime)*(Math.PI/180);
        }

        private static double GetAngle(DateTime dateTime)
        {
            return dateTime.Hour * 30 + dateTime.Minute * 0.5;
        }
    }
}
