using Richx;

namespace RichTable.Test.Data
{
    public class Table
    {
        public class Model
        {
            public string Book { get; set; }
            public string Chapter1 { get; set; }
            public string Chapter2 { get; set; }
            public string Chapter3 { get; set; }
            public int Words { get; set; }
        }

        public class Style
        {
            public static readonly RichStyle Base = RichStyle.Default.Border(true) with
            {
                TextAlign = RichTextAlignment.Center,
                VerticalAlign = RichVerticalAlignment.Middle,
            };
            public static readonly RichStyle Title = Base with { BackgroundColor = new RgbColor(0xFFFF00) };
            public static readonly RichStyle Book = Base with { BackgroundColor = new RgbColor(0xDDEBF7) };
            public static readonly RichStyle Chapter1 = Base with { BackgroundColor = new RgbColor(0xFCE4D6) };
            public static readonly RichStyle Chapter2 = Base with { BackgroundColor = new RgbColor(0xEDEDED) };
            public static readonly RichStyle Chapter3 = Base with { BackgroundColor = new RgbColor(0xFFF2CC) };
            public static readonly RichStyle Words = Base with { Format = "#,##0" };
        }
    }
}
