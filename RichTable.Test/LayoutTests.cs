using Richx;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RichTable.Test
{
    public class LayoutTests
    {
        [Fact]
        public void Test1()
        {
            var style = new RichStyle
            {
                Format = "#,##0",
            };
            var layout =
                Layout.VerticalAny(
                    Layout.HorizontalAny(111, null, 333),
                    Layout.HorizontalAny(null, 222, null),
                    Layout.HorizontalAny("", "", 333),
                    Layout.HorizontalAny(1234, "2-3", Layout.Span, 4),
                    Layout.HorizontalAny("a", "b")
                ).WithStyle(style);
            var table = new Richx.RichTable();
            var masking = table.CreateMasking("A1", AfterCursor.Default, RichStyle.Default);
            masking.Paint(layout);
            var html = new HtmlTable(table, props: new()
            {
                ["border"] = "1",
            }).ToHtml();
        }

    }
}
