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
        public string StringToFilter { get; set; }

        public Filter(FilterType filterType, VisibilityType visibilityType, string stringToFilter)
        {
            FilterType = filterType;
            VisibilityType = visibilityType;
            StringToFilter = stringToFilter;
        }
    }
}
