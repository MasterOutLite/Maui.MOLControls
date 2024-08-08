using Maui.FreakyControls;
using Maui.MOLControls.Events;

namespace Maui.MOLControls;

public class AutoComplete : FreakyAutoCompleteView, IAutoComplete
{
    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    public static readonly BindableProperty HorizontalTextAlignmentProperty =
        BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(AutoComplete),
            TextAlignment.Center, BindingMode.TwoWay);

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(AutoComplete), defaultValue: null);

    public float FontSize
    {
        get => (float)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public void RaiseSuggestionChosen(ChosenElementEvent e)
    {
        throw new NotImplementedException();
    }

    public void NativeControlTextChanged(TextChangedEvent e)
    {
        throw new NotImplementedException();
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(float), typeof(AutoComplete), defaultValue: 20f);
}