using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using LeanViewer.Model;
using LeanViewer.ViewModel;
using Microsoft.Win32;

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

        private void LoadFiltersButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            try
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                _filterViewModel.RestoreFromFile(sr.ReadToEnd());
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, "Houve um erro lendo o arquivo");
            }
        }

        private void SaveFiltersButton_OnClick(object sender, RoutedEventArgs e)
        {
            var objectToSave = _filterViewModel.SerializeCurrentFilters();
            SaveFileDialog sfd = new SaveFileDialog
            {
                DefaultExt = ".json",
                Filter = "JavaScript Object Notation File | *.json"
            };
            var result = sfd.ShowDialog();

            if (result == true)
            {
                var fileName = sfd.FileName;
                File.WriteAllText(fileName, objectToSave);
            }
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
            var selectedFilters = (IList) FiltersListView.SelectedItems;
            selectedFilters.Cast<Filter>().ToList().ForEach(x => _filterViewModel.Remove(x));
        }

        private void FiltersView_OnClosing(object sender, CancelEventArgs e)
        {
            if(Owner != null) Owner.Focus();
        }
    }
}
