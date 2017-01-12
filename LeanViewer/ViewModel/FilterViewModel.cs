using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeanViewer.Model;

namespace LeanViewer.ViewModel
{
    public class FilterViewModel
    {
        private static FilterViewModel _current;
        public static FilterViewModel Current
        {
            get { return _current ?? (_current = new FilterViewModel()); }
        }

        public ObservableCollection<Filter> Filters { get; set; }

        public bool GetLogVisibility(Log log)
        {
            foreach (var filter in Filters)
            {
                if (IsVisible(filter, log)) return false;
            }
            return true;
        }

        private bool IsVisible(Filter filter, Log log)
        {
            switch (filter.FilterType)
            {
                case FilterType.Contains:
                    var contains = log.Message.Contains(filter.StringToFilter);
                    if(filter.VisibilityType == VisibilityType.Reveal)
                    break;
                case FilterType.Exactly:
                    return log.Message == filter.StringToFilter;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
