using System.ComponentModel;
using System.Windows.Input;
using Maui.MOLControls.Enums;

namespace Maui.MOLControls;

public partial class TouchableView : ContentView
{
    public TouchableView()
    {
        InitializeComponent();
    }

    public View Render
    {
        get => (View)GetValue(RenderProperty);
        set => SetValue(RenderProperty, value);
    }

    public static readonly BindableProperty RenderProperty =
        BindableProperty.Create(nameof(Render),
            typeof(View),
            typeof(TouchableView),
            null, BindingMode.TwoWay);

    public Color NativeAnimationColor
    {
        get => (Color)GetValue(NativeAnimationColorProperty);
        set => SetValue(NativeAnimationColorProperty, value);
    }

    public static readonly BindableProperty NativeAnimationColorProperty =
        BindableProperty.Create(nameof(NativeAnimationColor),
            typeof(Color),
            typeof(TouchableView),
            Colors.Transparent);

    public ButtonAnimations Animation
    {
        get => (ButtonAnimations)GetValue(AnimationProperty);
        set => SetValue(AnimationProperty, value);
    }

    public static readonly BindableProperty AnimationProperty =
        BindableProperty.Create(nameof(Animation),
            typeof(ButtonAnimations),
            typeof(TouchableView),
            defaultValue: ButtonAnimations.FadeAndScale);

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(nameof(BorderColor),
            typeof(Color),
            typeof(TouchableView),
            defaultValue: Colors.White);

    public double BorderWidth
    {
        get => (double)GetValue(BorderWidthProperty);
        set => SetValue(BorderWidthProperty, value);
    }

    public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create(nameof(BorderWidth),
            typeof(double),
            typeof(TouchableView),
            defaultValue: Button.BorderWidthProperty.DefaultValue);

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(TouchableView),
            new CornerRadius(10));

    public Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public new static readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(nameof(BackgroundColor),
            typeof(Color),
            typeof(TouchableView),
            defaultValue: Colors.Black);

    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    public new static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(nameof(Padding),
            typeof(Thickness),
            typeof(TouchableView),
            defaultValue: new Thickness(12, 0));

    public LayoutOptions HorizontalContentOptions
    {
        get => (LayoutOptions)GetValue(HorizontalContentOptionsProperty);
        set => SetValue(HorizontalContentOptionsProperty, value);
    }

    public new static readonly BindableProperty HorizontalContentOptionsProperty =
        BindableProperty.Create(nameof(Padding),
            typeof(LayoutOptions),
            typeof(TouchableView),
            defaultValue: LayoutOptions.Center);

    public FormattedString FormattedText
    {
        get => (FormattedString)GetValue(FormattedTextProperty);
        set => SetValue(FormattedTextProperty, value);
    }

    public new static readonly BindableProperty FormattedTextProperty =
        BindableProperty.Create(nameof(FormattedText),
            typeof(FormattedString),
            typeof(TouchableView),
            defaultValue: new FormattedString());


    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(TouchableView));

    public object CommandParameter
    {
        get => (object)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter),
            typeof(object),
            typeof(TouchableView),
            null,
            BindingMode.TwoWay);

    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (nameof(Render) == propertyName)
        {
            Container.Clear();
            Container.Add(Render);
        }
    }

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        switch (Animation)
        {
            case ButtonAnimations.Scale:
                await this.ScaleTo(0.95, 100);
                await this.ScaleTo(1, 100);
                break;

            case ButtonAnimations.Fade:
                await this.FadeTo(0.7, 100);
                await this.FadeTo(1, 500);
                break;

            case ButtonAnimations.FadeAndScale:
                await Task.WhenAll(this.FadeTo(0.7, 100), this.ScaleTo(0.95, 100));
                await Task.WhenAll(this.FadeTo(1, 500), this.ScaleTo(1, 100));
                break;
        }

        Command?.Execute(CommandParameter);
    }
}