using Android.Graphics;

namespace Maui.MOLControls.Platforms.Android.Extensions;

public partial class FontFamilyAsset
{
    private static string GetFontPath(string fontFamily) => $"{fontFamily}.ttf";

    public static bool FontExists(string? fontName)
    {
        if (string.IsNullOrWhiteSpace(fontName))
        {
            return false;
        }

        var assetManager = Platform.CurrentActivity?.Assets;
        var assets = assetManager?.List("");
        return assets?.Contains(GetFontPath(fontName)) == true;
    }

    public static Typeface? GeTypeFace(string? fontName)
    {
        Typeface? face = Typeface.Default;
        if (FontExists(fontName))
        {
            face = Typeface.CreateFromAsset(
                Platform.CurrentActivity?.Assets,
                GetFontPath(fontName!));
        }

        return face;
    }

    public static bool TryGeTypeFace(string? fontName, out Typeface? typeface)
    {
        var isSuccess = FontExists(fontName);
        typeface = Typeface.Default;
        if (isSuccess)
        {
            typeface = Typeface.CreateFromAsset(
                Platform.CurrentActivity?.Assets,
                GetFontPath(fontName!));
        }

        return isSuccess;
    }
}