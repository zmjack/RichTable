using System.Collections.Generic;

namespace Richx
{
    public class RichRow
    {
        public RichTable Table { get; private set; }
        public int Index { get; internal set; }
        private readonly Dictionary<int, RichCell> _innerCells = new();

        internal RichRow(RichTable table, int index)
        {
            Table = table;
            Index = index;
        }

        public IEnumerable<RichCell> Cells
        {
            get
            {
                var maxIndex = Table.MaxColumnIndex;
                for (var i = 0; i <= maxIndex; i++) yield return Cell(i);
            }
        }

        public RichCell this[int index] => Cell(index);

        public RichCell Cell(int index)
        {
            if (_innerCells.ContainsKey(index)) return _innerCells[index];
            else
            {
                var cell = new RichCell(this, index);
                _innerCells.Add(index, cell);
                if (Table.MaxColumnIndex < index) Table.MaxColumnIndex = index;
                return cell;
            }
        }

    }
}
