using System.Runtime.InteropServices;

namespace Richx
{
    [StructLayout(LayoutKind.Explicit)]
    public struct RgbColor
    {
        [FieldOffset(0)] public byte Blue;
        [FieldOffset(1)] public byte Green;
        [FieldOffset(2)] public byte Red;

        [FieldOffset(0)] private uint _value;
        public uint Value { get => _value; set => _value = value & 0xffffff; }

        public static RgbColor Create(uint value) => new RgbColor { Value = value };
        public static RgbColor Create(byte red, byte green, byte blue) => new RgbColor { Red = red, Green = green, Blue = blue };

        public static bool operator ==(RgbColor left, RgbColor right) => left.Value == right.Value;
        public static bool operator !=(RgbColor left, RgbColor right) => left.Value != right.Value;

    }

}
