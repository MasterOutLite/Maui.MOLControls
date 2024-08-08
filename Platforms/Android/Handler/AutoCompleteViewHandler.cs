using Android.Content.Res;
using Android.Graphics;
using AndroidX.Core.View;
using Maui.MOLControls.Events;
using Maui.MOLControls.Platforms.Android.Extensions;
using Maui.MOLControls.Platforms.Android.Helpers;
using Maui.MOLControls.Platforms.Android.NativeControls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Maui.MOLControls;

public partial class AutoCompleteViewHandler : ViewHandler<IAutoCompleteView, AutoCompleteNativeView>
{
    protected override AutoCompleteNativeView CreatePlatformView()
    {
        var _nativeView = new AutoCompleteNativeView(this.Context);
        var colorStateList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        ViewCompat.SetBackgroundTintList(_nativeView, colorStateList);
        return _nativeView;
    }

    protected override void ConnectHandler(AutoCompleteNativeView platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SuggestionChosen += OnPlatformViewSuggestionChosen;
        platformView.TextChanged += OnPlatformViewTextChanged;
        platformView.SetTextColor(VirtualView?.TextColor.ToPlatform() ?? VirtualView.TextColor.ToPlatform());
        UpdateTextColor(platformView);
        UpdatePlaceholder(platformView);

        platformView.UpdateAdapterStyle(new SuggestCompleteAdapterStyle()
        {
            FontFamily = VirtualView.FontFamily
        });

        platformView.Click += PlatformViewOnClick;
    }

    protected override void DisconnectHandler(AutoCompleteNativeView platformView)
    {
        platformView.SuggestionChosen -= OnPlatformViewSuggestionChosen;
        platformView.TextChanged -= OnPlatformViewTextChanged;
        platformView.Click -= PlatformViewOnClick;

        platformView.Dispose();
        base.DisconnectHandler(platformView);
    }

    private void PlatformViewOnClick(object? sender, EventArgs e)
    {
        if (VirtualView.IsSuggestionListOpen)
        {
            VirtualView.IsSuggestionListOpen = false;
        }

        VirtualView.IsSuggestionListOpen = true;
    }

    private void OnPlatformViewSuggestionChosen(object? sender, ChosenElementEvent e)
    {
        VirtualView?.RaiseSuggestionChosen(e);
    }

    private void OnPlatformViewTextChanged(object? sender, TextChangedEvent e)
    {
        VirtualView?.NativeControlTextChanged(e);
    }

    public static void MapText(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        if (handler.PlatformView.Text != view.Text)
            handler.PlatformView.Text = view.Text;
    }

    public static void MapTextColor(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        handler.PlatformView?.SetTextColor(view.TextColor.ToPlatform());
    }

    public static void MapPlaceholder(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        handler.PlatformView.Placeholder = view.Placeholder;
    }

    public static void MapFontFamily(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        if (!string.IsNullOrWhiteSpace(view.FontFamily) &&
            FontFamilyAsset.TryGeTypeFace(view.FontFamily, out var face))
        {
            handler.PlatformView.Typeface = face;
        }
    }

    public static void MapFontSize(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        handler.PlatformView.TextSize = view.FontSize;
    }

    public static void MapHorizontalTextAlignment(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        handler.PlatformView.TextAlignment = view.HorizontalTextAlignment switch
        {
            TextAlignment.Start => Android.Views.TextAlignment.TextStart,
            TextAlignment.Center => Android.Views.TextAlignment.Center,
            TextAlignment.End => Android.Views.TextAlignment.TextEnd,
            _ => Android.Views.TextAlignment.TextStart
        };
    }

    public static void MapThreshold(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        handler.PlatformView.Threshold = view.Threshold;
    }

    public static void MapAllowCopyPaste(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        if (view.AllowCopyPaste)
        {
            handler.PlatformView.CustomInsertionActionModeCallback = null;
            handler.PlatformView.CustomSelectionActionModeCallback = null;
        }
        else
        {
            handler.PlatformView.CustomInsertionActionModeCallback = new CustomInsertionActionModeCallback();
            handler.PlatformView.CustomSelectionActionModeCallback = new CustomSelectionActionModeCallback();
        }
    }

    public static void MapPlaceholderColor(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        handler.PlatformView?.SetPlaceholderColor(view.PlaceholderColor);
    }

    public static void MapTextMemberPath(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        handler.PlatformView?.SetItems(view.ItemsSource?.OfType<object>(), (o) => FormatType(o, view.DisplayMemberPath),
            (o) => FormatType(o, view.TextMemberPath));
    }

    public static void MapDisplayMemberPath(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(),
            (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }

    public static void MapIsSuggestionListOpen(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        handler.PlatformView.IsSuggestionListOpen = view.IsSuggestionListOpen;
    }

    public static void MapUpdateTextOnSelect(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        handler.PlatformView.UpdateTextOnSelect = view.UpdateTextOnSelect;
    }

    public static void MapIsEnabled(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        handler.PlatformView.Enabled = view.IsEnabled;
    }

    public static void MapItemsSource(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(),
            (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }

    private void UpdateTextColor(AutoCompleteNativeView platformView)
    {
        var color = VirtualView?.TextColor;
        platformView.SetTextColor(color.ToPlatform());
    }

    private void UpdateDisplayMemberPath(AutoCompleteViewHandler handler, IAutoCompleteView view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(),
            (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }

    private void UpdatePlaceholderColor(AutoCompleteNativeView platformView)
    {
        var placeholderColor = VirtualView?.PlaceholderColor;
        platformView.SetPlaceholderColor(placeholderColor);
    }

    private void UpdatePlaceholder(AutoCompleteNativeView platformView) =>
        platformView.Placeholder = VirtualView?.Placeholder;

    private void UpdateIsEnabled(AutoCompleteNativeView platformView)
    {
        platformView.Enabled = (bool)(VirtualView?.IsEnabled);
    }

    private void UpdateItemsSource(AutoCompleteNativeView platformView)
    {
        platformView.SetItems(VirtualView?.ItemsSource?.OfType<object>(),
            (o) => FormatType(o, VirtualView?.DisplayMemberPath), (o) => FormatType(o, VirtualView?.TextMemberPath));
    }

    private static string FormatType(object instance, string memberPath)
    {
        if (!string.IsNullOrEmpty(memberPath))
            return instance?.GetType().GetProperty(memberPath)?.GetValue(instance)?.ToString() ?? "";
        else
            return instance?.ToString() ?? "";
    }
}