using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeanViewer
{
    public delegate void HttpMessageDelegate(Log args);
    public delegate void ServerMessageDelegate(Log args);

    public class HttpServer
    {
        private static HttpServer _current;
        public static HttpServer Current
        {
            get { return _current ?? (_current = new HttpServer()); }
        }
        public event HttpMessageDelegate OnHttpMessage = delegate { };
        public event ServerMessageDelegate OnServerMessage = delegate { };

        private int _port = 8000;
        private string _server = "*";
        private string _serverUrl
        {
            get
            {
                return "http://" + _server + ":" + _port + "/";
            }
        }
        private HttpListener _httpListener;
        private bool _running;

        public HttpServer() { }

        public void Start()
        {
            _running = true;
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(_serverUrl);
            _httpListener.Start();
            RegisterServerMessage("HttpListener running...");
            while (_running)
            {
                try
                {
                    var ctx = _httpListener.GetContext();
                    new Thread(() => ProcessMessage(ctx)).Start();
                }
                catch (Exception) { }
            }
        }

        public void Stop()
        {
            RegisterServerMessage("Cleaning up");
            _running = false;
            _httpListener.Close();
            RegisterServerMessage("Bye!!!");
        }

        public void ProcessMessage(HttpListenerContext listenerContext)
        {
            string msg;
            using (var sr = new StreamReader(listenerContext.Request.InputStream))
            {
                msg = sr.ReadToEnd();
            }
            byte[] b = Encoding.UTF8.GetBytes("ok.");
            listenerContext.Response.StatusCode = 200;
            listenerContext.Response.KeepAlive = false;
            listenerContext.Response.ContentLength64 = b.Length;
            var output = listenerContext.Response.OutputStream;
            output.Write(b, 0, b.Length);
            listenerContext.Response.Close();
            RegisterHttpMessage(msg);
        }

        public void RegisterHttpMessage(string message)
        {
            var log = new Log(DateTime.Now, message);
            RegisterHttpMessage(log);
        }

        public void RegisterHttpMessage(Log logMessage)
        {
            OnHttpMessage(logMessage);
        }

        public void RegisterServerMessage(string message)
        {
            var log = new Log(DateTime.Now, message);
            RegisterServerMessage(log);
        }

        public void RegisterServerMessage(Log serverLog)
        {
            OnServerMessage(serverLog);
        }
    }
}
