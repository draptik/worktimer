using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WorkTimer
{
    public class Rect : AbstractGuiObject
    {
        private readonly RotateTransform _rotateTransform;
        private readonly Rectangle _rectangle;
        public bool Visibility
        {
            set { _rectangle.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden; }
        }
        public double Width { get { return _rectangle.Width; } set { _rectangle.Width = value; } }
        public Brush Color { 
            set { _rectangle.Fill = value; _rectangle.Stroke = value; } 
        }
        public Thickness Margin { set { _rectangle.Margin = value; } } 

        public Rect(Rectangle rectangle) : this(new RotateTransform(0), rectangle)
        {
        }

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
