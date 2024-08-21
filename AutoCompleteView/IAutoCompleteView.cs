using System.Collections;
using Maui.MOLControls.Events;

namespace Maui.MOLControls;

public interface IAutoCompleteView : IView
{
    public TextAlignment HorizontalTextAlignment { get; set; }
    public string FontFamily { get; set; }
    public float FontSize { get; set; }
    public string DropDownListIcon { get; set; }
    public int IconListHeight { get; set; }
    public int IconListVerticalMargin { get; set; }
    public Color ListBackground { get; set; }
    public Color ListTextColor { get; set; }
    public Color DividerColor { get; set; }

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