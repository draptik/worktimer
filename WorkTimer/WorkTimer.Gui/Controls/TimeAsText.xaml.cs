using System;
using System.Windows.Media;
using WorkTimer.Common;
using WorkTimer.Domain;

namespace WorkTimer.Gui.Controls
{
    /// <summary>
    /// Interaction logic for TimeAsText.xaml
    /// </summary>
    public partial class TimeAsText
    {
        private Config _config;
        private Brush _defaultBackground;

        public TimeAsText()
        {
            InitializeComponent();
        }

        public void Init(Config config)
        {
            _config = config;
            _defaultBackground = gbTimes.Background;
            datePickerStartDate.Text = DateTime.Today.ToShortDateString();
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
            if (workTime.WarnIfMaxTimeReached()) {
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
    }
}
