using Xunit;

namespace Richx.Test
{
    public class RgbaColorTests
    {
        [Fact]
        public void FromArgbTest()
        {
            var color = RgbaColor.FromArgb(0x80112233);
            Assert.Equal(0x11, color.Red);
            Assert.Equal(0x22, color.Green);
            Assert.Equal(0x33, color.Blue);
            Assert.Equal(0x80, color.Alpha);
            Assert.Equal(0x11223380u, color.Value);
            Assert.Equal(0x80112233u, color.ArgbValue);
        }

        [Fact]
        public void ValueTest1()
        {
            var color = RgbaColor.Create(0x11223380);
            Assert.Equal(0x11, color.Red);
            Assert.Equal(0x22, color.Green);
            Assert.Equal(0x33, color.Blue);
            Assert.Equal(0x80, color.Alpha);
            Assert.Equal(0x11223380u, color.Value);
            Assert.Equal(0x80112233u, color.ArgbValue);
        }

        [Fact]
        public void ValueTest2()
        {
            var color = RgbaColor.Create(0x11, 0x22, 0x33, 0x80);
            Assert.Equal(0x11, color.Red);
            Assert.Equal(0x22, color.Green);
            Assert.Equal(0x33, color.Blue);
            Assert.Equal(0x80, color.Alpha);
            Assert.Equal(0x11223380u, color.Value);
            Assert.Equal(0x80112233u, color.ArgbValue);
        }

        [Fact]
        public void AssignmentTest()
        {
            var color1 = new RgbaColor { Red = 0x11, Green = 0x22, Blue = 0x33, Alpha = 0x80 };
            Assert.Equal(0x80112233u, color1.ArgbValue);

            var color2 = color1;
            Assert.Equal(color1, color2);
            Assert.False(ReferenceEquals(color1, color2));
        }

    }
}
