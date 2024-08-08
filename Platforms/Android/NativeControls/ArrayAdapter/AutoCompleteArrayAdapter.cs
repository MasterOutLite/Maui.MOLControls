using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;
using View = Android.Views.View;

namespace Maui.MOLControls.Platforms.Android.NativeControls.ArrayAdapter;

public class AutoCompleteArrayAdapter : global::Android.Widget.ArrayAdapter, IFilterable
{
    private List<object> _originList = [];
    private Func<object, string> labelFunc;
    private List<object> _filteredList;

    private readonly SuggestFilter filter;

    public AutoCompleteArrayAdapter(Context context, int resource)
        : base(context, resource)
    {
        _filteredList = new List<object>(_originList);
        filter = new SuggestFilter(this);
    }

    public override int Count => _filteredList.Count;

    public override Filter Filter => filter;

    public override View GetView(int position, View convertView, ViewGroup parent)
    {
        View view = base.GetView(position, convertView, parent);
        TextView textView = view.FindViewById<TextView>(global::Android.Resource.Id.Text1);
        textView.Text = (string?)_filteredList[position];
        return view;
    }

    private class SuggestFilter : Filter
    {
        private AutoCompleteArrayAdapter adapter;

        public SuggestFilter(AutoCompleteArrayAdapter adapter)
        {
            this.adapter = adapter;
        }

        protected override FilterResults PerformFiltering(ICharSequence constraint)
        {
            FilterResults results = new FilterResults();
            if (constraint != null)
            {
            }

            return results;
        }

        protected override void PublishResults(ICharSequence constraint, FilterResults results)
        {
            adapter.NotifyDataSetChanged();
        }
    }
}