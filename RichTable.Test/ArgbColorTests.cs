using Xunit;

namespace Richx.Test
{
    public class ArgbColorTests
    {
        [Fact]
        public void FromArgbTest()
        {
            var color = ArgbColor.FromArgb(0x80112233);
            Assert.Equal(0x11, color.Red);
            Assert.Equal(0x22, color.Green);
            Assert.Equal(0x33, color.Blue);
            Assert.Equal(0x80, color.Alpha);
            Assert.Equal(0x80112233u, color.Value);
            Assert.Equal(0x80112233u, color.ArgbValue);
        }

        [Fact]
        public void ValueTest1()
        {
            var color = ArgbColor.Create(0x80112233);
            Assert.Equal(0x11, color.Red);
            Assert.Equal(0x22, color.Green);
            Assert.Equal(0x33, color.Blue);
            Assert.Equal(0x80, color.Alpha);
            Assert.Equal(0x80112233u, color.Value);
            Assert.Equal(0x80112233u, color.ArgbValue);
        }

        [Fact]
        public void ValueTest2()
        {
            var color = ArgbColor.Create(0x80, 0x11, 0x22, 0x33);
            Assert.Equal(0x11, color.Red);
            Assert.Equal(0x22, color.Green);
            Assert.Equal(0x33, color.Blue);
            Assert.Equal(0x80, color.Alpha);
            Assert.Equal(0x80112233u, color.Value);
            Assert.Equal(0x80112233u, color.ArgbValue);
        }

        [Fact]
        public void AssignmentTest()
        {
            var color1 = new ArgbColor { Red = 0x11, Green = 0x22, Blue = 0x33, Alpha = 0x80 };
            Assert.Equal(0x80112233u, color1.ArgbValue);

            var color2 = color1;
            Assert.Equal(color1, color2);
            Assert.False(ReferenceEquals(color1, color2));
        }

    }
}
