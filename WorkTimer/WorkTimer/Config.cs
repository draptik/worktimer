using System.Windows.Media;

namespace WorkTimer
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
            MinTimeBrush = new SolidColorBrush { Color = Colors.YellowGreen };
            MaxTimeBrush = new SolidColorBrush { Color = Colors.RosyBrown };
            TargetTimeBrush = new SolidColorBrush { Color = Colors.SkyBlue };
            TimeSpentBrush = new SolidColorBrush { Color = Colors.Aquamarine };
        }

        public Brush MinTimeBrush { get; set; }
        public Brush MaxTimeBrush { get; set; }
        public Brush TargetTimeBrush { get; set; }
        public Brush TimeSpentBrush { get; set; }

    }
}
