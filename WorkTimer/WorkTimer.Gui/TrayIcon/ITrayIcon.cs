using System.Windows.Forms;

namespace WorkTimer.Gui.TrayIcon
{
    public interface ITrayIcon
    {
        NotifyIcon Icon { get; }
        void Init();
        void Close();
    }
}