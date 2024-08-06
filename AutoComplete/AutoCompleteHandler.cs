using Maui.FreakyControls;

#if ANDROID
using PlatformView = Maui.FreakyControls.Platforms.Android.NativeControls.FreakyNativeAutoCompleteView;
#elif IOS
using PlatformView = Maui.FreakyControls.Platforms.iOS.NativeControls.FreakyNativeAutoCompleteView;
#endif
using Microsoft.Maui.Handlers;

namespace Maui.MOLControls;

#if ANDROID //|| IOS
public partial class AutoCompleteHandler : FreakyAutoCompleteViewHandler
{
    public static IPropertyMapper<IFreakyAutoCompleteView, FreakyAutoCompleteViewHandler> PropertyMapperNew =
        new PropertyMapper<IFreakyAutoCompleteView, FreakyAutoCompleteViewHandler>(ViewMapper)
        {
            [nameof(IAutoComplete.HorizontalTextAlignment)] = (handler, view) =>
                ExecuteForAutoComplete(handler, view, MapHorizontalTextAlignment),
            [nameof(IAutoComplete.FontFamily)] =
                (handler, view) => ExecuteForAutoComplete(handler, view, MapFontFamily),
            [nameof(IAutoComplete.FontSize)] = (handler, view) => ExecuteForAutoComplete(handler, view, MapFontSize)
        };

    private static IPropertyMapper<IFreakyAutoCompleteView, FreakyAutoCompleteViewHandler> CombineMappers(
        IPropertyMapper<IFreakyAutoCompleteView, FreakyAutoCompleteViewHandler> oldMapper,
        IPropertyMapper<IFreakyAutoCompleteView, FreakyAutoCompleteViewHandler> newMapper)
    {
        var combinedMapper = new PropertyMapper<IFreakyAutoCompleteView, FreakyAutoCompleteViewHandler>(ViewMapper);

        // Додавання властивостей з oldMapper
        foreach (var key in oldMapper.GetKeys())
        {
            var property = oldMapper.GetProperty(key);
            if (property != null)
            {
                combinedMapper[key] = property;
            }
        }

        // Додавання властивостей з newMapper (перезаписує властивості, якщо вони існують в oldMapper)
        foreach (var key in newMapper.GetKeys())
        {
            var property = newMapper.GetProperty(key);
            if (property != null)
            {
                combinedMapper[key] = property;
            }
        }

        return combinedMapper;
    }

    public AutoCompleteHandler() : base(CombineMappers(PropertyMapperNew, PropertyMapper), CommandMapper)
    {
    }

    private static void ExecuteForAutoComplete(
        FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view,
        Action<AutoCompleteHandler, IAutoComplete> action)
    {
        if (handler is AutoCompleteHandler newHandler && view is IAutoComplete newView)
        {
            action.Invoke(newHandler, newView);
        }
    }
}
#endif