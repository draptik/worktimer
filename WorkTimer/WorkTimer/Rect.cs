using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WorkTimer
{
    public class Rect : AbstractGuiObject
    {
        private readonly RotateTransform _rotateTransform;
        private Rectangle _rectangle;
        public Visibility Visibility { get; set; }

        public Rect(RotateTransform rotateTransform, Rectangle rectangle)
        {
            _rotateTransform = rotateTransform;
            _rectangle = rectangle;
        }

        public void Update(DateTime dateTime)
        {
            _rotateTransform.Angle = GetAngle(dateTime);
        }
    }
}
