using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using LeanViewer.Annotations;

namespace LeanViewer.Common
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>([NotNull]this ObservableCollection<T> collection, [NotNull]IEnumerable<T> range)
        {
            Type type = collection.GetType();

            type.InvokeMember("CheckReentrancy",
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, collection, null);

            foreach (var item in range)
            {
                collection.Add(item);
            }

            type.InvokeMember("OnPropertyChanged",
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null,
                collection, new object[] {new PropertyChangedEventArgs("Count")});

            type.InvokeMember("OnPropertyChanged",
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null,
                collection, new object[] {new PropertyChangedEventArgs("Item[]")});

            type.InvokeMember("OnCollectionChanged",
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null,
                collection, new object[] { new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset) });
        }
    }
}