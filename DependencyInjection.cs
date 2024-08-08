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
        handlers.AddHandler(typeof(AutoCompleteView), typeof(AutoCompleteViewHandler));
#if ANDROID
        handlers.AddHandler(typeof(AutoCompleteTextCenter), typeof(AutoCompleteTextCenterHandler));
#endif
    }
}