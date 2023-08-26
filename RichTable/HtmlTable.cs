using NStandard;
using NStandard.Flows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Richx
{
    public class HtmlTable
    {
        public RichTable Table { get; }
        public string Class { get; set; }
        public string PropsString { get; set; }
        public string StyleString { get; set; }

        public static readonly Dictionary<string, string> PropsDefault = new();
        public static readonly Dictionary<string, string> StyleDefault = new()
        {
            ["border-collapse"] = "collapse",
        };

        public HtmlTable(RichTable table, string @class = "table", Dictionary<string, string> props = null, Dictionary<string, string> style = null)
        {
            props ??= new();
            var sb_props = new StringBuilder();
            if (!props.ContainsKey("width")) props.Add("width", "100%");
            foreach (var pair in props) sb_props.Append($" {pair.Key.Pipe(StringFlow.HtmlEncode)}=\"{pair.Value.Pipe(StringFlow.HtmlEncode)}\"");

            style ??= new();
            var sb_style = new StringBuilder();
            if (!style.ContainsKey("border-collapse")) style.Add("border-collapse", "collapse");
            foreach (var pair in style) sb_style.Append($"{pair.Key.Pipe(StringFlow.HtmlEncode)}:{pair.Value.Pipe(StringFlow.HtmlEncode)};");

            Table = table;
            Class = @class;
            PropsString = sb_props.ToString();
            StyleString = sb_style.ToString();
        }

        private static string Props(params string[] props)
        {
            return props.Where(prop => !prop.IsNullOrWhiteSpace()).Select(prop => $" {prop}").Join("");
        }
        private static string RowSpanProp(RichCell cell) => cell.RowSpan.Pipe(span => span > 1 ? $@"rowspan=""{span}""" : "");
        private static string ColSpanProp(RichCell cell) => cell.ColSpan.Pipe(span => span > 1 ? $@"colspan=""{span}""" : "");

        private static string StyleProp(RichCell cell)
        {
            if (cell.Ignored) return "";

            var dict = new Dictionary<string, string>();
            var style = cell.Style;

            if (style is null) return string.Empty;

            if (style.BackgroundColor is not null) dict.Add("background-color", $"#{style.BackgroundColor.Red:x2}{style.BackgroundColor.Green:x2}{style.BackgroundColor.Blue:x2}{(style.BackgroundColor.Alpha < byte.MaxValue ? $"{style.BackgroundColor.Alpha:x2}" : "")}");
            if (style.Color is not null) dict.Add("color", $"#{style.Color.Red:x2}{style.Color.Green:x2}{style.Color.Blue:x2}{(style.Color.Alpha < byte.MaxValue ? $"{style.Color.Alpha}:x2" : "")}");
            if (style.FontFamily is not null) dict.Add("font-family", $"{style.FontFamily}");
            if (style.FontSize.HasValue) dict.Add("font-size", $"{style.FontSize}px");
            if (style.Bold.HasValue && style.Bold.Value) dict.Add("font-weight", $"bold");
            if (style.BorderTop.HasValue && style.BorderTop.Value) dict.Add("border-top", $"1px solid #000000");
            if (style.BorderBottom.HasValue && style.BorderBottom.Value) dict.Add("border-bottom", $"1px solid #000000");
            if (style.BorderLeft.HasValue && style.BorderLeft.Value) dict.Add("border-left", $"1px solid #000000");
            if (style.BorderRight.HasValue && style.BorderRight.Value) dict.Add("border-right", $"1px solid #000000");
            if (style.TextAlign != RichTextAlignment.Preserve) dict.Add("text-align", $"{style.TextAlign.ToString().ToLower()}");
            if (style.VerticalAlign != RichVerticalAlignment.Preserve) dict.Add("vertical-align", $"{style.VerticalAlign.ToString().ToLower()}");

            return $@"style=""{dict.Select(pair => $"{pair.Key}:{pair.Value}").Join(";")}""";
        }

        private string GetContentHtml(string str)
        {
            return str?.NormalizeNewLine().Replace(" ", "\u00A0").Pipe(StringFlow.HtmlEncode).Replace(Environment.NewLine, "<br/>");
        }

        public string ToHtml()
        {
            var sb = new StringBuilder();
            sb.Append("<table");
            if (!Class.IsNullOrWhiteSpace()) sb.Append($@" class=""{Class}""");
            if (!PropsString.IsNullOrWhiteSpace()) sb.Append(PropsString);
            if (!StyleString.IsNullOrWhiteSpace()) sb.Append($@" style=""{StyleString}""");
            sb.AppendLine(">");

            sb.AppendLine(@"<tbody>");

            foreach (var row in Table.Rows)
            {
                sb.AppendLine(@"<tr>");
                foreach (var cell in row.Cells.Where(x => !x.Ignored))
                {
                    //TODO: Auto calculate width
                    var cellWidth = cell.Comment.GetPureLines().Pipe(lines =>
                    {
                        if (lines.Any()) return 10 + lines.Max(x => (cell.Style.FontSize ?? 12) * x.GetLengthA());
                        else return 10;
                    });

                    sb.AppendLine($@"<td{Props(RowSpanProp(cell), ColSpanProp(cell), StyleProp(cell))}>");
                    if (!cell.Comment.IsNullOrWhiteSpace())
                    {
                        sb.AppendLine($@"
<span class=""excel-sharp"">
    <span class=""excel-text"">{GetContentHtml(cell.Text)}</span>
    <div></div>
    <div class=""excel-comment"" style=""width:{cellWidth}px"">
        {GetContentHtml(cell.Comment)}
    </div>
</span>
");
                    }
                    else sb.AppendLine(GetContentHtml(cell.Text));
                    sb.AppendLine("</td>");
                }
                sb.AppendLine(@"</tr>");
            }

            sb.AppendLine(@"</tbody>");
            sb.AppendLine(@"</table>");

            return sb.ToString();
        }

    }
}
