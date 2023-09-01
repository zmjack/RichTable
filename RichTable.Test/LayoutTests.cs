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
                new Layout.Vert(Table.Style.Base)
                {
                    new Layout.Hori(Table.Style.Title)
                    {
                        "Book",
                        new Layout.Span(3) { "Chapter" },
                        new Layout.Span(2) { "Words" },
                    },
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
                                                new Layout.Hori(Table.Style.Chapter1, Table.Style.Chapter2)
                                                {
                                                    "A-1-1",
                                                    new Layout.Hori { 1011, 1012 },
                                                },
                                                new Layout.Hori(Table.Style.Words)
                                                {
                                                    "A-1-2",
                                                    new Layout.Hori { 1011, 1012 },
                                                },
                                            }
                                        }
                                    }
                                },
                                new Layout.Hori
                                {
                                    new Layout.Span(3) { "Total Words" },
                                    2022,
                                    2024,
                                }
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
                                                new Layout.Hori(Table.Style.Words)
                                                {
                                                    "B-1-1",
                                                    new Layout.Hori { 1011, null },
                                                },
                                                new Layout.Hori(Table.Style.Words)
                                                {
                                                    "B-1-2",
                                                    new Layout.Hori { 1011, 1012 } ,
                                                }
                                            }
                                        }
                                    }
                                },
                                new Layout.Hori
                                {
                                    new Layout.Span(3)
                                    {
                                        "Total Words"
                                    },
                                    2022,
                                    1012,
                                }
                            },

                            new Layout.Hori
                            {
                                new Layout.Span(3) { "Total Words" },
                                4044,
                                3036,
                            }
                        }
                    }
                };

            var table = new Richx.RichTable();
            var masking = table.CreateMasking("A2");
            masking.Paint(layout);
            var html = new HtmlTable(table, props: new()
            {
                ["border"] = "1",
            }).ToHtml();
        }

        [Fact]
        public void HoriMergeTest()
        {
            var table = new Richx.RichTable();
            var masking = table.CreateMasking("A2");
            masking.Paint(new Layout.Vert
            {
                "Center",
                //new Layout.Cell(Table.Style.Title) { "Center" },
                new Layout.Hori { 1, 2, 3, 4, 5 },
                new Layout.Hori
                {
                    new Layout.Span(2) { "0 Words" },
                    new Layout.Hori { 3, 4, 5 },
                },
                new Layout.Hori
                {
                    new Layout.Hori
                    {
                        new Layout.Span(2) { "0 Words" },
                        new Layout.Hori { 3, 4, 5 },
                    }
                },
                new Layout.Vert
                {
                    1
                }
            });
            var html = new HtmlTable(table, props: new()
            {
                ["border"] = "1",
            }).ToHtml();
        }

        [Fact]
        public void VertMergeTest()
        {
            var table = new Richx.RichTable();
            var masking = table.CreateMasking("A2");
            masking.Paint(new Layout.Hori
            {
                "Center",
                //new Layout.Cell(Table.Style.Title) { "Center" },
                new Layout.Vert { 1, 2, 3, 4, 5 },
                new Layout.Vert
                {
                    new Layout.Span(2) { "0 Words" },
                    new Layout.Vert { 3, 4, 5 },
                },
                new Layout.Vert
                {
                    new Layout.Vert
                    {
                        new Layout.Span(2) { "0 Words" },
                        new Layout.Vert { 3, 4, 5 },
                    }
                },
                new Layout.Hori
                {
                    1
                }
            });
            var html = new HtmlTable(table, props: new()
            {
                ["border"] = "1",
            }).ToHtml();
        }

    }
}
