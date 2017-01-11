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
        public ObservableCollection<Log> Logs { get; set; }
        private HttpServer _httpServer;
        private Thread _httpServerThread;
        private SynchronizationContext _syncContext;

        public LogsWindow()
        {
            InitializeComponent();
            _syncContext = SynchronizationContext.Current;
            Logs = new ObservableCollection<Log>();
            _httpServer = new HttpServer();
            _httpServer.OnHttpMessage += OnHttpMessageReceived;
            this.DataContext = this;
            _httpServerThread = new Thread(() => _httpServer.Start());
            _httpServerThread.Start();
        }

        private void OnHttpMessageReceived(Log logMessage)
        {
            _syncContext.Send(x => Logs.Add(logMessage), null);
        }

        private void MessagesListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - 35; // take into account vertical scrollbar
            var col1 = 0.25;
            var col2 = 0.75;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _httpServer.Stop();
            _httpServerThread.Abort();
        }

        private void CleanLogsButton_Click(object sender, RoutedEventArgs e)
        {
            Logs.Clear();
        }

        private void MessagesListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var source = ((ListView)sender).SelectedItem;
            var window = new LogsWindow();
        }
    }
}
