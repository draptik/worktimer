using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkTimer.Gui.EventArgs
{
    public class TimeCheckBoxesEventArgs : System.EventArgs
    {
        public bool MinTimeChecked { get; set; }
        public bool MaxTimeChecked { get; set; }
        public bool TimeSpentChecked { get; set; }
        public bool TargetTimeChecked { get; set; }
    }
}
