using NStandard;
using System;
using System.Collections.Generic;

namespace Richx
{
    public class Masking
    {
        public Cursor Cursor { get; private set; }

        public RichTable Table { get; private set; }
        public Masking Parent { get; set; }
        public Cursor Start { get; private set; }
        public Cursor End { get; private set; }
        public RichStyle[] ParentStyles { get; private set; }
        public AfterCursor AfterCursor { get; private set; }

        internal Masking(RichTable table, Masking parent, Cursor start, AfterCursor afterCursor, RichStyle[] parentStyles)
        {
            Table = table;
            Parent = parent;
            Cursor = start;
            AfterCursor = afterCursor;
            ParentStyles = parentStyles;
            Start = End = start;
        }

        public void Paint(Layout layout)
        {
            var afterCursor = layout.Direction == Layout.RenderDirection.LeftToRight ? AfterCursor.AsideTopRight : AfterCursor.UnderBottomLeft;
            var styles = layout.Styles.Length == 0 ? ParentStyles : layout.Styles;
            var styleLength = styles.Length;

            Cursor GetNextCursor(int offset = 1)
            {
                return afterCursor switch
                {
                    AfterCursor.AsideTopRight => (Cursor)(Cursor.Row, Cursor.Col + offset),
                    AfterCursor.UnderBottomLeft => (Cursor)(Cursor.Row + offset, Cursor.Col),
                    _ => throw new NotImplementedException(),
                };
            }

            var singleCells = new List<Cursor>();
            int? mergeTo = null;

            foreach (var (index, obj) in layout.Objects.AsIndexValuePairs())
            {
                if (obj is Layout subLayout)
                {
                    var subMasking = new Masking(Table, this, Cursor, afterCursor, styles);
                    subMasking.Paint(subLayout);

                    if (subLayout.Single)
                    {
                        singleCells.Add(Cursor);
                        ExtendBound(Cursor);

                        if (subLayout.SpanValue > 1)
                        {
                            var toCursor = GetNextCursor(subLayout.SpanValue - 1);
                            Table[Cursor, toCursor].Merge();
                            ExtendBound(toCursor);
                            Cursor = GetNextCursor(subLayout.SpanValue);
                        }
                        else Cursor = GetNextCursor();
                    }
                    else
                    {
                        ExtendBound(subMasking.Start, subMasking.End);
                        Cursor = subMasking.GetAfterCursor();

                        if (afterCursor == AfterCursor.UnderBottomLeft)
                        {
                            var value = subMasking.End.Col;
                            mergeTo = mergeTo.HasValue ? Math.Max(mergeTo.Value, value) : value;
                        }
                        else if (afterCursor == AfterCursor.AsideTopRight)
                        {
                            var value = subMasking.End.Row;
                            mergeTo = mergeTo.HasValue ? Math.Max(mergeTo.Value, value) : value;
                        }
                    }

                }
                else
                {
                    if (styleLength > 0)
                    {
                        Table[Cursor].Style = styles[index % styleLength];
                    }
                    Table[Cursor].Value = obj;

                    singleCells.Add(Cursor);
                    ExtendBound(Cursor);
                    Cursor = GetNextCursor();
                }
            }

            if (mergeTo is not null)
            {
                foreach (var cursor in singleCells)
                {
                    if (afterCursor == AfterCursor.AsideTopRight && (cursor != (mergeTo.Value, cursor.Col)))
                    {
                        Table[cursor, (mergeTo.Value, cursor.Col)].Merge();
                    }
                    else if (afterCursor == AfterCursor.UnderBottomLeft && (cursor != (cursor.Row, mergeTo.Value)))
                    {
                        Table[cursor, (cursor.Row, mergeTo.Value)].Merge();
                    }
                }
            }
        }

        private Cursor GetAfterCursor()
        {
            return AfterCursor switch
            {
                AfterCursor.Default => (End.Row + 1, Start.Col),

                AfterCursor.TopLeft => Start,
                AfterCursor.TopRight => (Start.Row, End.Col),
                AfterCursor.BottomLeft => (End.Row, Start.Col),
                AfterCursor.BottomRight => End,

                AfterCursor.AsideTopLeft => (Start.Row, Start.Col - 1),
                AfterCursor.AsideTopRight => (Start.Row, End.Col + 1),
                AfterCursor.AsideBottomLeft => (End.Row, Start.Col - 1),
                AfterCursor.AsideBottomRight => (End.Row, End.Col + 1),

                AfterCursor.OverTopLeft => (Start.Row - 1, Start.Col),
                AfterCursor.OverTopRight => (Start.Row - 1, End.Col),
                AfterCursor.UnderBottomLeft => (End.Row + 1, Start.Col),
                AfterCursor.UnderBottomRight => (End.Row + 1, End.Col),

                _ => throw new NotImplementedException(),
            };
        }

        private void ExtendBound(Cursor cursor)
        {
            Start = (Math.Min(Start.Row, cursor.Row), Math.Min(Start.Col, cursor.Col));
            End = (Math.Max(End.Row, cursor.Row), Math.Max(End.Col, cursor.Col));
        }

        private void ExtendBound(Cursor start, Cursor end)
        {
            Start = (Math.Min(Start.Row, start.Row), Math.Min(Start.Col, start.Col));
            End = (Math.Max(End.Row, end.Row), Math.Max(End.Col, end.Col));
        }
    }
}
