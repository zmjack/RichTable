namespace Richx
{
    public class RichCell
    {
        public RichRow Row { get; private set; }
        public int Index { get; private set; }

        public int RowIndex => Row.Index;
        public RichStyle Style { get; set; } = RichStyle.Default;
        public object _innerValue;

        internal RichCell(RichRow row, int index)
        {
            Row = row;
            Index = index;
        }

        public bool Merged
        {
            get
            {
                if (Ignored) return true;
                else if (RowSpan > 1 || ColSpan > 1) return true;
                else return false;
            }
        }
        public bool Ignored { get; set; }
        public int RowSpan { get; set; } = 1;
        public int RowOffset { get; set; }
        public int ColSpan { get; set; } = 1;
        public int ColOffset { get; set; }
        public string Format => Style.Format;

        public string Comment { get; set; }
        public object Value
        {
            get => _innerValue;
            set
            {
                if (value is RichValue richValue)
                {
                    _innerValue = richValue.Value;
                    Comment = richValue.Comment;
                    if (richValue.Style is not null) Style = richValue.Style;
                }
                else _innerValue = value;
            }
        }
        public string Text => Style?.FormatValue(Value) ?? Value?.ToString();

    }
}
