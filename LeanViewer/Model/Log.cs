using System;
using System.Globalization;

namespace LeanViewer.Model
{
    public class Log
    {
        public string DateLogged { get; set; }
        public string Message { get; set; }

        public Log(DateTime dateLogged, string message)
        {
            DateLogged = dateLogged.ToString("dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Message = message;
        }

        public override string ToString()
        {
            return DateLogged + "|" + Message;
        }
    }
}
