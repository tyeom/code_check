using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Custompanel : Panel
    {
        Color _lineColor = Color.Black;
        int _linesWeight = 1;
        Color _borderFillColor = Color.Transparent;
        bool _isRound = false;
        cornerRadius _cornerRadius = new cornerRadius(30, 30, 30, 30);

        public Custompanel()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var graphicsPath = _getRoundRectangle(this.ClientRectangle))
            {
                //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                SolidBrush brush = new SolidBrush(BorderColor);
                Pen pen = new Pen(LineColor, LinesWeight);
                Pen pen2 = new Pen(Color.Red, 10);
                e.Graphics.FillPath(brush, graphicsPath);
                e.Graphics.DrawPath(pen, graphicsPath);
            }
        }

        private GraphicsPath _getRoundRectangle(Rectangle rectangle)
        {
            GraphicsPath path = new GraphicsPath();

            if (IsRound == true)
            {
                //좌상
                path.AddArc(LinesWeight / 2, LinesWeight / 2, CornerRadius.LeftTop, CornerRadius.LeftTop, 180, 90);
                //우상
                path.AddArc(rectangle.Width - CornerRadius.RightTop - 1 - LinesWeight / 2, LinesWeight / 2, CornerRadius.RightTop, CornerRadius.RightTop, 270, 90);
                //우하
                path.AddArc(rectangle.Width - CornerRadius.RightBottom - 1 - LinesWeight / 2, rectangle.Height - CornerRadius.RightBottom - 1 - LinesWeight / 2, CornerRadius.RightBottom, CornerRadius.RightBottom, 0, 90);
                //좌하
                path.AddArc(LinesWeight / 2, rectangle.Height - CornerRadius.LeftBottom - 1 - LinesWeight / 2, CornerRadius.LeftBottom, CornerRadius.LeftBottom, 90, 90);
            }
            else
            {
                Rectangle re = new Rectangle(new Point(LinesWeight / 2, LinesWeight / 2), new Size(rectangle.Width - LinesWeight, rectangle.Height - LinesWeight));

                path.AddRectangle(re);
            }

            path.CloseAllFigures();

            return path;
        }

        [Description("선 색깔"), Category("Custom mincook")]
        public Color LineColor { get { return _lineColor; } set { _lineColor = value; this.Invalidate(); } }

        [Description("배경 색깔"), Category("Custom mincook")]
        public Color BorderColor { get { return _borderFillColor; } set { _borderFillColor = value; this.Invalidate(); } }

        [Description("선 반경 사용유무"), Category("Custom mincook")]
        public bool IsRound { get { return _isRound; } set { _isRound = value; this.Invalidate(); } }

        [Description("선 굵기"), Category("Custom mincook")]
        public int LinesWeight { get { return _linesWeight; } set { _linesWeight = value; this.Invalidate(); } }



        //[Description("선 반경"), Category("Custom mincook")]
        //public cornerRadius CornerRadius
        //{
        //    get
        //    {
        //        return _cornerRadius;
        //    }
        //    set
        //    {
        //        _cornerRadius = value;

        //        this.Invalidate();
        //    }
        //}



        [Description("선 반경"), Category("Custom mincook")]
        public cornerRadius CornerRadius
        {
            get
            {
                return _cornerRadius;
            }
            set
            {
                if (value.LeftBottom > 0 && value.LeftTop > 0 && value.RightBottom > 0 && value.RightTop > 0)
                {
                    _cornerRadius = value;
                }
                else
                {
                    if (value.LeftTop <= 0)
                    {
                        value.LeftTop = 1;
                        _cornerRadius = value;
                    }
                    if (value.LeftBottom <= 0)
                    {
                        value.LeftBottom = 1;
                        _cornerRadius = value;
                    }
                    if (value.RightBottom <= 0)
                    {
                        value.RightBottom = 1;
                        _cornerRadius = value;
                    }
                    if (value.RightTop <= 0)
                    {
                        value.RightTop = 1;
                        _cornerRadius = value;
                    }
                }

                this.Invalidate();
            }
        }

        [TypeConverter("WindowsFormsApp1.Custompanel+cornerRadiusTypeConverter")]
        public struct cornerRadius
        {
            public cornerRadius(int leftBottom, int leftTop, int rightBottom, int rightTop)
            {
                LeftBottom = leftBottom;
                LeftTop = leftTop;
                RightBottom = rightBottom;
                RightTop = rightTop;
            }

            [Description("좌하 라운드")]
            public int LeftBottom { get; set; }
            [Description("좌상 라운드")]
            public int LeftTop { get; set; }
            [Description("우하 라운드")]
            public int RightBottom { get; set; }
            [Description("우상 라운드")]
            public int RightTop { get; set; }

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}, {3}", LeftBottom, LeftTop, RightBottom, RightTop);
            }
        }




        public class cornerRadiusTypeConverter2 : ExpandableObjectConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(InstanceDescriptor))
                    return true;
                return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertTo(ITypeDescriptorContext context,
                CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(InstanceDescriptor))
                {
                    ConstructorInfo ci = typeof(cornerRadius).GetConstructor(new Type[] { typeof(string) });
                    cornerRadius t = (cornerRadius)value;
                    return new InstanceDescriptor(ci, new object[] { t.LeftBottom, t.LeftTop, t.RightBottom, t.RightTop });
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
            public override object CreateInstance(ITypeDescriptorContext context,
                System.Collections.IDictionary propertyValues)
            {
                if (propertyValues == null)
                    throw new ArgumentNullException("propertyValues");



                object text = propertyValues["LeftBottom"];
                object text2 = propertyValues["LeftTop"];
                object text3 = propertyValues["RightBottom"];
                object text4 = propertyValues["RightTop"];

                return new cornerRadius((int)text, (int)text2, (int)text3, (int)text4);


            }
            public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
            {
                return true;
            }
        }



        public class cornerRadiusTypeConverter : ExpandableObjectConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                {
                    return true;
                }

                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context,
                System.Globalization.CultureInfo culture, object value)
            {

                string data = value.ToString().Replace(" ", "");

                int LeftBottom = Convert.ToInt32(data.Substring(0, data.IndexOf(",")));
                data = data.Substring(data.IndexOf(",") + 1);

                int LeftTop = Convert.ToInt32(data.Substring(0, data.IndexOf(",")));
                data = data.Substring(data.IndexOf(",") + 1);

                int RightBottom = Convert.ToInt32(data.Substring(0, data.IndexOf(",")));
                data = data.Substring(data.IndexOf(",") + 1);

                int RightTop = Convert.ToInt32(data);

                return new cornerRadius(LeftBottom, LeftTop, RightBottom, RightTop);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(InstanceDescriptor))
                    return true;
                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertTo(ITypeDescriptorContext context,
                CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(InstanceDescriptor))
                {
                    ConstructorInfo ci = typeof(cornerRadius).GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) });
                    cornerRadius t = (cornerRadius)value;
                    return new InstanceDescriptor(ci, new object[] { t.LeftBottom, t.LeftTop, t.RightBottom, t.RightTop });
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override object CreateInstance(ITypeDescriptorContext context,
                System.Collections.IDictionary propertyValues)
            {
                if (propertyValues == null)
                    throw new ArgumentNullException("propertyValues");

                object text = propertyValues["LeftBottom"];
                object text2 = propertyValues["LeftTop"];
                object text3 = propertyValues["RightBottom"];
                object text4 = propertyValues["RightTop"];

                return new cornerRadius((int)text, (int)text2, (int)text3, (int)text4);
            }


            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                //return base.GetProperties(context, value, attributes);
                return TypeDescriptor.GetProperties(value, attributes);
            }

            public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
            {
                return true;
            }

        }
    }
}
