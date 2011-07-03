using System;
using System.Windows;
using System.Windows.Media;

namespace WorkTimer.Gui.Shapes
{
    public abstract class AbstractGuiObject
    {
        public static Point TransformDate(DateTime dateTime, double radius, Point zeroPos)
        {
            return TransformPoint(GetPointRelative(dateTime, radius), zeroPos);
        }

        public static Point TransformPoint(Point point, Point zeroPos)
        {
            return new Point(zeroPos.X + point.X, zeroPos.Y - point.Y);
        }

        public static Point GetPointRelative(DateTime dateTime, double radius)
        {
            return new Point
            {
                X = radius * Math.Sin(GetRadians(dateTime)),
                Y = radius * Math.Cos(GetRadians(dateTime))
            };
        }

        public static double GetRadians(DateTime dateTime)
        {
            return GetAngle(dateTime) * (Math.PI / 180);
        }

        public static double GetAngle(DateTime dateTime)
        {
            return dateTime.Hour * 30 + dateTime.Minute * 0.5;
        }

        public Brush ToBrush(Color color)
        {
            return new SolidColorBrush { Color = color };
        }
    }
}
