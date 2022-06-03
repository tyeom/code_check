namespace SimpleMVVMWpf.Common.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class SpacingPanel : Panel
    {
        public static readonly DependencyProperty SpacingIndexProperty =
            DependencyProperty.Register("SpacingIndex",
                typeof(int),
                typeof(SpacingPanel),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnSpacingIndexChanged)));

        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.Register("Column",
                typeof(int),
                typeof(SpacingPanel),
                new FrameworkPropertyMetadata(1, new PropertyChangedCallback(OnColumnChanged)));

        public int SpacingIndex
        {
            get
            {
                return (int)base.GetValue(SpacingIndexProperty);
            }
            set
            {
                base.SetValue(SpacingIndexProperty, value);
            }
        }

        public int Column
        {
            get
            {
                return (int)base.GetValue(ColumnProperty);
            }
            set
            {
                base.SetValue(ColumnProperty, value);
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(availableSize);
            }

            var positions = new Point[InternalChildren.Count];
            var desiredHeight = ArrangeChildren(positions, availableSize.Width);

            return new Size(availableSize.Width, desiredHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var positions = new Point[InternalChildren.Count];
            ArrangeChildren(positions, finalSize.Width);

            for (int i = 0; i < InternalChildren.Count; i++)
            {
                var child = InternalChildren[i];
                child.Arrange(new Rect(positions[i], child.DesiredSize));
            }

            return finalSize;
        }

        private double ArrangeChildren(Point[] positions, double availableWidth)
        {
            var lastRowStartIndex = -1;
            var lastRowEndIndex = 0;
            var currentWidth = 0d;
            var desiredHeight = 0d;

            // 간격 컬럼 임시 인덱스
            int tmpSpacingCol = 0;
            // 이번 ChildIndex에서 간격 설정 여부
            bool tmpIsSpacingCol = false;
            for (int childIndex = 0; childIndex < InternalChildren.Count; childIndex++)
            {
                var child = InternalChildren[childIndex];
                var x = 0d;
                var y = 0d;

                bool isNewLine = (childIndex % Column) == 0;

                if (currentWidth == 0d || isNewLine == false)
                {
                    if (tmpIsSpacingCol)
                        currentWidth += 50;
                    tmpIsSpacingCol = false;

                    x = currentWidth;
                    currentWidth += child.DesiredSize.Width;
                }
                else
                {
                    currentWidth = child.DesiredSize.Width;
                    lastRowStartIndex = lastRowEndIndex;
                    lastRowEndIndex = childIndex;
                    tmpSpacingCol = 0;
                }

                if (SpacingIndex > 1)
                {
                    bool isSpacing = ((tmpSpacingCol + 1) % SpacingIndex) == 0;
                    if ((tmpSpacingCol + 1) < Column && isSpacing)
                        tmpIsSpacingCol = true;

                    tmpSpacingCol++;
                }

                if (lastRowStartIndex >= 0)
                {
                    int i = lastRowStartIndex;

                    while (i < lastRowEndIndex - 1 && positions[i + 1].X < x)
                    {
                        i++;
                    }

                    while (i < lastRowEndIndex && positions[i].X < x + child.DesiredSize.Width)
                    {
                        //y = Math.Max(y, positions[i].Y + InternalChildren[i].DesiredSize.Height);
                        y = positions[i].Y + InternalChildren[i].DesiredSize.Height;
                        i++;
                    }
                }

                positions[childIndex] = new Point(x, y);
                desiredHeight = Math.Max(desiredHeight, y + child.DesiredSize.Height);
            }

            return desiredHeight;
        }

        public static void OnSpacingIndexChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SpacingPanel spacingPanel = sender as SpacingPanel;
            spacingPanel.InvalidateVisual();
        }

        public static void OnColumnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SpacingPanel spacingPanel = sender as SpacingPanel;
            spacingPanel.InvalidateVisual();
        }
    }
}
