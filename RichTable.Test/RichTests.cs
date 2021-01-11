using NStandard;
using Xunit;

namespace Richx.Test
{
    public class RichTests
    {
        [Fact]
        public void Test1()
        {
            var table = new RichTable();
            table[(0, 0)].Then(cell => { cell.Value = "00"; });
            table[(0, 1)].Then(cell => { cell.Value = "01"; });
            table[(0, 2)].Then(cell => { cell.Value = "02"; });
            table[(0, 3)].Then(cell => { cell.Value = "03"; });
            table[(0, 4)].Then(cell => { cell.Value = "04"; });

            table[(1, 0)].Then(cell => { cell.Value = "10"; });
            table[(1, 1)].Then(cell => { cell.Value = "11"; });
            table[(1, 2)].Then(cell => { cell.Value = "12"; });
            table[(1, 3)].Then(cell => { cell.Value = "13"; });
            table[(1, 4)].Then(cell => { cell.Value = "14"; });

            table[(2, 0)].Then(cell => { cell.Value = "20"; });
            table[(2, 1)].Then(cell => { cell.Value = "11"; });
            table[(2, 2)].Then(cell => { cell.Value = "22"; });
            table[(2, 3)].Then(cell => { cell.Value = "23"; });
            table[(2, 4)].Then(cell => { cell.Value = "24"; });

            table[(0, 0), (1, 0)].Merge();
            table[(1, 1), (2, 4)].SmartMerge(0, 1, 2, 3);

            var brush = table.BeginBrush((5, 0));
            brush.PrintLine(new object[] { 1, 2, null, "", 5, 6 });
            foreach (var cell in brush.Area.Cells)
            {
                cell.Style = cell.Style with
                {
                    BackgroundColor = 0x00ffff,
                };
            }

            foreach (var cell in table.Cells)
            {
                cell.Style = cell.Style with
                {
                    BorderTop = true,
                    BorderBottom = true,
                    BorderLeft = true,
                    BorderRight = true,

                    TextAlign = RichTextAlignment.Center,
                };
            }

            var htmlTable = new HtmlTable(table);
            htmlTable.Style += "width:100%";
            var html = htmlTable.ToHtml();
        }

    }
}
