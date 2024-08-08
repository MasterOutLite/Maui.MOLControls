using Maui.FreakyControls.Shared.Enums;
using Maui.FreakyControls;
using System.Windows.Input;
using Maui.MOLControls.Events;

namespace Maui.MOLControls;

public class AutoCompleteView : View, IAutoComplete
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
        BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(AutoCompleteView),
            TextAlignment.Center, BindingMode.TwoWay);

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(AutoCompleteView), defaultValue: null);

    public float FontSize
    {
        get => (float)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(float), typeof(AutoCompleteView), defaultValue: 20f);

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
        BindableProperty.Create(nameof(Text), typeof(string), typeof(AutoCompleteView), "", BindingMode.OneWay, null,
            OnTextPropertyChanged);

    private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var box = (AutoCompleteView)bindable;
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
        BindableProperty.Create(nameof(Threshold), typeof(int), typeof(AutoCompleteView), 1, BindingMode.OneWay, null,
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
        BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.IList), typeof(AutoCompleteView), null,
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

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
        nameof(Image),
        typeof(ImageSource),
        typeof(AutoCompleteView),
        default(ImageSource));

    public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(
        nameof(ImageHeight),
        typeof(int),
        typeof(AutoCompleteView),
        25);

    public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(
        nameof(ImageWidth),
        typeof(int),
        typeof(AutoCompleteView),
        25);

    public static readonly BindableProperty ImageAlignmentProperty = BindableProperty.Create(
        nameof(ImageAlignment),
        typeof(ImageAlignment),
        typeof(AutoCompleteView),
        ImageAlignment.Right);

    public static readonly BindableProperty ImagePaddingProperty = BindableProperty.Create(
        nameof(ImagePadding),
        typeof(int),
        typeof(FreakyAutoCompleteView),
        5);

    public static readonly BindableProperty ImageCommandProperty = BindableProperty.Create(
        nameof(ImagePadding),
        typeof(ICommand),
        typeof(AutoCompleteView),
        default(ICommand));

    public static readonly BindableProperty ImageCommandParameterProperty = BindableProperty.Create(
        nameof(ImageCommandParameter),
        typeof(object),
        typeof(AutoCompleteView),
        default);

    public object ImageCommandParameter
    {
        get => GetValue(ImageCommandParameterProperty);
        set => SetValue(ImageCommandParameterProperty, value);
    }

    public ICommand ImageCommand
    {
        get => (ICommand)GetValue(ImageCommandProperty);
        set => SetValue(ImageCommandProperty, value);
    }

    public int ImagePadding
    {
        get => (int)GetValue(ImagePaddingProperty);
        set => SetValue(ImagePaddingProperty, value);
    }

    public int ImageWidth
    {
        get => (int)GetValue(ImageWidthProperty);
        set => SetValue(ImageWidthProperty, value);
    }

    public int ImageHeight
    {
        get => (int)GetValue(ImageHeightProperty);
        set => SetValue(ImageHeightProperty, value);
    }

    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public ImageAlignment ImageAlignment
    {
        get => (ImageAlignment)GetValue(ImageAlignmentProperty);
        set => SetValue(ImageAlignmentProperty, value);
    }

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

    public void RaiseQuerySubmitted(FreakyAutoCompleteViewQuerySubmittedEventArgs args)
    {
        querySubmittedEventManager.HandleEvent(this, args, nameof(QuerySubmitted));
    }

    public event EventHandler<FreakyAutoCompleteViewQuerySubmittedEventArgs> QuerySubmitted
    {
        add => querySubmittedEventManager.AddEventHandler(value);
        remove => querySubmittedEventManager.RemoveEventHandler(value);
    }
}