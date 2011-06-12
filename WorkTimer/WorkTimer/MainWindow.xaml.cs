using System;
using System.Windows;
using System.Windows.Threading;

namespace WorkTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer _dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region GUI Events


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Update();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Stop();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
        
        private void Update()
        {
            if (tbTimeStart.Text.IsNullOrEmpty()) {
                return;
            }

            var workTime = new WorkTime(tbTimeStart.Text);
            tbTimeTarget.Text = workTime.TargetTime.ToShortTimeString();
            tbTimeTargetRemaining.Text = workTime.RemainingTillTargetString;

            UpdateCurrentPos(workTime.TimeSpent);
        }




        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var zeroPosX = rctMain.Margin.Left;
            
            // Target time
            const double targetTime = 8.0;
            var targetPos = GetPos(targetTime);

            lineTagerTime.X1 = zeroPosX + targetPos;
            lineTagerTime.X2 = zeroPosX + targetPos;

            // Min times
            const double minTimeStart = 6.0;
            rctMin.Margin = new Thickness(zeroPosX + GetPos(minTimeStart), rctMin.Margin.Top, rctMin.Margin.Right, rctMin.Margin.Bottom);
            rctMin.Width = GetPos(0.75);
        }

        private void UpdateCurrentPos(TimeSpan timeSpent)
        {
            var currentPos = GetPos(timeSpent.TotalMinutes/60.0);
            rctCurrent.Width = currentPos;
        }

        private double GetPos(double d)
        {
            var maxPos = rctMain.Width;
            const double maxTime = 10.75;
            return Math.Round(maxPos/maxTime*d, 2, MidpointRounding.AwayFromZero);
        }

    }
}
