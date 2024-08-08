using Maui.MOLControls.Enums;
using Maui.MOLControls.Events;


namespace Maui.MOLControls;

public class AutoCompleteViewView : View, IAutoCompleteView
{
    private readonly WeakEventManager _querySubmittedEventManager = new();
    public readonly WeakEventManager _textChangedEventManager = new();
    private readonly WeakEventManager _suggestionChosenEventManager = new();

    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    public static readonly BindableProperty HorizontalTextAlignmentProperty =
        BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(AutoCompleteViewView),
            TextAlignment.Center, BindingMode.TwoWay);

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(AutoCompleteViewView),
            null);

    public float FontSize
    {
        get => (float)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(float), typeof(AutoCompleteViewView), defaultValue: 20f);

    private bool suppressTextChangedEvent;
    private readonly WeakEventManager querySubmittedEventManager = new();
    public readonly WeakEventManager textChangedEventManager = new();
    private readonly WeakEventManager suggestionChosenEventManager = new();

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(AutoCompleteViewView), "", BindingMode.OneWay,
            null,
            OnTextPropertyChanged);

    private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var box = (AutoCompleteViewView)bindable;
        if (!box.suppressTextChangedEvent)
            box.textChangedEventManager.HandleEvent(box, new TextChangedEvent("", TextChangeReason.ProgrammaticChange),
                nameof(TextChanged));
    }

    public int Threshold
    {
        get { return (int)GetValue(ThresholdProperty); }
        set { SetValue(ThresholdProperty, value); }
    }

    public static readonly BindableProperty ThresholdProperty =
        BindableProperty.Create(nameof(Threshold), typeof(int), typeof(AutoCompleteViewView), 1, BindingMode.OneWay,
            null,
            OnTextPropertyChanged);

    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AutoCompleteViewView), Colors.Gray,
            BindingMode.OneWay, null, null);

    public string Placeholder
    {
        get { return (string)GetValue(PlaceholderProperty); }
        set { SetValue(PlaceholderProperty, value); }
    }

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(AutoCompleteViewView), string.Empty,
            BindingMode.OneWay, null, null);

    public Color PlaceholderColor
    {
        get { return (Color)GetValue(PlaceholderColorProperty); }
        set { SetValue(PlaceholderColorProperty, value); }
    }

    public static readonly BindableProperty PlaceholderColorProperty =
        BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(AutoCompleteViewView), Colors.Gray,
            BindingMode.OneWay, null, null);

    public string TextMemberPath
    {
        get { return (string)GetValue(TextMemberPathProperty); }
        set { SetValue(TextMemberPathProperty, value); }
    }

    public static readonly BindableProperty TextMemberPathProperty =
        BindableProperty.Create(nameof(TextMemberPath), typeof(string), typeof(AutoCompleteViewView), string.Empty,
            BindingMode.OneWay, null, null);

    public string DisplayMemberPath
    {
        get { return (string)GetValue(DisplayMemberPathProperty); }
        set { SetValue(DisplayMemberPathProperty, value); }
    }

    public static readonly BindableProperty DisplayMemberPathProperty =
        BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(AutoCompleteViewView), string.Empty,
            BindingMode.OneWay, null, null);

    public bool IsSuggestionListOpen
    {
        get { return (bool)GetValue(IsSuggestionListOpenProperty); }
        set { SetValue(IsSuggestionListOpenProperty, value); }
    }

    public static readonly BindableProperty IsSuggestionListOpenProperty =
        BindableProperty.Create(nameof(IsSuggestionListOpen), typeof(bool), typeof(AutoCompleteViewView), false,
            BindingMode.OneWay, null, null);

    public bool UpdateTextOnSelect
    {
        get { return (bool)GetValue(UpdateTextOnSelectProperty); }
        set { SetValue(UpdateTextOnSelectProperty, value); }
    }

    public static readonly BindableProperty UpdateTextOnSelectProperty =
        BindableProperty.Create(nameof(UpdateTextOnSelect), typeof(bool), typeof(AutoCompleteViewView), true,
            BindingMode.OneWay, null, null);

    public System.Collections.IList ItemsSource
    {
        get { return GetValue(ItemsSourceProperty) as System.Collections.IList; }
        set { SetValue(ItemsSourceProperty, value); }
    }

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.IList), typeof(AutoCompleteViewView),
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
        typeof(AutoCompleteViewView),
        false,
        BindingMode.OneWay,
        null,
        null);

    public event EventHandler<ChosenElementEvent> SuggestionChosen
    {
        add => suggestionChosenEventManager.AddEventHandler(value);
        remove => suggestionChosenEventManager.RemoveEventHandler(value);
    }

    public void RaiseSuggestionChosen(ChosenElementEvent args)
    {
        suggestionChosenEventManager.HandleEvent(this, args, nameof(SuggestionChosen));
    }

    public event EventHandler<TextChangedEvent> TextChanged
    {
        add => textChangedEventManager.AddEventHandler(value);
        remove => textChangedEventManager.RemoveEventHandler(value);
    }

    public void NativeControlTextChanged(TextChangedEvent args)
    {
        suppressTextChangedEvent = true;
        Text = args.Text;
        suppressTextChangedEvent = false;
        textChangedEventManager.HandleEvent(this, args, nameof(TextChanged));
    }
}