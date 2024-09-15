using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Maui.MOLControls.Platforms.Android.Extensions;
using Color = Android.Graphics.Color;
using TextAlignment = Android.Views.TextAlignment;
using View = Android.Views.View;


namespace Maui.MOLControls.Platforms.Android.NativeControls.ArrayAdapter;

public class SimpleArrayAdapter : global::Android.Widget.ArrayAdapter
{
    protected List<object> ResultList;
    protected Func<object, string> LabelFunc;
    protected Typeface? Face;
    protected float FontSize = 16;
    protected Drawable? Drawable;
    protected ArrayAdapterStyle AdapterStyle;

    public SimpleArrayAdapter(Context context, int textViewResourceId,
        ArrayAdapterStyle? adapterStyle = null) : base(context,
        textViewResourceId)
    {
        Filter = new SimpleArrayAdapterFilter(this);
        ResultList = new List<object>();

        if (adapterStyle == null)
        {
            return;
        }

        SetStyle(adapterStyle);
        SetNotifyOnChange(true);
    }

    public void SetStyle(ArrayAdapterStyle? adapterStyle)
    {
        if (adapterStyle == null)
        {
            return;
        }

        AdapterStyle = adapterStyle;

        FontFamilyAsset.TryGeTypeFace(adapterStyle.FontFamily, out Face);
        FontSize = adapterStyle.FontSize;
        if (!string.IsNullOrWhiteSpace(adapterStyle.Icon))
        {
            var t = Context.Resources.GetIdentifier(adapterStyle.Icon, "drawable", Context.PackageName);
            if (t != 0)
            {
                Drawable = Context.Resources.GetDrawable(t);
            }
        }
    }

    public void UpdateList(IEnumerable<object> list, Func<object, string> labelFunc)
    {
        this.LabelFunc = labelFunc;
        ResultList = list.ToList();
        NotifyDataSetChanged();
    }

    public override Filter Filter { get; }
    public override int Count => ResultList.Count;
    public override View GetView(int position, View? convertView, ViewGroup parent)
    {
        LinearLayout layout = new LinearLayout(parent.Context)
        {
            Orientation = Orientation.Horizontal,
            LayoutParameters =
                new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent),
        };
        layout.SetVerticalGravity(GravityFlags.Center);
        TextView textView = new TextView(parent.Context)
        {
            LayoutParameters =
                new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.WrapContent, 1)
                {
                    TopMargin = 30,
                    BottomMargin = 30,
                    LeftMargin = 20,
                    RightMargin = 6,
                },
            Gravity = GravityFlags.CenterVertical,
            Text = LabelFunc(GetObject(position)),
            Typeface = Face,
            TextSize = FontSize,
        };
        ImageView imageView = new ImageView(parent.Context)
        {
            LayoutParameters =
                new LinearLayout.LayoutParams(AdapterStyle.IconListWidth,
                    AdapterStyle.IconListHeight)
                {
                    TopMargin = AdapterStyle.IconListVerticalMargin,
                    BottomMargin = AdapterStyle.IconListVerticalMargin,
                },
        };

        layout.AddView(textView);
        layout.AddView(imageView);
        layout.SetBackgroundColor(AdapterStyle.Background);

        textView.SetSingleLine(AdapterStyle.IsSingleLine);
        textView.SetLines(AdapterStyle.ListTextLines);
        textView.SetTextColor(AdapterStyle.TextColor);
        textView.Ellipsize = global::Android.Text.TextUtils.TruncateAt.End;
        textView.SetMinimumHeight(AdapterStyle.ListMinimumTextHeight);

        imageView.SetImageDrawable(Drawable);
        imageView.SetScaleType(ImageView.ScaleType.FitCenter);

        if (!string.IsNullOrWhiteSpace(AdapterStyle.Icon))
        {
            View view = new View(parent.Context)
            {
                LayoutParameters = new ViewGroup.LayoutParams(4, ViewGroup.LayoutParams.MatchParent),
            };

            view.SetBackgroundColor(AdapterStyle.DividerColor);

            layout.AddView(view, 1);
        }

        // imageView.SetBackgroundColor(Color.AliceBlue);
        // text.SetBackgroundColor(Color.Bisque);
        return layout;
    }

    public override Java.Lang.Object GetItem(int position)
    {
        return LabelFunc(GetObject(position));
    }

    public object GetObject(int position)
    {
        return ResultList[position];
    }

    private class SimpleArrayAdapterFilter(SimpleArrayAdapter adapter) : Filter
    {
        private SimpleArrayAdapter Adapter { get; } = adapter;

        protected override FilterResults? PerformFiltering(ICharSequence? constraint)
        {
            if (Adapter.ResultList == null || Adapter.LabelFunc == null)
            {
                return new FilterResults()
                {
                    Values = null,
                    Count = 0
                };
            }

            return new FilterResults()
            {
                Count = Adapter.ResultList.Count,
                Values = Adapter.ResultList.Select(Adapter.LabelFunc).ToArray()
            };
        }

        protected override void PublishResults(ICharSequence? constraint, FilterResults? results)
        {
            Adapter.NotifyDataSetChanged();
        }
    }
}