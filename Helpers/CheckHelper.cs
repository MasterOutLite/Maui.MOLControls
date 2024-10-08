using Maui.MOLControls.Models;

namespace Maui.MOLControls.Helpers;

public static class CheckHelper
{
    public static BindableProperty.ValidateValueDelegate IsAssignableType(List<Type> assignableType)
    {
        return (bindable, value) =>
        {
            if (value is List<AdaptiveSettings<object>> set)
            {
                foreach (var settings in set)
                {
                    var type = settings.SetValue.GetType();
                    var all = assignableType.All(v => !type.IsAssignableFrom(v));
                    if (all)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        };
    }


    public static BindableProperty.BindingPropertyChangingDelegate CheckAssignableType(List<Type> assignableType)
    {
        return (bindable, oldValue, newValue) =>
        {
            if (newValue is List<AdaptiveSettings<object>> set)
            {
                set.ForEach(settings =>
                {
                    var type = settings.SetValue.GetType();
                    var all = assignableType.All(v => !type.IsAssignableFrom(v));
                    if (all)
                    {
                        throw new InvalidOperationException($"Type for is bad");
                    }
                });
            }
        };
    }
}