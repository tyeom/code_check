namespace SimpleMVVMWpf.Models
{
    using SimpleMVVMWpf.Base;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;

    public class PolygonModel : BaseModel
    {
        private List<Point> _pointList = new List<Point>();
        private PointCollection _points;

        public int Top
        {
            get; set;
        }

        public int Left
        {
            get; set;
        }

        [NotifyParentProperty(true)]
        public PointCollection Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }

        public void AddPoint(Point point)
        {
            _pointList.Add(point);

            Points = new PointCollection(_pointList.ToArray());
        }
    }
}
