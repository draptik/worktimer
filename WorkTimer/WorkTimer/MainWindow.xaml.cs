using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace WorkTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Fields
        
        private DispatcherTimer _dispatcherTimer;
        private const string TitleString = "Should I Stay Or Should I Go Now";

        private readonly TimeSpan _warningTimeSpanMax = new TimeSpan(0, 30, 0);
        private readonly Color _warnBackgroundColor = Colors.LightPink;
        private readonly Color _okBackgroundColor = Colors.LightGreen;
        private readonly Color _warnForgroundColor = Colors.Red;
        private Brush _defaultBackground;

        #endregion
        
        #region GUI Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ucProgress.Init();
            _defaultBackground = gbTimes.Background;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValidStartTime()) {
                ShowErrorDlg();
                return;
            }

            StartDispatcher();
        }
        

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Update();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            StopDispatcher();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #region Update

        private void Update()
        {
            if (!IsValidStartTime()) { return; }

            try {
                var workTime = new WorkTime(tbTimeStart.Text);
                UpdateTextBoxes(workTime);
                UpdateProgressGui(workTime);
                UpdateTitle(workTime);
                UpdateWarnings(workTime);
            }
            catch (Exception) {
                ShowErrorDlgKillTimer();
            }
        }


        private void UpdateTextBoxes(WorkTime workTime)
        {
            tbTimeTarget.Text = workTime.TargetTime.ToShortTimeString();
            tbTimeTargetRemaining.Text = workTime.RemainingTillTarget.ToDisplayString();

            tbMinTime.Text = workTime.MinTimeStart.ToShortTimeString();
            tbMinTimeRemaining.Text = workTime.RemainingTillMinTime.ToDisplayString();

            tbMaxTime.Text = workTime.MaxTime.ToShortTimeString();
            tbMaxTimeRemaining.Text = workTime.RemainingTillMaxTime.ToDisplayString();

            tbBalance.Text = workTime.Balance.ToDisplayString();
        }

        private void UpdateProgressGui(WorkTime workTime)
        {
            ucProgress.UpdateCurrentPos(workTime.TimeSpent);
        }

        private void UpdateTitle(WorkTime workTime)
        {
            Title = string.Format("{0} ({1})", TitleString, workTime.TimeSpent.ToDisplayString());
        }

        private void UpdateWarnings(WorkTime workTime)
        {
            if (WarnIfMaxTimeReached(workTime)) {
                gbTimes.Background = new SolidColorBrush(_warnBackgroundColor);
                tbMaxTimeRemaining.Background = new SolidColorBrush(_warnBackgroundColor);
            }
            else {
                gbTimes.Background = _defaultBackground;
                tbMaxTimeRemaining.Background = new SolidColorBrush(_okBackgroundColor);
            }

            tbTimeTargetRemaining.Background = IsLessThanTargetTime(workTime)
                                                   ? new SolidColorBrush(_warnBackgroundColor)
                                                   : new SolidColorBrush(_okBackgroundColor);

            tbMinTimeRemaining.Background = IsLessThanMinTime(workTime)
                                                ? new SolidColorBrush(_warnBackgroundColor)
                                                : new SolidColorBrush(_okBackgroundColor);
        }

        #endregion
        
        #region Warning and Validation

        private bool IsValidStartTime()
        {
            return !tbTimeStart.Text.IsNullOrEmpty();
        }

        private bool WarnIfMaxTimeReached(WorkTime workTime)
        {
            return workTime.RemainingTillMaxTime < _warningTimeSpanMax;
        }


        private static bool IsLessThanMinTime(WorkTime workTime)
        {
            return workTime.RemainingTillMinTime.TotalSeconds > 0;
        }

        private static bool IsLessThanTargetTime(WorkTime workTime)
        {
            return workTime.RemainingTillTarget.TotalSeconds > 0;
        }

        private static void ShowErrorDlg()
        {
            MessageBox.Show("Bitte gültige Startzeit eingeben!", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowErrorDlgKillTimer()
        {
            MessageBox.Show("Startzeit darf nicht in der Zukunft liegen!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            StopDispatcher();
        } 

        #endregion
        
        #region Timer
        
        private void StartDispatcher()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _dispatcherTimer.Start();

            ToggleStartStopButtons();
        }

        private void StopDispatcher()
        {
            _dispatcherTimer.Stop();
            ToggleStartStopButtons();
        }

        private void ToggleStartStopButtons()
        {
            btnUpdate.IsEnabled = !_dispatcherTimer.IsEnabled;
            btnStop.IsEnabled = _dispatcherTimer.IsEnabled;
        }

        #endregion
    }
}
