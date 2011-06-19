using System;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WorkTimer
{
    public class Arc : AbstractGuiObject
    {
        public Path Path { get; set; }
        public LineSegment LineSegment { get; set; }
        public ArcSegment ArcSegment { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Radius { get; set; }
        public bool IsLargeArc { get; set; }
        public Point ZeroPos { get; set; }
        public bool Visibility
        {
            set { Path.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden; }
        }

        public Arc(Path path, LineSegment lineSegment, ArcSegment arcSegment, Point zeroPos, Brush brush)
        {
            Path = path;
            LineSegment = lineSegment;
            ArcSegment = arcSegment;
            ZeroPos = zeroPos;
            Path.Stroke = brush;
            Path.Fill = brush;
        }

        public void Update(DateTime startTime, DateTime endTime, double radius, bool isLargeArc)
        {
            LineSegment.Point = TransformDate(startTime, radius, ZeroPos);
            ArcSegment.Point = TransformDate(endTime, radius, ZeroPos);
            ArcSegment.Size = new Size(radius, radius);
            ArcSegment.IsLargeArc = isLargeArc;
        }
    }
}
