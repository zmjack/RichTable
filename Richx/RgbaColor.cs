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
        [FieldOffset(0)] private int _value;

        public byte Alpha { get => _alpha; set => _alpha = value; }
        public byte Blue { get => _blue; set => _blue = value; }
        public byte Green { get => _green; set => _green = value; }
        public byte Red { get => _red; set => _red = value; }
        public uint ArgbValue => (uint)((Alpha << 24) + (Red << 16) + (Green << 8) + Blue);
        public uint RgbValue => (uint)((Red << 16) + (Green << 8) + Blue);

        public uint Value
        {
            get => (uint)_value;
            set => _value = (int)value;
        }

        public RgbaColor(uint value)
        {
            Value = value;
        }
        public RgbaColor(byte red, byte green, byte blue, byte alpha)
        {
            _red = red;
            _green = green;
            _blue = blue;
            _alpha = alpha;
        }
        public static RgbaColor FromArgb(uint argbValue)
        {
            unchecked
            {
                return new RgbaColor((0x00FFFFFF & argbValue) << 8 | (0xFF000000 & argbValue) >> 24);
            }
        }

        public static implicit operator RgbaColor(uint value)
        {
            return new RgbaColor(value);
        }

        public static RgbaColor Transparent = new(0x000000FF);

        public static bool operator ==(RgbaColor left, IArgbColor right) => left.ArgbValue == right.ArgbValue;
        public static bool operator !=(RgbaColor left, IArgbColor right) => left.ArgbValue != right.ArgbValue;

        public override int GetHashCode() => _value;
        public override bool Equals(object obj)
        {
            if (obj is IArgbColor other) return ArgbValue == other.ArgbValue;
            return false;
        }
    }

}
