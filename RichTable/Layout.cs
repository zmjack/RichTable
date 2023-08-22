using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Richx
{
    public class Layout : IEnumerable<object>
    {
        public class CellSpan
        {
            public static readonly CellSpan Single = new(1);
            public int Span { get; set; }

            public CellSpan(int span)
            {
                Span = span;
            }
        }

        private readonly List<object> _objectList = new();

        public RichStyle Style { get; set; }
        public bool LeftToRight { get; set; }
        public bool TopToBottom { get; set; }
        public object[] Objects
        {
            get => _objectList.ToArray();
            set
            {
                _objectList.Clear();
                _objectList.AddRange(value);
            }
        }
        public bool Single { get; set; }

        public static CellSpan Span => CellSpan.Single;
        public static CellSpan Spans(int span) => new(span);

        public Layout WithStyle(RichStyle style)
        {
            Style = style;
            return this;
        }

        public static Layout Const(object obj)
        {
            return new Layout
            {
                Style = RichStyle.Default,
                LeftToRight = true,
                TopToBottom = false,
                Objects = new[] { obj },
                Single = true,
            };
        }

        public static Layout Vertical(IEnumerable<object> objects) => VerticalAny(objects.Cast<object>().ToArray());
        public static Layout Vertical<T>(IEnumerable<T> objects) => VerticalAny(objects.Cast<object>().ToArray());
        public static Layout Vertical<T>(params T[] objects) => VerticalAny(objects.Cast<object>().ToArray());

        public static Layout VerticalAny(params object[] objects)
        {
            return new Layout
            {
                Style = RichStyle.Default,
                LeftToRight = false,
                TopToBottom = true,
                Objects = objects,
            };
        }

        public static Layout Horizontal(IEnumerable<object> objects) => HorizontalAny(objects.Cast<object>().ToArray());
        public static Layout Horizontal<T>(IEnumerable<T> objects) => HorizontalAny(objects.Cast<object>().ToArray());
        public static Layout Horizontal<T>(params T[] objects) => HorizontalAny(objects.Cast<object>().ToArray());

        public static Layout HorizontalAny(params object[] objects)
        {
            return new Layout
            {
                Style = RichStyle.Default,
                LeftToRight = true,
                TopToBottom = false,
                Objects = objects,
            };
        }

        public IEnumerator<object> GetEnumerator()
        {
            return ((IEnumerable<object>)Objects).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Objects.GetEnumerator();
        }

        public class Constant : Layout
        {
            public Constant(object value) : this(value, RichStyle.Default) { }
            public Constant(object value, RichStyle style)
            {
                Style = style;
                LeftToRight = true;
                TopToBottom = false;
                Objects = new[] { value };
                Single = true;
            }
        }

        public class Hori : Layout
        {
            public Hori() : this(RichStyle.Default) { }
            public Hori(RichStyle style)
            {
                Style = style;
                LeftToRight = true;
                TopToBottom = false;
            }

            public void Add(object value)
            {
                _objectList.Add(value);
            }
        }

        public class Vert : Layout
        {
            public Vert() : this(RichStyle.Default) { }
            public Vert(RichStyle style)
            {
                Style = style;
                LeftToRight = false;
                TopToBottom = true;
            }

            public void Add(object value)
            {
                _objectList.Add(value);
            }
        }
    }
}
