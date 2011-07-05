using System;
using System.Globalization;
using System.Windows.Media;

namespace WorkTimer.Domain
{
    public class Config
    {
        #region Singleton
        
        private static Config _instance;

        public static Config GetInstance()
        {
            return _instance ?? (_instance = new Config());
        }

        private Config()
        {
            Init();
        }

        #endregion
        
        private void Init()
        {
            TitleString = "Should I Stay Or Should I Go Now";

            MinTimeColor        = Colors.YellowGreen;
            MaxTimeColor        = Colors.RosyBrown;
            TargetTimeColor     = Colors.SkyBlue;
            TimeSpentColor      = Colors.Aquamarine;
            WarnBackgroundColor = Colors.LightPink;
            OkBackgroundColor   = Colors.LightGreen;
            
            WarningTimeSpanMax       = new TimeSpan(0, 30, 0);
            WarningTimeSpanInterval  = new TimeSpan(0, 0, 5);

            CurrentCultureInfo = new CultureInfo("de-DE");

            TimeFormat = "H:mm";

            BreakTimeNum    = 0.75;
            TargetTimeNum   = 8.75;
            MinTimeStartNum = 6.0;
            MaxTimeNum      = 10.75;
            
        }

        

        #region Properties

        public string TitleString { get; set; }

        public CultureInfo CurrentCultureInfo { get; set; }
        public Color MinTimeColor { get; set; }
        public Color MaxTimeColor { get; set; }
        public Color TargetTimeColor { get; set; }
        public Color TimeSpentColor { get; set; }
        public Color WarnBackgroundColor { get; set; }
        public Color OkBackgroundColor { get; set; }
        
        public TimeSpan WarningTimeSpanMax { get; set; }
        public TimeSpan WarningTimeSpanInterval { get; set; }
        
        public string TimeFormat { get; set; }

        public double BreakTimeNum { get; set; }
        public double TargetTimeNum { get; set; }
        public double MinTimeStartNum { get; set; }
        public double MaxTimeNum { get; set; }
        
        public TimeSpan MaxTimeSpan { get { return TimeSpan.FromHours(MaxTimeNum); } }
        public TimeSpan MinTimeSpan { get { return TimeSpan.FromHours(MinTimeStartNum); } }
        public TimeSpan TargetTimeSpan { get { return TimeSpan.FromHours(TargetTimeNum); } }
        public TimeSpan BreakTimeSpan { get { return TimeSpan.FromHours(BreakTimeNum); } }

        #endregion
    }
}