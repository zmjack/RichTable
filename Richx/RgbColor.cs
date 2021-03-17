using System.Runtime.InteropServices;

namespace Richx
{
    [StructLayout(LayoutKind.Explicit)]
    public struct RgbColor : IArgbColor
    {
        [FieldOffset(0)] private byte _blue;
        [FieldOffset(1)] private byte _green;
        [FieldOffset(2)] private byte _red;
        [FieldOffset(0)] private uint _value;

        public byte Alpha => 0;
        public byte Blue { get => _blue; set => _blue = value; }
        public byte Green { get => _green; set => _green = value; }
        public byte Red { get => _red; set => _red = value; }
        public uint Value { get => _value; set => _value = value & 0xffffff; }
        public uint ArgbValue => ((uint)Alpha << 24) + ((uint)Red << 16) + ((uint)Green << 8) + Blue;

        public static RgbColor Create(uint value) => new RgbColor { Value = value };
        public static RgbColor Create(byte red, byte green, byte blue) => new RgbColor { _red = red, _green = green, _blue = blue };

        public static bool operator ==(RgbColor left, IArgbColor right) => left.ArgbValue == right.ArgbValue;
        public static bool operator !=(RgbColor left, IArgbColor right) => left.ArgbValue != right.ArgbValue;

    }

}
