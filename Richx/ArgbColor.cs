using System.Runtime.InteropServices;

namespace Richx
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ArgbColor : IArgbColor
    {
        [FieldOffset(0)] private byte _blue;
        [FieldOffset(1)] private byte _green;
        [FieldOffset(2)] private byte _red;
        [FieldOffset(3)] private byte _alpha;
        [FieldOffset(0)] private uint _value;

        public byte Alpha { get => _alpha; set => _alpha = value; }
        public byte Blue { get => _blue; set => _blue = value; }
        public byte Green { get => _green; set => _green = value; }
        public byte Red { get => _red; set => _red = value; }
        public uint ArgbValue => ((uint)Alpha << 24) + ((uint)Red << 16) + ((uint)Green << 8) + Blue;
        public uint RgbValue => ((uint)Red << 16) + ((uint)Green << 8) + Blue;

        public uint Value { get => _value; set => _value = value; }

        public static ArgbColor Create(uint value) => new ArgbColor { Value = value };
        public static ArgbColor Create(byte alpha, byte red, byte green, byte blue) => new ArgbColor { _red = red, _green = green, _blue = blue, _alpha = alpha };
        public static ArgbColor FromArgb(uint argbValue) => new ArgbColor { Value = argbValue };

        public static ArgbColor Transparent = new ArgbColor { Value = 0xff000000 };

        public static bool operator ==(ArgbColor left, IArgbColor right) => left.ArgbValue == right.ArgbValue;
        public static bool operator !=(ArgbColor left, IArgbColor right) => left.ArgbValue != right.ArgbValue;

        public override int GetHashCode() => (int)_value;
        public override bool Equals(object obj)
        {
            if (obj is IArgbColor other) return ArgbValue == other.ArgbValue;
            return false;
        }
    }

}
