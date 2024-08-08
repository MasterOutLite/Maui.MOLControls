namespace Maui.MOLControls.Events;

public class ChosenElementEvent(object selectedElement) : EventArgs
{
    public object SelectedElement { get; } = selectedElement;
}