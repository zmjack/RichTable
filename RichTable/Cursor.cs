using System;
using System.Text.RegularExpressions;

namespace Richx
{
    public struct Cursor
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public Cursor(string cellName)
        {
            var regex = new Regex(@"^([A-Z]+)(\d+)$");
            var match = regex.Match(cellName);
            if (match.Success)
            {
                Row = int.Parse(match.Groups[2].Value) - 1;
                Col = LetterSequence.GetNumber(match.Groups[1].Value);
            }
            else throw new FormatException($"Illegal cell format：{cellName}。");
        }

        public Cursor(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public Cursor((int row, int col) cell)
        {
            Row = cell.row;
            Col = cell.col;
        }

        public static bool operator ==(Cursor left, Cursor right) => left.Row == right.Row && left.Col == right.Col;
        public static bool operator !=(Cursor left, Cursor right) => left.Row != right.Row || left.Col != right.Col;
        public override string ToString() => $"{LetterSequence.GetLetter(Col)}{Row + 1}, ({Row}, {Col})";

        public static implicit operator Cursor(string cellName) => new(cellName);
        public static implicit operator Cursor((int row, int col) cell) => new(cell);

        public override int GetHashCode()
        {
            return (Row << 16) | (Col & 0x0000FFFF);
        }

        public override bool Equals(object obj)
        {
            if (obj is Cursor other) return Row == other.Row && Col == other.Col;
            return false;
        }
    }
}
