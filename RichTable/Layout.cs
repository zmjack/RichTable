using System;
using System.Collections;
using System.Collections.Generic;

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

        public enum RenderDirection
        {
            LeftToRight,
            TopToBottom,
        }

        private static ArgumentException SingleCellCannotSetMultipleValues() => new("Single cell cannot set multiple values.");

        private readonly List<object> _objectList = new();
        public object[] Objects
        {
            get => _objectList.ToArray();
            protected set
            {
                _objectList.Clear();
                _objectList.AddRange(value);
            }
        }

        public RichStyle[] Styles { get; protected set; }
        public RenderDirection Direction { get; protected set; }
        public bool Single => SpanValue > 0;
        public int SpanValue { get; set; }

        public IEnumerator<object> GetEnumerator()
        {
            return ((IEnumerable<object>)Objects).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Objects.GetEnumerator();
        }

        public void Add(object value)
        {
            if (value is Layout || value is string)
            {
                if (Single && _objectList.Count > 0) throw SingleCellCannotSetMultipleValues();

                _objectList.Add(value);
            }
            else if (value is IEnumerable enumerable)
            {
                if (Single) throw SingleCellCannotSetMultipleValues();

                var enumerator = enumerable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    _objectList.Add(enumerator.Current);
                }
            }
            else _objectList.Add(value);
        }

        public class Span : Layout
        {
            public Span(int span = 1, params RichStyle[] styles)
            {
                Styles = styles;
                Direction = RenderDirection.LeftToRight;
                SpanValue = span < 1 ? 1 : span;
            }
        }

        public class Hori : Layout
        {
            public Hori(params RichStyle[] styles)
            {
                Styles = styles;
                Direction = RenderDirection.LeftToRight;
            }
        }

        public class Vert : Layout
        {
            public Vert(params RichStyle[] styles)
            {
                Styles = styles;
                Direction = RenderDirection.TopToBottom;
            }
        }
    }
}
