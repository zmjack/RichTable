using System.Collections.Generic;
using System.Linq;

namespace Richx
{
    public class Layout
    {
        public class CellSpan { }

        public RichStyle Style { get; set; }
        public bool LeftToRight { get; set; }
        public bool TopToBottom { get; set; }
        public object[] Objects { get; set; }
        public bool Single { get; set; }

        public static readonly CellSpan Span = new();

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
    }
}
