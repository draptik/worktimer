﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
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
        private Config _config;
        private const string TitleString = "Should I Stay Or Should I Go Now";

        #endregion
        

        public MainWindow()
        {
            InitTrayIcon();
            InitializeComponent();
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
            _config = Config.GetInstance();
            ucTimeAsText.Init(_config);
            ucProgress.Init(_config);
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
        
        private void InitClock()
        {
            if (IsValidStartTime()) {
                EnableVisibilityChecboxes(true);
                try {
                    ucClock.Init(new WorkTime(ucTimeAsText.tbTimeStart.Text), Config.GetInstance());
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
                var workTime = new WorkTime(ucTimeAsText.tbTimeStart.Text);
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
            ucTimeAsText.UpdateTextBoxes(workTime);
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
            ucTimeAsText.UpdateWarnings(workTime);
        }

        private void UpdateTrayIcon(WorkTime workTime)
        {
            _trayIcon.Icon.Text = workTime.TimeSpent.ToDisplayString();
        }
        
        #endregion
        
        #region Warning and Validation

        private bool IsValidStartTime()
        {
            return !ucTimeAsText.tbTimeStart.Text.IsNullOrEmpty();
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
