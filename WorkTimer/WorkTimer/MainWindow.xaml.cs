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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ucProgress.Init();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValidStartTime()) {
                ShowErrorDlg();
                return;
            }

            StartDispatcher();
            ToggleStartStopButtons();
        }
        

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Update();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            _dispatcherTimer.Stop();
            ToggleStartStopButtons();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
        
        private void StartDispatcher()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();
        }

        private void Update()
        {
            if (!IsValidStartTime()) {
                return;
            }

            var workTime = new WorkTime(tbTimeStart.Text);
            tbTimeTarget.Text = workTime.TargetTime.ToShortTimeString();
            tbTimeTargetRemaining.Text = workTime.RemainingTillTargetString;

            ucProgress.UpdateCurrentPos(workTime.TimeSpent);
        }

        private bool IsValidStartTime()
        {
            return !tbTimeStart.Text.IsNullOrEmpty();
        }

        private static void ShowErrorDlg()
        {
            MessageBox.Show("Bitte gültige Startzeit eingeben!", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ToggleStartStopButtons()
        {
            btnUpdate.IsEnabled = !_dispatcherTimer.IsEnabled;
            btnStop.IsEnabled = _dispatcherTimer.IsEnabled;
        }

    }
}
