using Maui.MOLControls.Enums;

namespace Maui.MOLControls;

public static class AnimationExtension
{
    public static async Task ButtonAnimation(this View view, ButtonAnimations animation)
    {
        switch (animation)
        {
            case ButtonAnimations.Scale:
                await view.ScaleTo(0.95, 100);
                await view.ScaleTo(1, 100);
                break;

            case ButtonAnimations.Fade:
                await view.FadeTo(0.7, 100);
                await view.FadeTo(1, 500);
                break;

            case ButtonAnimations.FadeAndScale:
                await Task.WhenAll(view.FadeTo(0.7, 100), view.ScaleTo(0.95, 100));
                await Task.WhenAll(view.FadeTo(1, 500), view.ScaleTo(1, 100));
                break;
        }
    }
}