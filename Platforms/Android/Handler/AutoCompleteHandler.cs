using Android.Graphics;
using Maui.FreakyControls;
using static Microsoft.Maui.ApplicationModel.Platform;

namespace Maui.MOLControls;

public partial class AutoCompleteHandler : FreakyAutoCompleteViewHandler
{
    public static void MapHorizontalTextAlignment(AutoCompleteHandler handler, IAutoComplete autoComplete)
    {
        handler.PlatformView.TextAlignment = autoComplete.HorizontalTextAlignment switch
        {
            TextAlignment.Start => Android.Views.TextAlignment.TextStart,
            TextAlignment.Center => Android.Views.TextAlignment.Center,
            TextAlignment.End => Android.Views.TextAlignment.TextEnd,
            _ => Android.Views.TextAlignment.TextStart
        };
    }

    public static void MapFontFamily(AutoCompleteHandler handler, IAutoComplete autoComplete)
    {
        if (Microsoft.Maui.ApplicationModel.Platform.CurrentActivity?.Assets != null)
        {
            Typeface? face = Typeface.Default;

            if (!string.IsNullOrWhiteSpace(autoComplete.FontFamily))
            {
                face = Typeface.CreateFromAsset(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.Assets,
                    $"{autoComplete.FontFamily}.ttf");

                // face = Typeface.Create(autoComplete.FontFamily, TypefaceStyle.Normal);
                // var create = Typeface.Create(autoComplete.FontFamily, TypefaceStyle.Normal);
            }

            handler.PlatformView.Typeface = face;
        }
    }

    public static void MapFontSize(AutoCompleteHandler handler, IAutoComplete autoComplete)
    {
        handler.PlatformView.TextSize = autoComplete.FontSize;
    }
}