using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeanViewer
{
    class LogViewModel : IDisposable
    {
        private SynchronizationContext _syncContext;
        public ObservableCollection<Log> Logs { get; set; }
        private HttpServer _httpServer;
        private Thread _httpServerThread;

        public LogViewModel()
        {
            _syncContext = SynchronizationContext.Current;
            Logs = new ObservableCollection<Log>();
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
            _syncContext.Send(x => Logs.Add(log), null);
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
