using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Java.Lang;
using Maui.MOLControls.Enums;
using Maui.MOLControls.Events;
using Maui.MOLControls.Platforms.Android.NativeControls.ArrayAdapter;
using Microsoft.Maui.Platform;
using Color = Microsoft.Maui.Graphics.Color;
using Rect = Android.Graphics.Rect;
using View = Android.Views.View;


namespace Maui.MOLControls.Platforms.Android.NativeControls;

public class AutoCompleteNativeView : AppCompatAutoCompleteTextView
{
    protected bool suppressTextChangedEvent;
    protected Func<object, string> textFunc;
    protected SimpleArrayAdapter adapter;
    protected Drawable drawableRight;
    protected Drawable drawableLeft;
    protected Drawable drawableTop;
    protected Drawable drawableBottom;
    protected int actionX, actionY;

    public AutoCompleteNativeView(Context context) : base(context)
    {
        SetMaxLines(1);
        InputType = global::Android.Text.InputTypes.TextFlagNoSuggestions |
                    global::Android.Text.InputTypes
                        .TextVariationVisiblePassword; //Disables text suggestions as the auto-complete view is there to do that
        ItemClick += OnItemClick;

        Adapter = adapter = new SimpleArrayAdapter(Context, global::Android.Resource.Layout.SimpleDropDownItem1Line);
        // Adapter = adapter =
        //     new AutoCompleteArrayAdapter(Context, global::Android.Resource.Layout.SimpleDropDownItem1Line);
    }

    public override bool EnoughToFilter() => true;

    public void SetItems(IEnumerable<object> items, Func<object, string> labelFunc, Func<object, string> textFunc)
    {
        this.textFunc = textFunc;
        if (items is null)
            adapter.UpdateList(Enumerable.Empty<string>(), labelFunc);
        else
            adapter.UpdateList(items.OfType<object>(), labelFunc);
    }

    public virtual new string Text
    {
        get => base.Text;
        set
        {
            suppressTextChangedEvent = true;
            base.Text = value;
            suppressTextChangedEvent = false;
            this.TextChanged?.Invoke(this,
                new TextChangedEvent(value, TextChangeReason.ProgrammaticChange));
        }
    }

    public virtual void SetTextColor(Color color)
    {
        this.SetTextColor(color.ToPlatform());
    }

    public virtual string Placeholder
    {
        set => HintFormatted = new Java.Lang.String(value as string ?? "");
    }

    public virtual void SetPlaceholderColor(Color color)
    {
        this.SetHintTextColor(color.ToPlatform());
    }

    public virtual bool IsSuggestionListOpen
    {
        set
        {
            if (value)
                ShowDropDown();
            else
                DismissDropDown();
        }
    }

    public virtual bool UpdateTextOnSelect { get; set; } = true;

    protected override void OnTextChanged(ICharSequence text, int start, int lengthBefore, int lengthAfter)
    {
        if (!suppressTextChangedEvent)
            this.TextChanged?.Invoke(this,
                new TextChangedEvent(text.ToString(), TextChangeReason.UserInput));
        base.OnTextChanged(text, start, lengthBefore, lengthAfter);
    }

    protected void DismissKeyboard()
    {
        var imm = (InputMethodManager)Context.GetSystemService(Context.InputMethodService);
        imm.HideSoftInputFromWindow(WindowToken, 0);
    }

    protected void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
    {
        DismissKeyboard();
        var obj = adapter.GetObject(e.Position);
        if (UpdateTextOnSelect)
        {
            suppressTextChangedEvent = true;
            string text = textFunc(obj);
            base.Text = text;
            suppressTextChangedEvent = false;
            TextChanged?.Invoke(this,
                new TextChangedEvent(text, TextChangeReason.SuggestionChosen));
        }

        SuggestionChosen?.Invoke(this, new ChosenElementEvent(obj));
    }

    public override void OnEditorAction([GeneratedEnum] ImeAction actionCode)
    {
        if (actionCode == ImeAction.Done || actionCode == ImeAction.Next)
        {
            DismissDropDown();
            DismissKeyboard();
        }
        else
            base.OnEditorAction(actionCode);
    }

    protected override void ReplaceText(ICharSequence text)
    {
        //Override to avoid updating textbox on itemclick. We'll do this later using TextMemberPath and raise the proper TextChanged event then
    }

    public new event EventHandler<TextChangedEvent> TextChanged;

    public event EventHandler<ChosenElementEvent> SuggestionChosen;

    public override void SetCompoundDrawablesWithIntrinsicBounds(Drawable left, Drawable top,
        Drawable right, Drawable bottom)
    {
        if (left is not null)
        {
            drawableLeft = left;
        }

        if (right is not null)
        {
            drawableRight = right;
        }

        if (top is not null)
        {
            drawableTop = top;
        }

        if (bottom is not null)
        {
            drawableBottom = bottom;
        }

        base.SetCompoundDrawablesWithIntrinsicBounds(left, top, right, bottom);
    }

    public override bool OnTouchEvent(MotionEvent e)
    {
        Rect bounds;
        if (e.Action == MotionEventActions.Down)
        {
            actionX = (int)e.GetX();
            actionY = (int)e.GetY();
            if (drawableBottom is not null
                && drawableBottom.Bounds.Contains(actionX, actionY))
            {
                return base.OnTouchEvent(e);
            }

            if (drawableTop is not null
                && drawableTop.Bounds.Contains(actionX, actionY))
            {
                return base.OnTouchEvent(e);
            }

            // this works for left since container shares 0,0 origin with bounds
            if (drawableLeft is not null)
            {
                bounds = null;
                bounds = drawableLeft.Bounds;

                int x, y;
                int extraTapArea = (int)((13 * Resources.DisplayMetrics.Density) + 0.5);

                x = actionX;
                y = actionY;

                if (!bounds.Contains(actionX, actionY))
                {
                    // Gives the +20 area for tapping. /
                    x = (int)(actionX - extraTapArea);
                    y = (int)(actionY - extraTapArea);

                    if (x <= 0)
                        x = actionX;
                    if (y <= 0)
                        y = actionY;

                    // Creates square from the smallest value /
                    if (x < y)
                    {
                        y = x;
                    }
                }

                if (bounds.Contains(x, y))
                {
                    e.Action = (MotionEventActions.Cancel);
                    return false;
                }
            }

            if (drawableRight is not null)
            {
                bounds = null;
                bounds = drawableRight.Bounds;

                int x, y;
                int extraTapArea = 13;

                //
                //  IF USER CLICKS JUST OUT SIDE THE RECTANGLE OF THE DRAWABLE
                //  THAN ADD X AND SUBTRACT THE Y WITH SOME VALUE SO THAT AFTER
                //  CALCULATING X AND Y CO-ORDINATE LIES INTO THE DRAWBABLE
                //  BOUND. - this process help to increase the tappable area of
                //  the rectangle.
                //
                x = (int)(actionX + extraTapArea);
                y = (int)(actionY - extraTapArea);

                // Since this is right drawable subtract the value of x from the width
                // of view. so that width - tappedarea will result in x co-ordinate in drawable bound.
                //
                x = Width - x;

                //x can be negative if user taps at x co-ordinate just near the width.
                // e.g views width = 300 and user taps 290. Then as per previous calculation
                // 290 + 13 = 303. So subtract X from getWidth() will result in negative value.
                // So to avoid this add the value previous added when x goes negative.
                //

                if (x <= 0)
                {
                    x += extraTapArea;
                }

                // If result after calculating for extra tappable area is negative.
                // assign the original value so that after subtracting
                // extratapping area value doesn't go into negative value.
                //

                if (y <= 0)
                    y = actionY;

                //If drawble bounds contains the x and y points then move ahead./
                if (bounds.Contains(x, y))
                {
                    e.Action = (MotionEventActions.Cancel);
                    return false;
                }

                return base.OnTouchEvent(e);
            }
        }

        return base.OnTouchEvent(e);
    }

    protected override void JavaFinalize()
    {
        drawableRight = null;
        drawableBottom = null;
        drawableLeft = null;
        drawableTop = null;
        base.JavaFinalize();
    }

    public void UpdateAdapterStyle(ArrayAdapterStyle adapterStyle)
    {
        adapter.SetStyle(adapterStyle);
    }
}