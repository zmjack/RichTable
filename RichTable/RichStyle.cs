using System;

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

        public string FormatValue(object value)
        {
            if (Format is not null)
            {
                return value switch
                {
                    short v => v.ToString(Format),
                    int v => v.ToString(Format),
                    long v => v.ToString(Format),
                    ushort v => v.ToString(Format),
                    uint v => v.ToString(Format),
                    ulong v => v.ToString(Format),
                    float v => v.ToString(Format),
                    double v => v.ToString(Format),
                    DateTime v => v.ToString(Format),
                    decimal v => v.ToString(Format),
                    _ => value?.ToString(),
                };
            }
            else return value?.ToString();
        }

        public RichStyle Border(bool? value)
        {
            BorderTop = value;
            BorderBottom = value;
            BorderLeft = value;
            BorderRight = value;
            return this;
        }

        public RichStyle Center(bool value)
        {
            if (value)
            {
                TextAlign = RichTextAlignment.Center;
                VerticalAlign = RichVerticalAlignment.Middle;
            }
            else
            {
                TextAlign = RichTextAlignment.Preserve;
                VerticalAlign = RichVerticalAlignment.Preserve;
            }
            return this;
        }

    }
}
