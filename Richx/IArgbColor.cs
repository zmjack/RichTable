namespace Richx
{
    public interface IArgbColor
    {
        byte Alpha { get; }
        byte Blue { get; set; }
        byte Green { get; set; }
        byte Red { get; set; }
        uint Value { get; set; }
    }
}
