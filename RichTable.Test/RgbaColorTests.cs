using Richx;
using Xunit;

namespace RichTable.Test
{
    public class RgbaColorTests
    {
        [Fact]
        public void Test1()
        {
            var color = new RgbaColor { Value = 0x11223380 };
            Assert.Equal(0x11, color.Red);
            Assert.Equal(0x22, color.Green);
            Assert.Equal(0x33, color.Blue);
            Assert.Equal(0x80, color.Alpha);
            Assert.Equal(0x11223380u, color.Value);
        }

        [Fact]
        public void Test2()
        {
            var color = new RgbaColor { Red = 0x11, Green = 0x22, Blue = 0x33, Alpha = 0x80 };
            Assert.Equal(0x11223380u, color.Value);
        }

    }
}
