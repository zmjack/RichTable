using System.Runtime.InteropServices;

namespace Richx
{
    [StructLayout(LayoutKind.Explicit)]
    public struct RgbaColor
    {
        [FieldOffset(0)] public byte Alpha;
        [FieldOffset(1)] public byte Blue;
        [FieldOffset(2)] public byte Green;
        [FieldOffset(3)] public byte Red;

        [FieldOffset(0)] private uint _value;
        public uint Value { get => _value; set => _value = value; }

        public static RgbaColor Create(uint value) => new RgbaColor { Value = value };
        public static RgbaColor Create(byte red, byte green, byte blue) => new RgbaColor { Red = red, Green = green, Blue = blue };
        public static RgbaColor Create(byte red, byte green, byte blue, byte alpha) => new RgbaColor { Red = red, Green = green, Blue = blue, Alpha = alpha };

        public static bool operator ==(RgbaColor left, RgbaColor right) => left.Value == right.Value;
        public static bool operator !=(RgbaColor left, RgbaColor right) => left.Value != right.Value;
    }

}
