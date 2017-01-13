using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using LeanViewer.Model;
using LeanViewer.ViewModel;

namespace LeanViewer.View
{
    /// <summary>
    /// Interaction logic for FiltersView.xaml
    /// </summary>
    public partial class FiltersView : Window
    {
        private FilterViewModel _filterViewModel;
        public FiltersView()
        {
            InitializeComponent();
            _filterViewModel = FilterViewModel.Current;
            this.DataContext = _filterViewModel;
        }

        private void MessagesListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = (ListView)sender;
            GridView gView = (GridView)listView.View;

            var workingWidth = listView.ActualWidth - 35; // take into account vertical scrollbar
            var col1 = 0.2;
            var col2 = 0.2;
            var col3 = 0.6;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
            gView.Columns[2].Width = workingWidth * col3;
        }

        private void ClearFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            _filterViewModel.ClearFilters();
        }

        private void AddFilterButton_Click(object sender, RoutedEventArgs e)
        {
            var newFilterWindow = new CreateFilterView {Owner = this};
            newFilterWindow.ShowDialog();
        }

        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
