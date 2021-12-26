namespace Richx
{
    public record RichStyle : IContentStyle, IValueStyle
    {
        public static readonly RichStyle Default = new();

        public RichStyle() { }

        public bool IsHeadCell { get; set; }

        public IArgbColor BackgroundColor { get; set; }
        public IArgbColor Color { get; set; }
        public int? FontSize { get; set; }
        public string FontFamily { get; set; }
        public bool? Bold { get; set; }
        public RichTextAlignment TextAlign { get; set; }
        public RichVerticalAlignment VerticalAlign { get; set; }

        public bool? BorderTop { get; set; }
        public bool? BorderBottom { get; set; }
        public bool? BorderLeft { get; set; }
        public bool? BorderRight { get; set; }

        public string Format { get; set; }
    }
}
