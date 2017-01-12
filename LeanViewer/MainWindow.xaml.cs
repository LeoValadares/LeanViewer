using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            _logViewModel = new LogViewModel();
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
    }
}
