using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Maui.MOLControls.Platforms.Android.Extensions;
using Object = Java.Lang.Object;
using View = Android.Views.View;

namespace Maui.MOLControls.Platforms.Android.NativeControls.ArrayAdapter;

public class AutoCompleteArrayAdapter : global::Android.Widget.ArrayAdapter, IFilterable
{
    private List<object> _originList = [];
    private Func<object, string> _labelFunc;
    private List<object> _filteredList;

    private readonly SuggestFilter _filter;

    protected Typeface? Face;
    protected float FontSize = 16;

    public AutoCompleteArrayAdapter(Context context, int resource, ArrayAdapterStyle? adapterStyle = null)
        : base(context, resource)
    {
        _filteredList = new List<object>(_originList);
        _filter = new SuggestFilter(this);
        SetStyle(adapterStyle);
    }

    public void SetStyle(ArrayAdapterStyle? adapterStyle)
    {
        if (adapterStyle == null)
        {
            return;
        }

        FontFamilyAsset.TryGeTypeFace(adapterStyle.FontFamily, out Face);
        FontSize = adapterStyle.FontSize;
    }

    public void UpdateList(IEnumerable<object> list, Func<object, string> labelFunc)
    {
        _labelFunc = labelFunc;
        _originList = list.ToList();
        _filteredList = _originList;
        NotifyDataSetChanged();
    }

    public override int Count => _filteredList.Count;

    public override Object? GetItem(int position)
    {
        return _labelFunc(GetObject(position));
    }

    public object GetObject(int position)
    {
        return _filteredList[position];
    }

    public override Filter Filter => _filter;

    public override View GetView(int position, View convertView, ViewGroup parent)
    {
        var view = base.GetView(position, convertView, parent);
        TextView? textView = view.FindViewById<TextView>(global::Android.Resource.Id.Text1);
        if (textView != null)
        {
            textView.Typeface = Face;
            textView.TextSize = FontSize;
        }

        return view;
    }

    private class SuggestFilter(AutoCompleteArrayAdapter adapter) : Filter
    {
        protected override FilterResults PerformFiltering(ICharSequence? constraint)
        {
            FilterResults results = new FilterResults();

            if (constraint is null || string.IsNullOrWhiteSpace(constraint.ToString()))
            {
                var castObject = adapter._originList.Select(item => new WrapperObject(item)).ToArray()!;
                results.Count = adapter._originList.Count;
                results.Values = castObject;
                return results;
            }

            var text = constraint.ToString().ToLower();

            var filteredArr = adapter._originList
                .Where(element => adapter._labelFunc(element).ToLower().Contains(text)).ToArray();

            var castFilteredObject = filteredArr.Select(item => new WrapperObject(item)).ToArray()!;
            results.Values = castFilteredObject;
            results.Count = filteredArr.Length;

            return results;
        }

        protected override void PublishResults(ICharSequence? constraint, FilterResults results)
        {
            if (results?.Values == null)
            {
                adapter._filteredList = new List<object>();
                return;
            }

            adapter._filteredList =
                (results.Values.ToArray<Object>() ?? Array.Empty<Object>())
                .Cast<WrapperObject>().Select(o => o.Value).ToList();

            adapter.NotifyDataSetChanged();
        }
    }
}

public class WrapperObject(object obj) : Java.Lang.Object
{
    public object Value { get; } = obj;
}