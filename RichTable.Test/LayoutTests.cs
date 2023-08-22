using NStandard.Collections;
using RichTable.Test.Data;
using Richx;
using Xunit;

namespace RichTable.Test
{
    public class LayoutTests
    {
        [Fact]
        public void ConstTest()
        {
            var layout =
                Layout.Vertical(
                    Layout.HorizontalAny("Book", "Chapter", Layout.Spans(2), "Words").WithStyle(Table.Style.Title),
                    Layout.HorizontalAny("Book1",
                        Layout.Vertical(

                            Layout.Vertical(
                                Layout.HorizontalAny("A",
                                    Layout.Vertical(
                                        Layout.HorizontalAny("A-1",
                                            Layout.Vertical(
                                                Layout.HorizontalAny("A-1-1", Layout.Const(1011).WithStyle(Table.Style.Words)),
                                                Layout.HorizontalAny("A-1-2", Layout.Const(1012).WithStyle(Table.Style.Words))
                                            ).WithStyle(Table.Style.Chapter3)
                                        )
                                    ).WithStyle(Table.Style.Chapter2)
                                ),
                                Layout.HorizontalAny("Total Words", Layout.Span, Layout.Span, 4066)
                            ).WithStyle(Table.Style.Chapter1),

                            Layout.Vertical(
                                Layout.HorizontalAny("B",
                                    Layout.Vertical(
                                        Layout.HorizontalAny("B-1",
                                            Layout.Vertical(
                                                Layout.HorizontalAny("B-1-1", Layout.Const(2011).WithStyle(Table.Style.Words)),
                                                Layout.HorizontalAny("B-1-2", Layout.Const(2012).WithStyle(Table.Style.Words))
                                            ).WithStyle(Table.Style.Chapter3)
                                        )
                                    ).WithStyle(Table.Style.Chapter2)
                                ),
                                Layout.HorizontalAny("Total Words", Layout.Spans(2), 4066)
                            ).WithStyle(Table.Style.Chapter1),

                            Layout.HorizontalAny("12132 Words", Layout.Spans(3))
                        )
                    ).WithStyle(Table.Style.Book)
                ).WithStyle(Table.Style.Base);

            var table = new Richx.RichTable();
            var masking = table.CreateMasking("A2", AfterCursor.Default, RichStyle.Default);
            masking.Paint(layout);
            var html = new HtmlTable(table, props: new()
            {
                ["border"] = "1",
            }).ToHtml();
        }

        [Fact]
        public void ConstTest2()
        {
            var layout =
                new Layout.Vert(Table.Style.Base)
                {
                    new Layout.Hori(Table.Style.Title) { "Book", "Chapter", Layout.Spans(2), "Words" },
                    new Layout.Hori(Table.Style.Book)
                    {
                        "Book1",
                        new Layout.Vert
                        {
                            new Layout.Vert(Table.Style.Chapter1)
                            {
                                new Layout.Hori
                                {
                                    "A",
                                    new Layout.Vert(Table.Style.Chapter2)
                                    {
                                        new Layout.Hori
                                        {
                                            "A-1",
                                            new Layout.Vert(Table.Style.Chapter3)
                                            {
                                                new Layout.Hori(Table.Style.Words) { "A-1-1", Layout.Const(1011) },
                                                new Layout.Hori(Table.Style.Words) { "A-1-2", Layout.Const(1012) },
                                            }
                                        }
                                    }
                                },
                                new Layout.Hori { "Total Words", Layout.Span, Layout.Span, 4066 }
                            },

                            new Layout.Vert(Table.Style.Chapter1)
                            {
                                new Layout.Hori
                                {
                                    "B",
                                    new Layout.Vert(Table.Style.Chapter2)
                                    {
                                        new Layout.Hori
                                        {
                                            "B-1",
                                            new Layout.Vert(Table.Style.Chapter3)
                                            {
                                                new Layout.Hori(Table.Style.Words) { "B-1-1", Layout.Const(2011) },
                                                new Layout.Hori(Table.Style.Words) { "B-1-2", Layout.Const(2012) }
                                            }
                                        }
                                    }
                                },
                                new Layout.Hori { "Total Words", Layout.Spans(2), 4066 }
                            },

                            new Layout.Hori { "12132 Words", Layout.Spans(3) }
                        }
                    }
                };

            var table = new Richx.RichTable();
            var masking = table.CreateMasking("A2", AfterCursor.Default, RichStyle.Default);
            masking.Paint(layout);
            var html = new HtmlTable(table, props: new()
            {
                ["border"] = "1",
            }).ToHtml();
        }

        [Fact]
        public void MergeTest()
        {
            var layout =
                Layout.VerticalAny(
                    "Center",
                    Layout.Const("Center").WithStyle(Table.Style.Title),
                    Layout.HorizontalAny(1, 2, 3, 4, 5),
                    Layout.HorizontalAny("0 Words", Layout.Span, Layout.HorizontalAny(3, 4, 5)),
                    Layout.HorizontalAny(Layout.HorizontalAny("0 Words", Layout.Span), Layout.HorizontalAny(3, 4, 5)),
                    Layout.Vertical(new[] { 1 })
                );

            var table = new Richx.RichTable();
            var masking = table.CreateMasking("A2", AfterCursor.Default, RichStyle.Default);
            masking.Paint(layout);
            var html = new HtmlTable(table, props: new()
            {
                ["border"] = "1",
            }).ToHtml();
        }

    }
}
