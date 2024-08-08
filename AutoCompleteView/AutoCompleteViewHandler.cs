#if ANDROID
using Maui.FreakyControls;
using PlatformView = Maui.FreakyControls.Platforms.Android.NativeControls.FreakyNativeAutoCompleteView;

#elif IOS
using PlatformView = Maui.FreakyControls.Platforms.iOS.NativeControls.FreakyNativeAutoCompleteView;
#endif

namespace Maui.MOLControls;

#if ANDROID
public partial class AutoCompleteViewHandler
{
    public static IPropertyMapper<IAutoComplete, AutoCompleteViewHandler> PropertyMapper =
        new PropertyMapper<IAutoComplete, AutoCompleteViewHandler>(ViewMapper)
        {
            [nameof(IAutoComplete.Text)] = MapText,
            [nameof(IAutoComplete.TextColor)] = MapTextColor,
            [nameof(IAutoComplete.Placeholder)] = MapPlaceholder,
            [nameof(IAutoComplete.PlaceholderColor)] = MapPlaceholderColor,
            [nameof(IAutoComplete.TextMemberPath)] = MapTextMemberPath,
            [nameof(IAutoComplete.DisplayMemberPath)] = MapDisplayMemberPath,
            [nameof(IAutoComplete.IsEnabled)] = MapIsEnabled,
            [nameof(IAutoComplete.ItemsSource)] = MapItemsSource,
            [nameof(IAutoComplete.UpdateTextOnSelect)] = MapUpdateTextOnSelect,
            [nameof(IAutoComplete.IsSuggestionListOpen)] = MapIsSuggestionListOpen,
            [nameof(IAutoComplete.Threshold)] = MapThreshold,

            [nameof(IAutoComplete.FontFamily)] = MapFontFamily,
            [nameof(IAutoComplete.FontSize)] = MapFontSize,
            [nameof(IAutoComplete.HorizontalTextAlignment)] = MapHorizontalTextAlignment,
        };

    public static CommandMapper<IAutoComplete, AutoCompleteViewHandler> CommandMapper =
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