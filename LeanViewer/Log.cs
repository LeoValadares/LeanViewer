using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeanViewer
{
    public class Log
    {
        public string DateLogged { get; set; }
        public string Message { get; set; }

        public Log(DateTime dateLogged, string message)
        {
            DateLogged = dateLogged.ToString("MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
            Message = message;
        }
    }
}
