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
        handlers.AddHandler(typeof(AutoCompleteViewView), typeof(AutoCompleteViewHandler));
    }
}