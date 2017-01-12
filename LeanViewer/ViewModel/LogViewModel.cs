using System;
using System.Collections.ObjectModel;
using System.Threading;
using LeanViewer.Model;
using LeanViewer.Network;

namespace LeanViewer.ViewModel
{
    class LogViewModel : IDisposable
    {
        private static LogViewModel _current;
        public static LogViewModel Current
        {
            get { return _current ?? (_current = new LogViewModel()); }
        }
        private SynchronizationContext _syncContext;
        public ObservableCollection<LogScreenObject> Logs { get; set; }
        private HttpServer _httpServer;
        private Thread _httpServerThread;
        private FilterViewModel _filterViewModel;

        public LogViewModel()
        {
            _syncContext = SynchronizationContext.Current;
            Logs = new ObservableCollection<LogScreenObject>();
            _filterViewModel = FilterViewModel.Current;
            _httpServer = HttpServer.Current;
            _httpServer.OnHttpMessage += OnHttpMessage;
            _httpServer.OnServerMessage += OnServerMessage;
            _httpServerThread = new Thread(() => _httpServer.Start());
            _httpServerThread.Start();
        }

        private void OnHttpMessage(Log log)
        {
            LogSink(log);
        }

        private void OnServerMessage(Log log)
        {
            LogSink(log);
        }

        private void LogSink(Log log)
        {
            AddLogToScreen(log);
        }

        private void AddLogToScreen(Log log)
        {
            var logOnScreen = new LogScreenObject(log, true);
            _syncContext.Send(x => Logs.Add(logOnScreen), null);
        }

        public void ClearScreen()
        {
            Logs.Clear();
        }

        public void Dispose()
        {
            _httpServer.Stop();
            _httpServerThread.Join();
        }
    }
}
