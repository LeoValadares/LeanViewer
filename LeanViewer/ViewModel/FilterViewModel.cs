using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LeanViewer.Model;
using System.Linq;

namespace LeanViewer.ViewModel
{
    public delegate void FiltersUpdatedEventHandler();
    public class FilterViewModel
    {
        public event FiltersUpdatedEventHandler FiltersUpdatedEvent = delegate { };

        private static FilterViewModel _current;
        public static FilterViewModel Current
        {
            get { return _current ?? (_current = new FilterViewModel()); }
        }

        public ObservableCollection<Filter> Filters { get; set; }

        public FilterViewModel()
        {
            Filters = new ObservableCollection<Filter>();
            Filters.CollectionChanged += OnFiltersUpdated;
        }

        private void OnFiltersUpdated(object sender, NotifyCollectionChangedEventArgs e)
        {
            FiltersUpdatedEvent();
        }

        public bool IsVisible(Log log)
        {
            if (Filters.Count < 1) return true;
            foreach (var filter in Filters)
            {
                if (IsVisible(filter, log)) return true;
            }
            return false;
        }

        private static bool IsVisible(Filter filter, Log log)
        {
            switch (filter.FilterType)
            {
                case FilterType.Contains:
                    var contains = log.Message.Contains(filter.FilterString);
                    if (filter.VisibilityType == VisibilityType.Reveal && contains) return true;
                    else return false;
                    break;
                case FilterType.Exactly:
                    var exactly = log.Message == filter.FilterString;
                    if (filter.VisibilityType == VisibilityType.Reveal && exactly) return true;
                    else return false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddFilter(Filter filterToAdd)
        {
            Filters.Add(filterToAdd);
        }

        public void ClearFilters()
        {
            Filters.Clear();
        }

        public void Remove(Filter selectedItem)
        {
            Filters.Remove(selectedItem);
        }
    }
}
