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
            var layout = Layout.HorizontalAny("123", null, null, Layout.HorizontalAny("123", "123", null, null), Layout.HorizontalAny("a", "b"));
            var table = new Richx.RichTable();
            var masking = table.CreateMasking("A1", AfterCursor.Default, RichStyle.Default);
            masking.Paint(layout);
            var html = new HtmlTable(table).ToHtml();
        }

    }
}
