using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using WorkTimer.Common;
using WorkTimer.Domain;

namespace WorkTimer.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        #region Fields

        private TrayIcon.ITrayIcon _trayIcon;
        private WindowState _storedWindowState = WindowState.Normal;

        private DispatcherTimer _dispatcherTimer;
        private const string TitleString = "Should I Stay Or Should I Go Now";

        private readonly TimeSpan _warningTimeSpanMax = new TimeSpan(0, 30, 0);
        private readonly Color _warnBackgroundColor = Colors.LightPink;
        private readonly Color _okBackgroundColor = Colors.LightGreen;
        private readonly Color _warnForgroundColor = Colors.Red;
        private Brush _defaultBackground;

        private const string TimeFormat = "H:mm";
        private readonly CultureInfo _currentCultureInfo = new CultureInfo("de-DE");

        public DateTime? StartDateTime
        {
            get
            {
                DateTime? result = null;
                if (!tbTimeStart.Text.IsNullOrEmpty()) {
                    DateTime startTime;
                    if (DateTime.TryParseExact(tbTimeStart.Text, TimeFormat, _currentCultureInfo, DateTimeStyles.None, out startTime)) {
                        result = startTime;
                    }
                }
                return result;
            }
        }

        #endregion
        

        public MainWindow()
        {
            InitTrayIcon();
            InitializeComponent();
            InitStartDate();
        }

        #region Tray Icon

        private void InitTrayIcon()
        {
            _trayIcon = new TrayIcon.TrayIcon();
            _trayIcon.Init();
            _trayIcon.Icon.Click += trayIcon_ShowClicked;
            _trayIcon.Icon.DoubleClick += trayIcon_DoubleClick;
        }

        void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        void trayIcon_ShowClicked(object sender, EventArgs e)
        {
            Show();
            WindowState = _storedWindowState;
        }

        void OnClose(object sender, CancelEventArgs args)
        {
            _trayIcon.Close();
            _trayIcon = null;
        }

        void OnStateChanged(object sender, System.EventArgs args)
        {
            if (WindowState == WindowState.Minimized) {
                Hide();
                if (_trayIcon!= null && _trayIcon.Icon != null) {
                    _trayIcon.Icon.ShowBalloonTip(2000);
                }
            }
            else {
                _storedWindowState = WindowState;
            }
        }

        void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            if (_trayIcon != null && _trayIcon.Icon != null) {
                _trayIcon.Icon.Visible = !IsVisible;
            }
        }

        #endregion

        #region GUI Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ucProgress.Init(Config.GetInstance());
            _defaultBackground = gbTimes.Background;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValidStartTime()) {
                ShowErrorDlg();
                return;
            }
            
            try {
                InitClock();
            }
            catch (Exception exception) {
                ShowErrorDlg();
            }
            StartDispatcher();
        }

        private void InitStartDate()
        {
            datePickerStartDate.Text = DateTime.Today.ToShortDateString();
        }


        private void InitClock()
        {
            if (IsValidStartTime()) {
                EnableVisibilityChecboxes(true);
                try {
                    ucClock.Init(new WorkTime(tbTimeStart.Text), Config.GetInstance());
                    ucClock.ToggleMinTimeDisplay(cbMinTime.IsChecked.GetValueOrDefault());
                    ucClock.ToggleMaxTimeDisplay(cbMaxTime.IsChecked.GetValueOrDefault());
                    ucClock.ToggleTargetTimeDisplay(cbTargetTime.IsChecked.GetValueOrDefault());
                    ucClock.ToggleTimeSpentDisplay(cbTimeSpent.IsChecked.GetValueOrDefault());
                }
                catch (Exception exception) {
                    ShowErrorDlgKillTimer();
                    throw;
                }
            }
        }

        void dispatcherTimer_Tick(object sender, System.EventArgs e)
        {
            Update();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            StopDispatcher();
            EnableVisibilityChecboxes(false);
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
                UpdateClock(workTime);
                UpdateTrayIcon(workTime);
            }
            catch (Exception exception) {
                ShowErrorDlg(exception.Message);
            }
        }

        private void UpdateClock(WorkTime workTime)
        {
            ucClock.Update(workTime, cbTimeSpent.IsChecked.GetValueOrDefault());
            ucClock.ToggleMinTimeDisplay(cbMinTime.IsChecked.GetValueOrDefault());
            ucClock.ToggleMaxTimeDisplay(cbMaxTime.IsChecked.GetValueOrDefault());
            ucClock.ToggleTargetTimeDisplay(cbTargetTime.IsChecked.GetValueOrDefault());
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
            ucProgress.ToggleMinTimeDisplay(cbMinTime.IsChecked.GetValueOrDefault());
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

        private void UpdateTrayIcon(WorkTime workTime)
        {
            _trayIcon.Icon.Text = workTime.TimeSpent.ToDisplayString();
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

        private void ShowErrorDlg()
        {
            ShowErrorDlg("Bitte gültige Startzeit eingeben!");
        }

        private void ShowErrorDlgKillTimer()
        {
            ShowErrorDlg("Startzeit darf nicht in der Zukunft liegen!");
        } 

        private void ShowErrorDlg(string errorMsg)
        {
            MessageBox.Show(errorMsg, "", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (_dispatcherTimer != null) _dispatcherTimer.Stop();
            ToggleStartStopButtons();
        }

        private void ToggleStartStopButtons()
        {
            if (_dispatcherTimer == null) return;
            if (btnStart != null) { btnStart.IsEnabled = !_dispatcherTimer.IsEnabled; }
            if (btnStop != null) { btnStop.IsEnabled = _dispatcherTimer.IsEnabled; }
        }

        #endregion

        #region Checkbox States
        
        private void cbMinTime_CheckChanged(object sender, RoutedEventArgs e)
        {
            ucClock.ToggleMinTimeDisplay(cbMinTime.IsChecked.GetValueOrDefault());
            ucProgress.ToggleMinTimeDisplay(cbMinTime.IsChecked.GetValueOrDefault());
        }

        private void cbMaxTime_CheckChanged(object sender, RoutedEventArgs e)
        {
            ucClock.ToggleMaxTimeDisplay(cbMaxTime.IsChecked.GetValueOrDefault());
            ucProgress.ToggleMaxTimeDisplay(cbMaxTime.IsChecked.GetValueOrDefault());
        }

        private void cbTimeSpent_CheckChanged(object sender, RoutedEventArgs e)
        {
            ucClock.ToggleTimeSpentDisplay(cbTimeSpent.IsChecked.GetValueOrDefault());
            ucProgress.ToggleTimeSpentDisplay(cbTimeSpent.IsChecked.GetValueOrDefault());
        }

        private void cbTargetTime_CheckChanged(object sender, RoutedEventArgs e)
        {
            ucClock.ToggleTargetTimeDisplay(cbTargetTime.IsChecked.GetValueOrDefault());
            ucProgress.ToggleTargetTimeDisplay(cbTargetTime.IsChecked.GetValueOrDefault());
        }

        private void EnableVisibilityChecboxes(bool enableCheckboxes)
        {
            cbMinTime.IsEnabled = enableCheckboxes;
            cbMaxTime.IsEnabled = enableCheckboxes;
            cbTimeSpent.IsEnabled = enableCheckboxes;
            cbTargetTime.IsEnabled = enableCheckboxes;
        } 

        #endregion

    }
}
