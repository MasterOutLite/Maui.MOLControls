using System.ComponentModel;
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
        BindableProperty.Create(nameof(Animation), typeof(ButtonAnimations), typeof(TouchableView),
            defaultValue: ButtonAnimations.FadeAndScale);

    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (nameof(Content) == propertyName)
        {
            var con = Content;
        }

        if (nameof(Render) == propertyName)
        {
            var conRen = Render;
            var con = Content;
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
                await Task.WhenAll(this.FadeTo(0.7, 100));
                await Task.WhenAll(this.FadeTo(1, 500));
                break;
        }
    }
}