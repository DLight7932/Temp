using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace CrossplatformButton
{
    enum HorizontalAlignment
    {
        Left,
        Midle,
        Right
    }
    enum VerticalAlignment
    {
        Top,
        Midle,
        Bottom
    }

    public struct VectorInt2
    {
        public int x;
        public int y;
        public VectorInt2(int x_, int y_)
        {
            x = x_;
            y = y_;
        }
    }

    class MyButton
    {
        public delegate void MyMouseEventHandler(object sender, MouseEventArgs e);
        public event MyMouseEventHandler MyMouseMove;
        public event MyMouseEventHandler MyMouseDown;
        public event MyMouseEventHandler MyMouseUp;
        public event MyMouseEventHandler MyMouseClick;
        public delegate void MyEventHandler(object sender, EventArgs e);
        public event MyEventHandler MyMouseEnter;
        public event MyEventHandler MyMouseLeave;

        public bool Enabled = true;
        public bool Visible = true;
        protected bool MouseInside;
        protected bool MouseDown;

        protected virtual bool IsInside(Point point)
        {
            return point.X > Left && point.X < Right && point.Y > Top && point.Y < Bottom;
        }

        public void ControlMouseMoveEvent(object sender, MouseEventArgs e)
        {
            if (IsInside(new Point(e.X, e.Y)))
            {
                if (!MouseInside)
                {
                    MouseInside = true;
                    MyMouseEnter?.Invoke(sender, null);
                }
                MyMouseMove?.Invoke(sender, e);
            }
            else if (MouseInside)
            {
                MouseInside = false;
                MouseDown = false;
                MyMouseLeave?.Invoke(sender, null);
            }
        }
        public void ControlMouseDownEvent(object sender, MouseEventArgs e)
        {
            if (e.X > Left && e.X < Right && e.Y > Top && e.Y < Bottom)
            {
                MyMouseDown?.Invoke(sender, e);
                MouseDown = true;
            }
        }
        public void ControlMouseUpEvent(object sender, MouseEventArgs e)
        {
            if (e.X > Left && e.X < Right && e.Y > Top && e.Y < Bottom)
            {
                MyMouseUp?.Invoke(sender, e);
                if (MouseDown && Enabled)
                {
                    MyMouseClick?.Invoke(sender, e);
                    MouseDown = false;
                }
            }
        }

        protected StringFormat StringFormat = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        public string text;
        public Font Font = new Font("Consolas", 10);
        public VectorInt2 position;
        public VectorInt2 scale;
        public HorizontalAlignment HorizontalAlignment;
        public VerticalAlignment VerticalAlignment;
        public Bitmap Icon;

        protected Color foreColor;
        protected Brush foreColorBrush = new SolidBrush(Color.Black);
        public Color ForeColor
        {
            get
            {
                return foreColor;
            }
            set
            {
                foreColor = value;
                foreColorBrush = new SolidBrush(value);
            }
        }

        public int Width
        {
            get
            {
                return scale.x;
            }
        }
        public int Height
        {
            get
            {
                return scale.y;
            }
        }

        public int Left
        {
            get
            {
                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        return position.x - scale.x;
                    case HorizontalAlignment.Midle:
                        return position.x - scale.x / 2;
                    case HorizontalAlignment.Right:
                        return position.x;
                    default:
                        throw new Exception();
                }
            }
        }
        public int Right
        {
            get
            {
                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        return position.x;
                    case HorizontalAlignment.Midle:
                        return position.x + scale.x / 2;
                    case HorizontalAlignment.Right:
                        return position.x + scale.x;
                    default:
                        throw new Exception();
                }
            }
        }
        public int Top
        {
            get
            {
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        return position.y - scale.y;
                    case VerticalAlignment.Midle:
                        return position.y - scale.y / 2;
                    case VerticalAlignment.Bottom:
                        return position.y;
                    default:
                        throw new Exception();
                }
            }
        }
        public int Bottom
        {
            get
            {
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        return position.y;
                    case VerticalAlignment.Midle:
                        return position.y + scale.y / 2;
                    case VerticalAlignment.Bottom:
                        return position.y + scale.y;
                    default:
                        throw new Exception();
                }
            }
        }

        public int VerticalCenter
        {
            get
            {
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        return position.y - scale.y / 2;
                    case VerticalAlignment.Midle:
                        return position.y;
                    case VerticalAlignment.Bottom:
                        return position.y + scale.y / 2;
                    default:
                        throw new Exception();
                }
            }
        }
        public int HorizontalCenter
        {
            get
            {
                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        return position.x - scale.x / 2;
                    case HorizontalAlignment.Midle:
                        return position.x;
                    case HorizontalAlignment.Right:
                        return position.x + scale.x / 2;
                    default:
                        throw new Exception();
                }
            }
        }

        public Rectangle Rectangle => new Rectangle(Left, Top, Width, Height);

        public virtual void Display(Graphics g)
        {
            if (!Visible) return;

            g.DrawRectangle(new Pen(Color.Black), Rectangle);
            if (Icon != null)
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(Icon, new Rectangle(Rectangle.X + 1, Rectangle.Y + 1, Rectangle.Width, Rectangle.Height));
            }
            g.DrawString(text, Font, foreColorBrush, HorizontalCenter, VerticalCenter, StringFormat);
            if (!Enabled)
            {
                using (Brush shadowBrush = new SolidBrush(Color.FromArgb(200, Color.Black)))
                {
                    g.FillRectangle(shadowBrush, new Rectangle(Rectangle.X + 1, Rectangle.Y + 1, Rectangle.Width - 1, Rectangle.Height - 1));
                }
            }

            if (!Enabled) return;

            if (MouseDown)
            {
                using (Brush shadowBrush = new SolidBrush(Color.FromArgb(200, Color.Gray)))
                {
                    g.FillRectangle(shadowBrush, new Rectangle(Rectangle.X + 1, Rectangle.Y + 1, Rectangle.Width - 1, Rectangle.Height - 1));
                }
            }
            else if (MouseInside)
            {
                using (Brush shadowBrush = new SolidBrush(Color.FromArgb(200, Color.White)))
                {
                    g.FillRectangle(shadowBrush, new Rectangle(Rectangle.X + 1, Rectangle.Y + 1, Rectangle.Width - 1, Rectangle.Height - 1));
                }
            }
        }
    }

    class MyRoundcornerButton : MyButton
    {
        Color color = Color.White;
        Brush colorBrush = new SolidBrush(Color.White);
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                colorBrush = new SolidBrush(value);
            }
        }

        Color frameColor = Color.Black;
        Pen frameColorPen = new Pen(Color.Black) { Width = 3 };
        public Color FrameColor
        {
            get
            {
                return frameColor;
            }
            set
            {
                frameColor = value;
                frameColorPen = new Pen(value) { Width = 3 };
            }
        }

        Color highlightColor = Color.FromArgb(100, Color.Blue);
        Brush highlightColorBrush = new SolidBrush(Color.FromArgb(100, Color.Blue));
        public Color HighlightColor
        {
            get
            {
                return highlightColor;
            }
            set
            {
                highlightColor = value;
                highlightColorBrush = new SolidBrush(value);
            }
        }

        Color clickHighlightColor = Color.FromArgb(100, Color.Red);
        Brush clickHighlightColorBrush = new SolidBrush(Color.FromArgb(100, Color.Red));
        public Color ClickHighlightColor
        {
            get
            {
                return clickHighlightColor;
            }
            set
            {
                clickHighlightColor = value;
                clickHighlightColorBrush = new SolidBrush(value);
            }
        }

        int roundSize;
        public int RoundSize
        {
            get
            {
                return roundSize;
            }
            set
            {
                roundSize = value;
                gp = new GraphicsPath();
                float roundSizef = Width < Height ? Width / 100f * RoundSize : Height / 100f * RoundSize;
                if (roundSizef == 0f) roundSizef = 0.01f;
                gp.AddArc(Rectangle.X, Rectangle.Y, roundSizef, roundSizef, 180, 90);
                gp.AddArc(Rectangle.X + Rectangle.Width - roundSizef, Rectangle.Y, roundSizef, roundSizef, 270, 90);
                gp.AddArc(Rectangle.X + Rectangle.Width - roundSizef, Rectangle.Y + Rectangle.Height - roundSizef, roundSizef, roundSizef, 0, 90);
                gp.AddArc(Rectangle.X, Rectangle.Y + Rectangle.Height - roundSizef, roundSizef, roundSizef, 90, 90);
                gp.CloseFigure();
            }
        }
        GraphicsPath gp;

        public override void Display(Graphics g)
        {
            if (!Visible) return;

            g.SmoothingMode = SmoothingMode.HighQuality;

            g.Clip = new Region(gp);

            if (Icon != null)
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(Icon, new Rectangle(Rectangle.X + 1, Rectangle.Y + 1, Rectangle.Width, Rectangle.Height));
            }
            else
                g.FillPath(colorBrush, gp);

            if (MouseDown)
                g.FillPath(clickHighlightColorBrush, gp);

            else if (MouseInside)
                g.FillPath(highlightColorBrush, gp);

            g.DrawPath(frameColorPen, gp);

            g.DrawString(text, Font, foreColorBrush, HorizontalCenter, VerticalCenter, StringFormat);
        }

        protected override bool IsInside(Point point)
        {
            return gp.IsVisible(point);
        }
    }
}
