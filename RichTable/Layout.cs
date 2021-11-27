using System.Collections.Generic;
using System.Linq;

namespace Richx
{
    public static class Layout
    {
        public static LayoutArea Vertical(IEnumerable<object> objects) => Vertical(RichStyle.Default, objects.ToArray());
        public static LayoutArea Vertical(params object[] objects) => Vertical(RichStyle.Default, objects.ToArray());
        public static LayoutArea Vertical(RichStyle style, IEnumerable<object> objects) => Vertical(style, objects.ToArray());
        public static LayoutArea Vertical(RichStyle style, params object[] objects)
        {
            return new LayoutArea
            {
                Style = style,
                LeftToRight = false,
                TopToBottom = true,
                Objects = objects,
            };
        }

        public static LayoutArea Horizontal(IEnumerable<object> objects) => Horizontal(RichStyle.Default, objects.ToArray());
        public static LayoutArea Horizontal(params object[] objects) => Horizontal(RichStyle.Default, objects.ToArray());
        public static LayoutArea Horizontal(RichStyle style, IEnumerable<object> objects) => Horizontal(style, objects.ToArray());
        public static LayoutArea Horizontal(RichStyle style, params object[] objects)
        {
            return new LayoutArea
            {
                Style = style,
                LeftToRight = true,
                TopToBottom = false,
                Objects = objects,
            };
        }
    }
}
