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

        public static readonly CellSpan Span = new();

        public static Layout Vertical(IEnumerable<object> objects) => VerticalAny(RichStyle.Default, objects.OfType<object>().ToArray());
        public static Layout Vertical<T>(IEnumerable<T> objects) => VerticalAny(RichStyle.Default, objects.OfType<object>().ToArray());
        public static Layout Vertical<T>(RichStyle style, IEnumerable<T> objects) => VerticalAny(style, objects.OfType<object>().ToArray());

        public static Layout VerticalAny(params object[] objects) => VerticalAny(RichStyle.Default, objects.ToArray());
        public static Layout VerticalAny(RichStyle style, params object[] objects)
        {
            return new Layout
            {
                Style = style,
                LeftToRight = false,
                TopToBottom = true,
                Objects = objects,
            };
        }

        public static Layout Horizontal(IEnumerable<object> objects) => HorizontalAny(RichStyle.Default, objects.OfType<object>().ToArray());
        public static Layout Horizontal<T>(IEnumerable<T> objects) => HorizontalAny(RichStyle.Default, objects.OfType<object>().ToArray());
        public static Layout Horizontal<T>(RichStyle style, IEnumerable<T> objects) => HorizontalAny(style, objects.OfType<object>().ToArray());

        public static Layout HorizontalAny(params object[] objects) => HorizontalAny(RichStyle.Default, objects.ToArray());
        public static Layout HorizontalAny(RichStyle style, params object[] objects)
        {
            return new Layout
            {
                Style = style,
                LeftToRight = true,
                TopToBottom = false,
                Objects = objects,
            };
        }
    }
}
