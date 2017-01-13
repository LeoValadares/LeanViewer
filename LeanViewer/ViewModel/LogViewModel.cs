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
            _filterViewModel.FiltersUpdatedEvent += RescanLogVisibility;
            _httpServer = HttpServer.Current;
            _httpServer.HttpMessageReceivedEvent += HttpMessageReceivedEvent;
            _httpServer.ServerMessageEvent += ServerMessageEvent;
            _httpServerThread = new Thread(() => _httpServer.Start());
            _httpServerThread.Start();
        }

        private void HttpMessageReceivedEvent(Log log)
        {
            LogSink(log);
        }

        private void ServerMessageEvent(Log log)
        {
            LogSink(log);
        }

        private void LogSink(Log log)
        {
            if (!_filterViewModel.IsVisible(log)) return;
            AddLogToScreen(log);
        }

        private void RescanLogVisibility()
        {
            foreach (var logScreenObject in Logs)
            {
                logScreenObject.IsVisible = _filterViewModel.IsVisible(logScreenObject.UnderlyingLog);
            }
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
