namespace Richx
{
    public interface IArgbColor
    {
        byte Alpha { get; }
        byte Red { get; set; }
        byte Green { get; set; }
        byte Blue { get; set; }
        uint ArgbValue { get; }
        uint RgbValue { get; }
        uint Value { get; }
    }
}
