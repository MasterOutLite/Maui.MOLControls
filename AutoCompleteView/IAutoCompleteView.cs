using System.Collections;
using Maui.MOLControls.Events;

namespace Maui.MOLControls;

public interface IAutoCompleteView : IView
{
    public TextAlignment HorizontalTextAlignment { get; set; }
    public string FontFamily { get; set; }
    public float FontSize { get; set; }

    string Text { get; set; }
    Color TextColor { get; set; }
    string Placeholder { get; set; }
    Color PlaceholderColor { get; set; }
    string TextMemberPath { get; set; }
    string DisplayMemberPath { get; set; }
    bool IsSuggestionListOpen { get; set; }
    bool UpdateTextOnSelect { get; set; }
    IList ItemsSource { get; set; }
    int Threshold { get; set; }
    bool AllowCopyPaste { get; set; }

    void RaiseSuggestionChosen(ChosenElementEvent e);

    void NativeControlTextChanged(TextChangedEvent e);
}