using System;
using System.Collections.Generic;
using System.Linq;

namespace Richx
{
    public class RichTable
    {
        private readonly Dictionary<int, RichRow> _innerRows = new();
        public int MaxRowIndex { get; internal set; } = -1;
        public int MaxColumnIndex { get; internal set; } = -1;

        public int RowLength => MaxRowIndex + 1;
        public int ColumnLength => MaxColumnIndex + 1;

        public IEnumerable<RichRow> Rows
        {
            get
            {
                var keys = _innerRows.Keys;
                var maxKey = keys.Any() ? keys.Max() : 0;
                for (var i = 0; i <= maxKey; i++) yield return Row(i);
            }
        }
        public IEnumerable<RichCell> Cells
        {
            get
            {
                foreach (var row in Rows)
                {
                    foreach (var cell in row.Cells) yield return cell;
                }
            }
        }

        public RichCell this[Cursor cursor] => Cell(cursor);
        public RichArea this[Cursor start, Cursor end] => Area(start, end);

        public RichArea Area(Cursor start, Cursor end) => new(this, start, end);

        public RichRow Row(int index)
        {
            if (_innerRows.ContainsKey(index)) return _innerRows[index];
            else
            {
                var row = new RichRow(this, index);
                _innerRows.Add(index, row);
                if (MaxRowIndex < index) MaxRowIndex = index;
                return row;
            }
        }

        public RichCell Cell(Cursor cursor) => Row(cursor.Row).Cell(cursor.Col);

        public Masking CreateMasking(Cursor cursor)
        {
            return new Masking(this, null, cursor, AfterCursor.Default, RichStyle.Default);
        }

        public Masking CreateMasking(Cursor cursor, AfterCursor afterCursor, RichStyle richStyle)
        {
            return new Masking(this, null, cursor, afterCursor, richStyle);
        }

        public RichBrush BeginBrush() => new(this, null, (0, 0));
        public RichBrush BeginBrush(RichStyle style) => new(this, null, (0, 0), style);
        public RichBrush BeginBrush(Cursor cursor) => new(this, null, cursor);
        public RichBrush BeginBrush(Cursor cursor, RichStyle style) => new(this, null, cursor, style);

        public void Merge(Cursor start, Cursor end)
        {
            if (start == end) throw new InvalidOperationException("Can not merge a single cell.");
            var rowSpan = end.Row - start.Row + 1;
            var colSpan = end.Col - start.Col + 1;
            var startCell = Cell(start);

            for (int row = start.Row; row <= end.Row; row++)
            {
                for (int col = start.Col; col <= end.Col; col++)
                {
                    var cell = Cell((row, col));
                    if (cell.Merged) continue;

                    cell.Ignored = true;
                    cell.RowSpan = rowSpan;
                    cell.ColSpan = colSpan;
                    cell.RowOffset = row - start.Row;
                    cell.ColOffset = col - start.Col;
                    cell.Style = startCell.Style;
                }
            }

            startCell.Ignored = false;
        }

        public void UndoMerge(Cursor cursor)
        {
            var cursorCell = Cell(cursor);
            if (!cursorCell.Merged) return;

            Cursor start = cursorCell.Ignored switch
            {
                false => cursor,
                true => (cursor.Row - cursorCell.RowOffset, cursor.Col - cursorCell.ColOffset),
            };
            Cursor end = cursorCell.Ignored switch
            {
                false => (cursor.Row + cursorCell.RowSpan - 1, cursor.Col + cursorCell.ColSpan - 1),
                true => (cursor.Row + cursorCell.RowSpan - 1 - cursorCell.RowOffset, cursor.Col + cursorCell.ColSpan - 1 - cursorCell.ColOffset),
            };

            for (int row = start.Row; row <= end.Row; row++)
            {
                for (int col = start.Col; col <= end.Col; col++)
                {
                    var cell = Cell((row, col));
                    cell.Ignored = false;
                    cell.RowSpan = 1;
                    cell.ColSpan = 1;
                    cell.RowOffset = 0;
                    cell.ColOffset = 0;
                    cell.Style = cursorCell.Style;
                }
            }
        }

    }
}
