using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LeanViewer.View;
using LeanViewer.ViewModel;

namespace LeanViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LogsWindow : Window
    {
        private readonly LogViewModel _logViewModel;

        public LogsWindow()
        {
            _logViewModel = LogViewModel.Current;
            this.DataContext = _logViewModel;
        }

        private void MessagesListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = (ListView)sender;
            GridView gView = (GridView)listView.View;

            var workingWidth = listView.ActualWidth - 35; // take into account vertical scrollbar
            var col1 = 0.25;
            var col2 = 0.75;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _logViewModel.Dispose();
        }

        private void CleanLogsButton_Click(object sender, RoutedEventArgs e)
        {
            _logViewModel.ClearScreen();
        }

        private void MessagesListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void FiltersButton_Click(object sender, RoutedEventArgs e)
        {
            var filtersWindow = new FiltersView {Owner = this};
            filtersWindow.Show();
        }
    }
}
