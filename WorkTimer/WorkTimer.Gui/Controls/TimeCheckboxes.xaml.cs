using System;
using System.Windows;
using WorkTimer.Domain;
using WorkTimer.Gui.EventArgs;

namespace WorkTimer.Gui.Controls
{
    /// <summary>
    /// Interaction logic for TimeCheckboxes.xaml
    /// </summary>
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
            //ucClock.ToggleMinTimeDisplay(ucTimeCheckboxes.cbMinTime.IsChecked.GetValueOrDefault());
            //ucProgress.ToggleMinTimeDisplay(ucTimeCheckboxes.cbMinTime.IsChecked.GetValueOrDefault());
        }

        private void cbMaxTime_CheckChanged(object sender, RoutedEventArgs e)
        {
            InvokeCheckChanged(new TimeCheckBoxesEventArgs { MaxTimeChecked = cbMaxTime.IsChecked.GetValueOrDefault() });
            //ucClock.ToggleMaxTimeDisplay(ucTimeCheckboxes.cbMaxTime.IsChecked.GetValueOrDefault());
            //ucProgress.ToggleMaxTimeDisplay(ucTimeCheckboxes.cbMaxTime.IsChecked.GetValueOrDefault());
        }

        private void cbTimeSpent_CheckChanged(object sender, RoutedEventArgs e)
        {
            InvokeCheckChanged(new TimeCheckBoxesEventArgs { TimeSpentChecked = cbTimeSpent.IsChecked.GetValueOrDefault() });
            //ucClock.ToggleTimeSpentDisplay(ucTimeCheckboxes.cbTimeSpent.IsChecked.GetValueOrDefault());
            //ucProgress.ToggleTimeSpentDisplay(ucTimeCheckboxes.cbTimeSpent.IsChecked.GetValueOrDefault());
        }

        private void cbTargetTime_CheckChanged(object sender, RoutedEventArgs e)
        {
            InvokeCheckChanged(new TimeCheckBoxesEventArgs { TargetTimeChecked = cbTargetTime.IsChecked.GetValueOrDefault() });
            //ucClock.ToggleTargetTimeDisplay(ucTimeCheckboxes.cbTargetTime.IsChecked.GetValueOrDefault());
            //ucProgress.ToggleTargetTimeDisplay(ucTimeCheckboxes.cbTargetTime.IsChecked.GetValueOrDefault());
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
