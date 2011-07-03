using System;
using System.Windows;
using WorkTimer.Domain;
using WorkTimer.Gui.EventArgs;

namespace WorkTimer.Gui.Controls
{
    public partial class TimeCheckboxes
    {
        private Config _config;
        public event EventHandler<TimeCheckBoxesEventArgs> CheckChanged;

        public void InvokeCheckChanged(TimeCheckBoxesEventArgs e)
        {
            var handler = CheckChanged;
            if (handler != null) handler(this, e);
        }

        public TimeCheckboxes()
        {
            InitializeComponent();
        }

        public void Init(Config config)
        {
            _config = config;
        }

        private void cbMinTime_CheckChanged(object sender, RoutedEventArgs e)
        {
            InvokeCheckChanged(new TimeCheckBoxesEventArgs { MinTimeChecked = cbMinTime.IsChecked.GetValueOrDefault() });
        }

        private void cbMaxTime_CheckChanged(object sender, RoutedEventArgs e)
        {
            InvokeCheckChanged(new TimeCheckBoxesEventArgs { MaxTimeChecked = cbMaxTime.IsChecked.GetValueOrDefault() });
        }

        private void cbTimeSpent_CheckChanged(object sender, RoutedEventArgs e)
        {
            InvokeCheckChanged(new TimeCheckBoxesEventArgs { TimeSpentChecked = cbTimeSpent.IsChecked.GetValueOrDefault() });
        }

        private void cbTargetTime_CheckChanged(object sender, RoutedEventArgs e)
        {
            InvokeCheckChanged(new TimeCheckBoxesEventArgs { TargetTimeChecked = cbTargetTime.IsChecked.GetValueOrDefault() });
        }

        public void EnableVisibilityChecboxes(bool enableCheckboxes)
        {
            cbMinTime.IsEnabled = enableCheckboxes;
            cbMaxTime.IsEnabled = enableCheckboxes;
            cbTimeSpent.IsEnabled = enableCheckboxes;
            cbTargetTime.IsEnabled = enableCheckboxes;
        }

    }
}
