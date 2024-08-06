#if ANDROID

using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using static Microsoft.Maui.ApplicationModel.Platform;
using NativeImage = Android.Graphics.Bitmap;

#endif

namespace Maui.MOLControls;

public static class DependencyInjection
{
    public static MauiAppBuilder AddMOLControls(this MauiAppBuilder app)
    {
        app.ConfigureMauiHandlers(AddHandlers);

        return app;
    }

    private static void AddHandlers(this IMauiHandlersCollection handlers)
    {
        handlers.AddHandler(typeof(AutoComplete), typeof(AutoCompleteHandler));
#if ANDROID
        handlers.AddHandler(typeof(AutoCompleteTextCenter), typeof(AutoCompleteTextCenterHandler));
#endif
    }
}