using Richx;
using Xunit;

namespace RichTable.Test
{
    public class RgbColorTests
    {
        [Fact]
        public void Test1()
        {
            var color = new RgbColor { Value = 0x80112233 };
            Assert.Equal(0x11, color.Red);
            Assert.Equal(0x22, color.Green);
            Assert.Equal(0x33, color.Blue);
            Assert.Equal(0x00112233u, color.Value);
        }

        [Fact]
        public void Test2()
        {
            var color = new RgbColor { Red = 0x11, Green = 0x22, Blue = 0x33 };
            Assert.Equal(0x00112233u, color.Value);
        }

    }
}
