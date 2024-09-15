using Color = Android.Graphics.Color;

namespace Maui.MOLControls.Platforms.Android.NativeControls.ArrayAdapter;

public class ArrayAdapterStyle
{
    public string FontFamily { get; set; } = string.Empty;
    public float FontSize { get; set; } = 18;
    public string Icon { get; set; } = string.Empty;
    public int IconListHeight { get; set; } = -1;
    public int IconListWidth { get; set; } = 90;
    public int IconListVerticalMargin { get; set; } = 0;
    public Color Background { get; set; } = Color.White;
    public Color TextColor { get; set; } = Color.Black;
    public Color DividerColor { get; set; } = Color.Black;

    public bool IsSingleLine { get; set; } = false;
    public int ListTextLines { get; set; } = 1;
    public int ListMinimumTextHeight { get; set; } = 160;
}