﻿using System;
using System.Windows;
using System.Windows.Media;
using WorkTimer.Common;
using WorkTimer.Domain;

namespace WorkTimer.Gui.Controls
{
    public partial class TimeAsText
    {
        private Config _config;
        private Brush _defaultBackground;

        public TimeAsText()
        {
            InitializeComponent();
        }

        public event EventHandler<System.EventArgs> StartTimeChanged;

        public void InvokeStartTimeChanged(System.EventArgs e)
        {
            var handler = StartTimeChanged;
            if (handler != null) handler(this, e);
        }

        public void Init(Config config)
        {
            _config = config;
            _defaultBackground = gbTimes.Background;
            datePickerStartDate.Text = DateTime.Today.ToShortDateString();

            var now = DateTime.Now;
            var dt = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0); 
            dateTimePicker1.Value = dt;
        }

        public DateTime? StartTime
        {
            get
            {
                if (dateTimePicker1.Value.HasValue) {
                    var dt = (DateTime) dateTimePicker1.Value;
                    dt.AddSeconds(-((DateTime) dateTimePicker1.Value).Second);
                    return dt;
                }
                return dateTimePicker1.Value;
            }
        }

        public void UpdateTextBoxes(WorkTime workTime)
        {
            tbTimeTarget.Text          = workTime.TargetTime.ToShortTimeString();
            tbTimeTargetRemaining.Text = workTime.RemainingTillTarget.ToDisplayString();
            tbMinTime.Text             = workTime.MinTimeStart.ToShortTimeString();
            tbMinTimeRemaining.Text    = workTime.RemainingTillMinTime.ToDisplayString();
            tbMaxTime.Text             = workTime.MaxTime.ToShortTimeString();
            tbMaxTimeRemaining.Text    = workTime.RemainingTillMaxTime.ToDisplayString();
            tbBalance.Text             = workTime.Balance.ToDisplayString();
        }

        public void UpdateWarnings(WorkTime workTime)
        {
            if (workTime.WarningTimeReached()) {
                gbTimes.Background = new SolidColorBrush(_config.WarnBackgroundColor);
                tbMaxTimeRemaining.Background = new SolidColorBrush(_config.WarnBackgroundColor);
            }
            else {
                gbTimes.Background = _defaultBackground;
                tbMaxTimeRemaining.Background = new SolidColorBrush(_config.OkBackgroundColor);
            }

            tbTimeTargetRemaining.Background = workTime.IsLessThanTargetTime()
                                                   ? new SolidColorBrush(_config.WarnBackgroundColor)
                                                   : new SolidColorBrush(_config.OkBackgroundColor);

            tbMinTimeRemaining.Background = workTime.IsLessThanMinTime()
                                                ? new SolidColorBrush(_config.WarnBackgroundColor)
                                                : new SolidColorBrush(_config.OkBackgroundColor);
        }

        private void BlaChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            InvokeStartTimeChanged(new System.EventArgs());
        }
    }
}
