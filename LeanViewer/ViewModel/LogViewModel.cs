using LeanViewer.Model;
using LeanViewer.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LeanViewer.ViewModel
{
    class LogViewModel : IDisposable
    {
        private static LogViewModel _current;
        public static LogViewModel Current
        {
            get { return _current ?? (_current = new LogViewModel()); }
        }
        private List<LogScreenObject> _allLogs;
        private static readonly object _visibleLogsLock = new object();
        public  ObservableCollection<LogScreenObject> VisibleLogs { get; set;  }
        private HttpServer _httpServer;
        private Thread _httpServerThread;
        private FilterViewModel _filterViewModel;

        public LogViewModel()
        {
            _allLogs = new List<LogScreenObject>();
            VisibleLogs = new ObservableCollection<LogScreenObject>();
            BindingOperations.EnableCollectionSynchronization(VisibleLogs, _visibleLogsLock);
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
            VisibleLogs.Add(logOnScreen);
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
            var toRemove = _allLogs.Where(x => VisibleLogs.Contains(x)).ToList();
            toRemove.ForEach(x => _allLogs.Remove(x));
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
