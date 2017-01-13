using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using LeanViewer.Annotations;

namespace LeanViewer.Model
{
    public class LogScreenObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Log UnderlyingLog { get; set; }
        public string DateLogged
        {
            get { return UnderlyingLog.DateLogged; }
            private set { }
        }
        public string Message
        {
            get { return UnderlyingLog.Message; }
            private set { }
        }
        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public LogScreenObject(Log underlyingLog, bool isVisible)
        {
            UnderlyingLog = underlyingLog;
            IsVisible = isVisible;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
