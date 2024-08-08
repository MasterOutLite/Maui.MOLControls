using Maui.MOLControls.Enums;

namespace Maui.MOLControls;

public class TextChangedEvent(string text, TextChangeReason reason) : EventArgs
{
    public string Text { get; } = text;
    public TextChangeReason Reason { get; } = reason;
}