using System;

namespace UserInterfaceTemplate.Infrastructure.Dialogs
{
    public class RequestCloseEventArgs : EventArgs
    {
        public RequestCloseEventArgs(bool dialogResult)
        {
            this.DialogResult = dialogResult;
        }

        public bool DialogResult { get; }

    }
}
