﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LeanViewer.Model;
using System.Linq;
using Newtonsoft.Json;

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
            bool isVisible = true;
            if (Filters.Count < 1) return true;
            foreach (var filter in Filters)
            {
                isVisible &= IsVisible(filter, log);
            }
            return isVisible;
        }

        private static bool IsVisible(Filter filter, Log log)
        {
            switch (filter.FilterType)
            {
                case FilterType.Contains:
                    var contains = log.Message.Contains(filter.FilterString);
                    if (filter.VisibilityType == VisibilityType.Reveal && contains) return true;
                    if (filter.VisibilityType == VisibilityType.Hide && !contains) return true;
                    else return false;
                    break;
                case FilterType.Exactly:
                    var exactly = log.Message == filter.FilterString;
                    if (filter.VisibilityType == VisibilityType.Reveal && exactly) return true;
                    if (filter.VisibilityType == VisibilityType.Hide && !exactly) return true;
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

        public void RestoreFromFile(string fileContents)
        {
            var filters = JsonConvert.DeserializeObject<List<Filter>>(fileContents);
            filters.ForEach(x => Filters.Add(x));
        }

        public string SerializeCurrentFilters()
        {
            var filters = Filters.ToList();
            var serialized = JsonConvert.SerializeObject(filters, Formatting.Indented);
            return serialized;
        }
    }
}
