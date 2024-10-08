using Maui.MOLControls.Helpers;
using Maui.MOLControls.Models;

namespace Maui.MOLControls;

public class AdaptiveText : Label
{
    public List<AdaptiveSettings<object>> AdaptiveFontSize
    {
        get => (List<AdaptiveSettings<object>>)GetValue(AdaptiveFontSizeProperty);
        set => SetValue(AdaptiveFontSizeProperty, value);
    }

    public static readonly BindableProperty AdaptiveFontSizeProperty =
        BindableProperty.Create(nameof(AdaptiveFontSize),
            typeof(List<AdaptiveSettings<object>>),
            typeof(AdaptiveText),
            new List<AdaptiveSettings<object>>(),
            validateValue: CheckHelper.IsAssignableType([typeof(int), typeof(double), typeof(float)])
            // propertyChanging: CheckAssignableType([typeof(int), typeof(double)])
        );

    public List<AdaptiveSettings<object>> AdaptiveHeight
    {
        get => (List<AdaptiveSettings<object>>)GetValue(AdaptiveHeightProperty);
        set => SetValue(AdaptiveHeightProperty, value);
    }

    public static readonly BindableProperty AdaptiveHeightProperty =
        BindableProperty.Create(nameof(AdaptiveHeight),
            typeof(List<AdaptiveSettings<object>>),
            typeof(AdaptiveText),
            new List<AdaptiveSettings<object>>(),
            validateValue: CheckHelper.IsAssignableType([typeof(int), typeof(double), typeof(float)])
            // propertyChanging: CheckAssignableType([typeof(int), typeof(double)])
        );

    public bool RelativeToDisplay
    {
        get => (bool)GetValue(RelativeToDisplayProperty);
        set => SetValue(RelativeToDisplayProperty, value);
    }

    public static readonly BindableProperty RelativeToDisplayProperty =
        BindableProperty.Create(nameof(RelativeToDisplay),
            typeof(bool),
            typeof(AdaptiveText),
            false);

    private readonly List<AdaptiveSettingsSet<object>> _set = [];
    private bool _isSizeAllocated = false;

    protected void ApplyWithReplacement(AdaptiveSettingsSet<object> set)
    {
        var findIndex = _set.FindIndex(v => v.SetField == set.SetField);
        if (findIndex != -1)
        {
            _set[findIndex] = set;
        }
        else
        {
            _set.Add(set);
        }
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        switch (propertyName)
        {
            case nameof(AdaptiveFontSize):
                ApplyWithReplacement(new(AdaptiveFontSize, typeof(double), nameof(FontSize)));
                OnSizeAllocated();
                break;
            case nameof(AdaptiveHeight):
                ApplyWithReplacement(new(AdaptiveHeight, typeof(double), nameof(HeightRequest)));
                OnSizeAllocated();
                break;
        }
    }

    protected void CreateAdaptiveSetting()
    {
        _set.Add(new AdaptiveSettingsSet<object>(AdaptiveFontSize, typeof(double), nameof(FontSize)));
    }

    protected void OnSizeAllocated()
    {
        if (!_isSizeAllocated)
        {
            return;
        }

        var relativeWidth = RelativeToDisplay ? DeviceDisplay.MainDisplayInfo.Width : Width;

        SetAdaptiveProperties(relativeWidth);
    }

    protected void SetAdaptiveProperties(double width)
    {
        foreach (var set in _set)
        {
            if (!set.SetValue.Any())
            {
                continue;
            }

            var foundItem = set.SetValue
                .Where(x => x.SizeKey <= width).MaxBy(x => x.SizeKey);

            if (foundItem == null)
            {
                continue;
            }

            SetPropertyValue(set.SetField, foundItem.SetValue);
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        var relativeWidth = RelativeToDisplay ? DeviceDisplay.MainDisplayInfo.Width : width;

        CreateAdaptiveSetting();

        SetAdaptiveProperties(relativeWidth);

        _isSizeAllocated = true;
    }

    public void SetPropertyValue(string propertyName, object value)
    {
        var type = this.GetType();

        var property = type.GetProperty(propertyName);

        if (property != null && property.CanWrite)
        {
            property.SetValue(this, value);
        }
        else
        {
            throw new Exception($"Property '{propertyName}' not found or not writable.");
        }
    }
}