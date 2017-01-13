using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LeanViewer.Model;
using LeanViewer.ViewModel;

namespace LeanViewer.View
{
    /// <summary>
    /// Interaction logic for CreateFilterView.xaml
    /// </summary>
    public partial class CreateFilterView : Window
    {
        private readonly FilterViewModel _filterViewModel;

        public CreateFilterView()
        {
            InitializeComponent();
            _filterViewModel = FilterViewModel.Current;
        }

        private void CreateFilterButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedFilterType = (FilterType)this.FilterTypeCombobox.SelectedItem;
            var selectedVisibilityType = (VisibilityType) this.VisibilityCombobox.SelectedItem;
            var filterString = FilterStringTextBox.Text;
            var newFilter = new Filter(selectedFilterType, selectedVisibilityType, filterString);
            _filterViewModel.AddFilter(newFilter);
        }
    }
}
