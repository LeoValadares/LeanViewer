namespace LeanViewer.Model
{
    public enum FilterType
    {
        Exactly,
        Contains
    }

    public enum VisibilityType
    {
        Reveal,
        Hide
    }

    public class Filter
    {
        public FilterType FilterType { get; set; }
        public VisibilityType VisibilityType { get; set; }
        public string FilterString { get; set; }

        public Filter(FilterType filterType, VisibilityType visibilityType, string filterString)
        {
            FilterType = filterType;
            VisibilityType = visibilityType;
            FilterString = filterString;
        }
    }
}
