using LeanViewer.Model;
using LeanViewer.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        public  ObservableCollection<LogScreenObject> VisibleLogs { get; set;  }
        private List<LogScreenObject> _allLogs;
        private HttpServer _httpServer;
        private Thread _httpServerThread;
        private FilterViewModel _filterViewModel;

        public LogViewModel()
        {
            _syncContext = SynchronizationContext.Current;
            _allLogs = new List<LogScreenObject>();
            VisibleLogs = new ObservableCollection<LogScreenObject>();
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
            AddLogToScreen(log);
            StoreLog(log);
        }

        private void AddLogToScreen(Log log)
        {
            if (!_filterViewModel.IsVisible(log)) return;
            var logOnScreen = new LogScreenObject(log, true);
            _syncContext.Send(x => VisibleLogs.Add(logOnScreen), null);
        }

        private void StoreLog(Log log)
        {
            var logOnScreen = new LogScreenObject(log, true);
            _allLogs.Add(logOnScreen);
        }

        private void RescanLogVisibility()
        {
            Parallel.ForEach<LogScreenObject>(_allLogs, x => x.IsVisible = _filterViewModel.IsVisible(x.UnderlyingLog));
            VisibleLogs.Clear();
            _allLogs.Where(x => x.IsVisible).ToList().ForEach(y => VisibleLogs.Add(y));
        }

        public void ClearScreen()
        {
            VisibleLogs.Clear();
        }

        public void Dispose()
        {
            _httpServer.Stop();
            _httpServerThread.Join();
        }

        public string SerializeLogs(IEnumerable<Log> logs)
        {
            return JsonConvert.SerializeObject(logs, Formatting.Indented);
        }
    }
}
