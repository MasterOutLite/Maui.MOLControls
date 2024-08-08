#if ANDROID


#elif IOS
#endif

namespace Maui.MOLControls;

#if ANDROID
public partial class AutoCompleteViewHandler
{
    public static IPropertyMapper<IAutoCompleteView, AutoCompleteViewHandler> PropertyMapper =
        new PropertyMapper<IAutoCompleteView, AutoCompleteViewHandler>(ViewMapper)
        {
            [nameof(IAutoCompleteView.Text)] = MapText,
            [nameof(IAutoCompleteView.TextColor)] = MapTextColor,
            [nameof(IAutoCompleteView.Placeholder)] = MapPlaceholder,
            [nameof(IAutoCompleteView.PlaceholderColor)] = MapPlaceholderColor,
            [nameof(IAutoCompleteView.TextMemberPath)] = MapTextMemberPath,
            [nameof(IAutoCompleteView.DisplayMemberPath)] = MapDisplayMemberPath,
            [nameof(IAutoCompleteView.IsEnabled)] = MapIsEnabled,
            [nameof(IAutoCompleteView.ItemsSource)] = MapItemsSource,
            [nameof(IAutoCompleteView.UpdateTextOnSelect)] = MapUpdateTextOnSelect,
            [nameof(IAutoCompleteView.IsSuggestionListOpen)] = MapIsSuggestionListOpen,
            [nameof(IAutoCompleteView.Threshold)] = MapThreshold,

            [nameof(IAutoCompleteView.FontFamily)] = MapFontFamily,
            [nameof(IAutoCompleteView.FontSize)] = MapFontSize,
            [nameof(IAutoCompleteView.HorizontalTextAlignment)] = MapHorizontalTextAlignment,
        };

    public static CommandMapper<IAutoCompleteView, AutoCompleteViewHandler> CommandMapper =
        new(ViewCommandMapper);

    public AutoCompleteViewHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper,
        commandMapper ?? CommandMapper)
    {
    }

    public AutoCompleteViewHandler() : base(PropertyMapper, CommandMapper)
    {
    }
}
#endif