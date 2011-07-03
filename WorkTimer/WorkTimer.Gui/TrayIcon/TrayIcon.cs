using System.Windows.Forms;


namespace WorkTimer.Gui.TrayIcon
{
    public class TrayIcon : ITrayIcon
    {
        #region Implementation of ITrayIcon

        public NotifyIcon Icon { get; private set; }

        public void Init()
        {
            // http://possemeeg.wordpress.com/2007/09/06/minimize-to-tray-icon-in-wpf/
            Icon = new NotifyIcon
                    {
                        Icon = new System.Drawing.Icon(@"..\..\images\clock.ico"),
                        Visible = true,
                        BalloonTipTitle = @"WorkTimer",
                        BalloonTipText = @"Click the show...",
                        Text = @"The App..."
                    };
        }

        public void Close()
        {
            Icon.Dispose();
            Icon = null;
        }

        #endregion



    }
}
