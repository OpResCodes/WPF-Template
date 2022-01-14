using System;

namespace UserInterfaceTemplate.Infrastructure.Dialogs
{
    public interface IDialogResultHelper
    {
        void CloseDialog(bool withResult = false);
        event EventHandler<RequestCloseEventArgs> RequestCloseDialog;
    }
}
