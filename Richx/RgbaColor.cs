using System.Runtime.InteropServices;

namespace Richx
{
    [StructLayout(LayoutKind.Explicit)]
    public struct RgbaColor : IArgbColor
    {
        [FieldOffset(0)] private byte _alpha;
        [FieldOffset(1)] private byte _blue;
        [FieldOffset(2)] private byte _green;
        [FieldOffset(3)] private byte _red;
        [FieldOffset(0)] private uint _value;

        public byte Alpha { get => _alpha; set => _alpha = value; }
        public byte Blue { get => _blue; set => _blue = value; }
        public byte Green { get => _green; set => _green = value; }
        public byte Red { get => _red; set => _red = value; }
        public uint Value { get => _value; set => _value = value; }
        public uint ArgbValue => ((uint)Alpha << 24) + ((uint)Red << 16) + ((uint)Green << 8) + Blue;

        public static RgbaColor Create(uint value) => new RgbaColor { Value = value };
        public static RgbaColor Create(byte red, byte green, byte blue, byte alpha) => new RgbaColor { _red = red, _green = green, _blue = blue, _alpha = alpha };

        public static RgbaColor Transparent = new RgbaColor { Value = 0x000000ff };

        public static bool operator ==(RgbaColor left, IArgbColor right) => left.ArgbValue == right.ArgbValue;
        public static bool operator !=(RgbaColor left, IArgbColor right) => left.ArgbValue != right.ArgbValue;
    }

}
