using System.Runtime.InteropServices;

namespace Richx
{
    [StructLayout(LayoutKind.Explicit)]
    public struct RgbColor : IArgbColor
    {
        [FieldOffset(0)] private byte _blue;
        [FieldOffset(1)] private byte _green;
        [FieldOffset(2)] private byte _red;
        [FieldOffset(0)] private int _value;

        public byte Alpha => byte.MaxValue;
        public byte Blue { get => _blue; set => _blue = value; }
        public byte Green { get => _green; set => _green = value; }
        public byte Red { get => _red; set => _red = value; }
        public uint ArgbValue => (uint)((Alpha << 24) + (Red << 16) + (Green << 8) + Blue);
        public uint RgbValue => (uint)((Red << 16) + (Green << 8) + Blue);

        public uint Value
        {
            get => (uint)_value;
            set => _value = (int)(value & 0x00FFFFFF);
        }

        public RgbColor(uint value)
        {
            Value = value;
        }
        public RgbColor(byte red, byte green, byte blue)
        {
            _red = red;
            _green = green;
            _blue = blue;
        }
        public static RgbColor FromArgb(uint argbValue) => new(argbValue);

        public static implicit operator RgbColor(uint value)
        {
            return new RgbColor(value);
        }

        public static bool operator ==(RgbColor left, IArgbColor right) => left.ArgbValue == right.ArgbValue;
        public static bool operator !=(RgbColor left, IArgbColor right) => left.ArgbValue != right.ArgbValue;

        public override int GetHashCode() => _value;
        public override bool Equals(object obj)
        {
            if (obj is IArgbColor other) return ArgbValue == other.ArgbValue;
            return false;
        }
    }

}
