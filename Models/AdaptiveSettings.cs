namespace Maui.MOLControls.Models;

public record AdaptiveSettings<T>(double SizeKey, T SetValue);

public record AdaptiveSettingsDouble(double SizeKey, double SetValue) : AdaptiveSettings<double>(SizeKey, SetValue);

public record AdaptiveSettingsSet<T>(List<AdaptiveSettings<T>> SetValue, Type Type, string SetField);