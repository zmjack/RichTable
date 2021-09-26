using NStandard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Richx
{
    public class RichBrush : IDisposable
    {
        public RichBrush Parent { get; private set; }
        public RichTable Table { get; private set; }
        public RichArea Area { get; private set; }
        public Cursor Cursor;
        public RichStyle Style { get; private set; }

        private readonly int _startCol;
        private bool disposedValue;

        internal RichBrush(RichTable table, RichBrush parent, Cursor start)
        {
            Table = table;
            Parent = parent;
            Cursor = start;
            _startCol = start.Col;
            Area = new RichArea(table, Cursor, Cursor);
            Style = RichStyle.Default;
        }

        internal RichBrush(RichTable table, RichBrush parent, Cursor start, RichStyle style)
        {
            Table = table;
            Parent = parent;
            Cursor = start;
            _startCol = start.Col;
            Area = new RichArea(parent.Table, Cursor, Cursor);
            Style = style;
        }

        public void SetCursor(CursorPosition position)
        {
            Cursor = position switch
            {
                CursorPosition.Preserve => Cursor,
                CursorPosition.RowStart => new Cursor { Row = Area.Start.Row, Col = Cursor.Col },
                CursorPosition.RowEnd => new Cursor { Row = Area.End.Row, Col = Cursor.Col },
                CursorPosition.ColStart => new Cursor { Row = Cursor.Row, Col = Area.Start.Col },
                CursorPosition.ColEnd => new Cursor { Row = Cursor.Row, Col = Area.End.Col },
                CursorPosition.AreaTopLeft => Area.Start,
                CursorPosition.AreaTopRight => new Cursor { Row = Area.Start.Row, Col = Area.End.Col },
                CursorPosition.AreaBottomLeft => new Cursor { Row = Area.End.Row, Col = Area.Start.Col },
                CursorPosition.AreaBottomRight => Area.End,
                CursorPosition.AfterArea => new Cursor { Row = Area.End.Row + 1, Col = Area.Start.Col },
                CursorPosition.AsideArea => new Cursor { Row = Area.Start.Row, Col = Area.End.Col + 1 },
                _ => throw new NotImplementedException(),
            };
        }
        public void SetCursor(CursorPosition position, Cursor offset)
        {
            Cursor = position switch
            {
                CursorPosition.Preserve => new Cursor { Row = Cursor.Row + offset.Row, Col = Cursor.Col + offset.Col },
                CursorPosition.RowStart => new Cursor { Row = Area.Start.Row + offset.Row, Col = Cursor.Col + offset.Col },
                CursorPosition.RowEnd => new Cursor { Row = Area.End.Row + offset.Row, Col = Cursor.Col + offset.Col },
                CursorPosition.ColStart => new Cursor { Row = Cursor.Row + offset.Row, Col = Area.Start.Col + offset.Col },
                CursorPosition.ColEnd => new Cursor { Row = Cursor.Row + offset.Row, Col = Area.End.Col + offset.Col },
                CursorPosition.AreaTopLeft => new Cursor { Row = Area.Start.Row + offset.Row, Col = Area.Start.Col + offset.Col },
                CursorPosition.AreaTopRight => new Cursor { Row = Area.Start.Row + offset.Row, Col = Area.End.Col + offset.Col },
                CursorPosition.AreaBottomLeft => new Cursor { Row = Area.End.Row + offset.Row, Col = Area.Start.Col + offset.Col },
                CursorPosition.AreaBottomRight => new Cursor { Row = Area.End.Row + offset.Row, Col = Area.End.Col + offset.Col },
                CursorPosition.AfterArea => new Cursor { Row = Area.End.Row + 1 + offset.Row, Col = Area.Start.Col + offset.Col },
                CursorPosition.AsideArea => new Cursor { Row = Area.Start.Row + offset.Row, Col = Area.End.Col + 1 + offset.Col },
                _ => throw new NotImplementedException(),
            };
        }

        private void RecalculateArea(Cursor start, Cursor end) => Area.Update(start, end);

        public RichBrush Print(object[] values) => Print(PrintDirection.Horizontal, values);
        public RichBrush Print(object[][] values) => Print(PrintDirection.Horizontal, values);
        public RichBrush Print(PrintDirection direction, object[] values)
        {
            if (values is null) return this;
            return Print(direction, new object[][] { values });
        }
        public RichBrush Print(PrintDirection direction, object[][] values)
        {
            if (values is null) return this;

            var startRow = Cursor.Row;
            var startCol = Cursor.Col;
            var rowLength = values.Length;
            var colLength = values.Any() ? values.Max(values1 => values1.Length) : 0;
            var rangeStart = (startRow, startCol);

            Func<int, int, Cursor> getCursor = direction switch
            {
                PrintDirection.Horizontal => (row, col) => (startRow + row, startCol + col),
                PrintDirection.Vertical => (row, col) => (startRow + col, startCol + row),
                _ => throw new NotImplementedException(),
            };

            Cursor rangeEnd;
            for (int row = 0; row < rowLength; row++)
            {
                for (int col = 0; col < values[row].Length; col++)
                {
                    var valueObj = values[row][col];
                    var cell = Table[getCursor(row, col)];
                    cell.Style = Style;

                    if (valueObj is null) continue;
                    cell.Value = valueObj;
                }
            }

            if (direction == PrintDirection.Horizontal)
            {
                Cursor.Col = startCol + colLength;
                rangeEnd = (startRow + rowLength - 1, startCol + colLength - 1);
            }
            else if (direction == PrintDirection.Vertical)
            {
                Cursor.Col = startCol + rowLength;
                rangeEnd = (startRow + colLength - 1, startCol + rowLength - 1);
            }
            else throw new NotSupportedException();

            RecalculateArea(rangeStart, rangeEnd);
            return this;
        }
        public RichBrush Print<T>(T[] models, Func<T, object[]> select)
        {
            if (models is null) return this;
            return Print(PrintDirection.Horizontal, models, select);
        }
        public RichBrush Print<T>(PrintDirection direction, T[] models, Func<T, object[]> select)
        {
            if (models is null) return this;
            return Print(direction, models.Select(select).ToArray());
        }

        public RichBrush PrintLine(int lineHeight = 1)
        {
            if (lineHeight < 0) throw new ArgumentException($"The {nameof(lineHeight)} must be non-negative.", nameof(lineHeight));
            Cursor.Col = _startCol;
            Cursor.Row += lineHeight;
            return this;
        }
        public RichBrush PrintLine(object[] values) => PrintLine(PrintDirection.Horizontal, values);
        public RichBrush PrintLine(object[][] values) => PrintLine(PrintDirection.Horizontal, values);
        public RichBrush PrintLine(PrintDirection direction, object[] values)
        {
            if (values is null) return this;
            return PrintLine(direction, new object[][] { values });
        }
        public RichBrush PrintLine(PrintDirection direction, object[][] values)
        {
            if (values is null) return this;

            Print(direction, values);
            if (direction == PrintDirection.Horizontal) Cursor.Row += values.Length;
            else if (direction == PrintDirection.Vertical) Cursor.Row += values.Any() ? values.Max(innerValues => innerValues?.Length ?? 0) : 0;
            else throw new NotSupportedException();

            Cursor.Col = _startCol;
            return this;
        }
        public RichBrush PrintLine<T>(T[] models, Func<T, object[]> select)
        {
            if (models is null) return this;
            return PrintLine(PrintDirection.Horizontal, models, select);
        }
        public RichBrush PrintLine<T>(PrintDirection direction, T[] models, Func<T, object[]> select)
        {
            if (models is null) return this;

            var values = models.Select(select).ToArray();
            Print(direction, values);
            if (direction == PrintDirection.Horizontal) Cursor.Row += values.Length;
            else if (direction == PrintDirection.Vertical) Cursor.Row += values.Any() ? values.Max(innerValues => innerValues?.Length ?? 0) : 0;
            else throw new NotSupportedException();

            Cursor.Col = _startCol;
            return this;
        }

        public RichBrush PrintArea(object[,] values)
        {
            var startRow = Cursor.Row;
            var startCol = Cursor.Col;
            var rowLength = values.GetLength(0);
            var colLength = values.GetLength(1);

            for (var row = 0; row < rowLength; row++)
            {
                for (var col = 0; col < colLength; col++)
                {
                    var valueObj = values[row, col];
                    var cell = Table[(startRow + row, startCol + col)];
                    cell.Style = Style;

                    if (valueObj is null) continue;
                    cell.Value = valueObj;
                }
            }
            Cursor.Col = startCol + colLength;

            var rangeStart = (startRow, startCol);
            var rangeEnd = (startRow + rowLength - 1, startCol + colLength - 1);

            RecalculateArea(rangeStart, rangeEnd);
            return this;
        }
        public RichBrush PrintArea(DataTable table)
        {
            var range1 = PrintLine(table.Columns.Cast<DataColumn>().Select(a => a.ColumnName).ToArray());
            var range2 = PrintLine((from DataRow row in table.Select() select row.ItemArray.ToArray()).ToArray());
            return this;
        }

        public RichBrush PrintAreaLine(object[,] values)
        {
            PrintArea(values);
            Cursor.Row += values.GetLength(0);
            Cursor.Col = _startCol;
            return this;
        }
        public RichBrush PrintAreaLine(DataTable table)
        {
            PrintArea(table);
            Cursor.Row += table.Rows.Count;
            Cursor.Col = _startCol;
            return this;
        }

        public RichBrush BeginViceBrush() => new(Table, this, Cursor);
        public RichBrush BeginViceBrush(Cursor cursor) => new(Table, this, cursor);
        public RichBrush BeginViceBrush(RichStyle style) => new(Table, this, Cursor, style);
        public RichBrush BeginViceBrush(Cursor cursor, RichStyle style) => new(Table, this, cursor, style);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Parent is not null)
                    {
                        Parent.Area.Extend(Area);
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
