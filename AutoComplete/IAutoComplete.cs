using Maui.FreakyControls;

namespace Maui.MOLControls;

public interface IAutoComplete : IFreakyAutoCompleteView
{
    public TextAlignment HorizontalTextAlignment { get; set; }
    public string FontFamily { get; set; }
    public float FontSize { get; set; }
}