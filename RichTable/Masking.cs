using System;
using System.Collections.Generic;
using System.Linq;

namespace Richx
{
    public class Masking
    {
        public Cursor Cursor { get; private set; }

        public RichTable Table { get; private set; }
        public Masking Parent { get; set; }
        public Cursor Start { get; private set; }
        public Cursor End { get; private set; }
        public RichStyle DefaultStyle { get; private set; }
        public AfterCursor AfterCursor { get; private set; }

        internal Masking(RichTable table, Masking parent, Cursor start, AfterCursor afterCursor, RichStyle defaultStyle)
        {
            Table = table;
            Parent = parent;
            Cursor = start;
            AfterCursor = afterCursor;
            DefaultStyle = defaultStyle;
            Start = End = start;
        }

        public void Paint(Layout layout)
        {
            var afterCursor = layout.LeftToRight ? AfterCursor.AsideTopRight : AfterCursor.UnderBottomLeft;
            var style = layout.Style == RichStyle.Default ? DefaultStyle : layout.Style;

            Cursor GetNextCursor()
            {
                switch (afterCursor)
                {
                    case AfterCursor.AsideTopRight: return (Cursor.Row, Cursor.Col + 1);
                    case AfterCursor.UnderBottomLeft: return (Cursor.Row + 1, Cursor.Col);
                    default: throw new NotImplementedException();
                }
            }

            var singelCells = new List<Cursor>();
            int? mergeTo = null;
            Cursor? manualMergeFrom = null;

            foreach (var obj in layout.Objects)
            {
                if (manualMergeFrom is not null && obj is Layout.CellSpan)
                {
                    //TODO: optimizable
                    Table.UndoMerge(manualMergeFrom.Value);
                    Table[manualMergeFrom.Value, Cursor].Merge();
                    Cursor = GetNextCursor();
                    continue;
                }

                if (obj is Layout subLayout)
                {
                    var subMasking = new Masking(Table, this, Cursor, afterCursor, style);
                    subMasking.Paint(subLayout);
                    ExtendBound(subMasking.Start, subMasking.End);
                    manualMergeFrom = null;
                    Cursor = subMasking.GetAfterCursor();

                    switch (afterCursor)
                    {
                        case AfterCursor.AsideTopRight: mergeTo = subMasking.End.Row; break;
                        case AfterCursor.UnderBottomLeft: mergeTo = subMasking.End.Col; break;
                    }
                }
                else
                {
                    Table[Cursor].Value = obj;
                    Table[Cursor].Style = style;
                    ExtendBound(Cursor);
                    singelCells.Add(Cursor);
                    manualMergeFrom = Cursor;
                    Cursor = GetNextCursor();
                }
            }

            if (mergeTo is not null)
            {
                foreach (var cursor in singelCells)
                {
                    switch (afterCursor)
                    {
                        case AfterCursor.AsideTopRight: Table[cursor, (mergeTo.Value, cursor.Col)].Merge(); break;
                        case AfterCursor.UnderBottomLeft: Table[cursor, (cursor.Row, mergeTo.Value)].Merge(); break;
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
            Start = (new[] { Start.Row, cursor.Row }.Min(), new[] { Start.Col, cursor.Col }.Min());
            End = (new[] { End.Row, cursor.Row }.Max(), new[] { End.Col, cursor.Col }.Max());
        }

        private void ExtendBound(Cursor start, Cursor end)
        {
            Start = (new[] { Start.Row, start.Row }.Min(), new[] { Start.Col, start.Col }.Min());
            End = (new[] { End.Row, end.Row }.Max(), new[] { End.Col, end.Col }.Max());
        }
    }
}
