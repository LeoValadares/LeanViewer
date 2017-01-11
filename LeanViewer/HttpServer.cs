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
    public class HttpServer
    {
        public event HttpMessageDelegate OnHttpMessage = delegate { };

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

        public HttpServer()
        {
            _running = true;
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(_serverUrl);
            _httpListener.Start();
        }

        public void Start()
        {
            RegisterMessage("HttpListener running...");
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
            RegisterMessage("Cleaning up");
            _running = false;
            _httpListener.Close();
            RegisterMessage("Bye!!!");
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
            RegisterMessage(msg);
        }

        public void RegisterMessage(string message)
        {
            var log = new Log(DateTime.Now, message);
            OnHttpMessage(log);
        }

        public void RegisterMessage(Log logMessage)
        {
            OnHttpMessage(logMessage);
        }
    }
}
