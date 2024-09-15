using Maui.MOLControls.Enums;
using Maui.MOLControls.Events;


namespace Maui.MOLControls;

public class AutoCompleteView : View, IAutoCompleteView
{
    private readonly WeakEventManager _querySubmittedEventManager = new();
    public readonly WeakEventManager _textChangedEventManager = new();
    private readonly WeakEventManager _suggestionChosenEventManager = new();

    private bool suppressTextChangedEvent;

    public string DropDownListIcon
    {
        get => (string)GetValue(DropDownListIconProperty);
        set => SetValue(DropDownListIconProperty, value);
    }

    public static readonly BindableProperty DropDownListIconProperty =
        BindableProperty.Create(nameof(DropDownListIcon),
            typeof(string),
            typeof(AutoCompleteView),
            string.Empty,
            BindingMode.TwoWay);

    public int IconListHeight
    {
        get => (int)GetValue(IconListHeightProperty);
        set => SetValue(VerticalOptionsProperty, value);
    }

    public static readonly BindableProperty IconListHeightProperty =
        BindableProperty.Create(nameof(IconListHeight),
            typeof(int),
            typeof(AutoCompleteView),
            -1,
            BindingMode.TwoWay);

    public int IconListWidth
    {
        get => (int)GetValue(IconListWidthProperty);
        set => SetValue(IconListWidthProperty, value);
    }

    public static readonly BindableProperty IconListWidthProperty =
        BindableProperty.Create(nameof(IconListWidth),
            typeof(int),
            typeof(AutoCompleteView),
            -1,
            BindingMode.TwoWay);

    public int IconListVerticalMargin
    {
        get => (int)GetValue(IconListVerticalMarginProperty);
        set => SetValue(IconListVerticalMarginProperty, value);
    }

    public static readonly BindableProperty IconListVerticalMarginProperty =
        BindableProperty.Create(nameof(IconListVerticalMargin),
            typeof(int),
            typeof(AutoCompleteView),
            0,
            BindingMode.TwoWay);

    public bool IsSingleLine
    {
        get => (bool)GetValue(IsSingleLineProperty);
        set => SetValue(IsSingleLineProperty, value);
    }

    public static readonly BindableProperty IsSingleLineProperty =
        BindableProperty.Create(nameof(IsSingleLine),
            typeof(bool),
            typeof(AutoCompleteView),
            true,
            BindingMode.TwoWay);

    public int ListTextLines
    {
        get => (int)GetValue(ListTextLinesProperty);
        set => SetValue(ListTextLinesProperty, value);
    }

    public static readonly BindableProperty ListTextLinesProperty =
        BindableProperty.Create(nameof(ListTextLines),
            typeof(int),
            typeof(AutoCompleteView),
            1,
            BindingMode.TwoWay);

    public int ListMinimumTextHeight
    {
        get => (int)GetValue(ListMinimumTextHeightProperty);
        set => SetValue(ListMinimumTextHeightProperty, value);
    }

    public static readonly BindableProperty ListMinimumTextHeightProperty =
        BindableProperty.Create(nameof(ListMinimumTextHeight),
            typeof(int),
            typeof(AutoCompleteView),
            60,
            BindingMode.TwoWay);

    public Color ListBackground
    {
        get => (Color)GetValue(ListBackgroundProperty);
        set => SetValue(ListBackgroundProperty, value);
    }

    public static readonly BindableProperty ListBackgroundProperty =
        BindableProperty.Create(nameof(ListBackground),
            typeof(Color),
            typeof(AutoCompleteView),
            Colors.White,
            BindingMode.TwoWay);

    public Color ListTextColor
    {
        get => (Color)GetValue(ListTextColorProperty);
        set => SetValue(ListTextColorProperty, value);
    }

    public static readonly BindableProperty ListTextColorProperty =
        BindableProperty.Create(nameof(ListTextColor),
            typeof(Color),
            typeof(AutoCompleteView),
            Colors.Black,
            BindingMode.TwoWay);

    public Color DividerColor
    {
        get => (Color)GetValue(DividerColorProperty);
        set => SetValue(DividerColorProperty, value);
    }

    public static readonly BindableProperty DividerColorProperty =
        BindableProperty.Create(nameof(DividerColor),
            typeof(Color),
            typeof(AutoCompleteView),
            Colors.Black,
            BindingMode.TwoWay);

    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    public static readonly BindableProperty HorizontalTextAlignmentProperty =
        BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(AutoCompleteView),
            TextAlignment.Center, BindingMode.TwoWay);

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(AutoCompleteView),
            null);

    public float FontSize
    {
        get => (float)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(float), typeof(AutoCompleteView), defaultValue: 20f);

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(AutoCompleteView), "", BindingMode.OneWay,
            null,
            OnTextPropertyChanged);

    private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var box = (AutoCompleteView)bindable;
        if (!box.suppressTextChangedEvent)
            box._textChangedEventManager.HandleEvent(box, new TextChangedEvent("", TextChangeReason.ProgrammaticChange),
                nameof(TextChanged));
    }

    public int Threshold
    {
        get { return (int)GetValue(ThresholdProperty); }
        set { SetValue(ThresholdProperty, value); }
    }

    public static readonly BindableProperty ThresholdProperty =
        BindableProperty.Create(nameof(Threshold), typeof(int), typeof(AutoCompleteView), 1, BindingMode.OneWay,
            null,
            OnTextPropertyChanged);

    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AutoCompleteView), Colors.Gray,
            BindingMode.OneWay, null, null);

    public string Placeholder
    {
        get { return (string)GetValue(PlaceholderProperty); }
        set { SetValue(PlaceholderProperty, value); }
    }

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(AutoCompleteView), string.Empty,
            BindingMode.OneWay, null, null);

    public Color PlaceholderColor
    {
        get { return (Color)GetValue(PlaceholderColorProperty); }
        set { SetValue(PlaceholderColorProperty, value); }
    }

    public static readonly BindableProperty PlaceholderColorProperty =
        BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(AutoCompleteView), Colors.Gray,
            BindingMode.OneWay, null, null);

    public string TextMemberPath
    {
        get { return (string)GetValue(TextMemberPathProperty); }
        set { SetValue(TextMemberPathProperty, value); }
    }

    public static readonly BindableProperty TextMemberPathProperty =
        BindableProperty.Create(nameof(TextMemberPath), typeof(string), typeof(AutoCompleteView), string.Empty,
            BindingMode.OneWay, null, null);

    public string DisplayMemberPath
    {
        get { return (string)GetValue(DisplayMemberPathProperty); }
        set { SetValue(DisplayMemberPathProperty, value); }
    }

    public static readonly BindableProperty DisplayMemberPathProperty =
        BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(AutoCompleteView), string.Empty,
            BindingMode.OneWay, null, null);

    public bool IsSuggestionListOpen
    {
        get { return (bool)GetValue(IsSuggestionListOpenProperty); }
        set { SetValue(IsSuggestionListOpenProperty, value); }
    }

    public static readonly BindableProperty IsSuggestionListOpenProperty =
        BindableProperty.Create(nameof(IsSuggestionListOpen), typeof(bool), typeof(AutoCompleteView), false,
            BindingMode.OneWay, null, null);

    public bool UpdateTextOnSelect
    {
        get { return (bool)GetValue(UpdateTextOnSelectProperty); }
        set { SetValue(UpdateTextOnSelectProperty, value); }
    }

    public static readonly BindableProperty UpdateTextOnSelectProperty =
        BindableProperty.Create(nameof(UpdateTextOnSelect), typeof(bool), typeof(AutoCompleteView), true,
            BindingMode.OneWay, null, null);

    public System.Collections.IList ItemsSource
    {
        get { return GetValue(ItemsSourceProperty) as System.Collections.IList; }
        set { SetValue(ItemsSourceProperty, value); }
    }

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.IList), typeof(AutoCompleteView),
            null,
            BindingMode.OneWay, null, null);

    public bool AllowCopyPaste
    {
        get => (bool)GetValue(AllowCopyPasteProperty);
        set => SetValue(AllowCopyPasteProperty, value);
    }

    public static readonly BindableProperty AllowCopyPasteProperty = BindableProperty.Create(
        nameof(AllowCopyPaste),
        typeof(bool),
        typeof(AutoCompleteView),
        false,
        BindingMode.OneWay,
        null,
        null);

    public event EventHandler<ChosenElementEvent> SuggestionChosen
    {
        add => _suggestionChosenEventManager.AddEventHandler(value);
        remove => _suggestionChosenEventManager.RemoveEventHandler(value);
    }

    public void RaiseSuggestionChosen(ChosenElementEvent args)
    {
        _suggestionChosenEventManager.HandleEvent(this, args, nameof(SuggestionChosen));
    }

    public event EventHandler<TextChangedEvent> TextChanged
    {
        add => _textChangedEventManager.AddEventHandler(value);
        remove => _textChangedEventManager.RemoveEventHandler(value);
    }

    public void NativeControlTextChanged(TextChangedEvent args)
    {
        suppressTextChangedEvent = true;
        Text = args.Text;
        suppressTextChangedEvent = false;
        _textChangedEventManager.HandleEvent(this, args, nameof(TextChanged));
    }
}