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

        public static Layout Vertical<T>(IEnumerable<T> objects) => Vertical(RichStyle.Default, objects.ToArray());
        public static Layout Vertical(IEnumerable<object> objects) => Vertical(RichStyle.Default, objects.ToArray());
        public static Layout Vertical(params object[] objects) => Vertical(RichStyle.Default, objects.ToArray());
        public static Layout Vertical(RichStyle style, IEnumerable<object> objects) => Vertical(style, objects.ToArray());
        public static Layout Vertical(RichStyle style, params object[] objects)
        {
            return new Layout
            {
                Style = style,
                LeftToRight = false,
                TopToBottom = true,
                Objects = objects,
            };
        }

        public static Layout Horizontal<T>(IEnumerable<T> objects) => Horizontal(RichStyle.Default, objects.ToArray());
        public static Layout Horizontal(IEnumerable<object> objects) => Horizontal(RichStyle.Default, objects.ToArray());
        public static Layout Horizontal(params object[] objects) => Horizontal(RichStyle.Default, objects.ToArray());
        public static Layout Horizontal(RichStyle style, IEnumerable<object> objects) => Horizontal(style, objects.ToArray());
        public static Layout Horizontal(RichStyle style, params object[] objects)
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
