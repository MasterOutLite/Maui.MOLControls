using Android.Graphics;
using Maui.FreakyControls;
using Maui.FreakyControls.Platforms.Android.NativeControls;
using TextAlignment = Android.Views.TextAlignment;

namespace Maui.MOLControls;

public class AutoCompleteTextCenterHandler : FreakyAutoCompleteViewHandler
{
    protected override FreakyNativeAutoCompleteView CreatePlatformView()
    {
        var controls = base.CreatePlatformView();

        controls.TextAlignment = TextAlignment.Center;
        Typeface face = Typeface.CreateFromAsset(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.Assets,
            "Jura-Regular.ttf");

        controls.Typeface = face;
        controls.TextSize = 20;
        return controls;
    }
}