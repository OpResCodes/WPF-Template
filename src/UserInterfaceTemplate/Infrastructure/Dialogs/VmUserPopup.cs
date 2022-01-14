using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;


namespace UserInterfaceTemplate.Infrastructure.Dialogs
{
    public class VmUserPopup : IDialogResultHelper
    {
        public VmUserPopup(string title, string popupText)
        {
            Title = title;
            PopupText = popupText;
            ClosePopup = new RelayCommand(OnClose);
        }

        public string PopupText { get; }

        public string Title { get; }

        public ICommand ClosePopup { get; }

        private void OnClose() => CloseDialog(false);

        public void CloseDialog(bool withResult = false)
        {
            RequestCloseDialog?.Invoke(this, new RequestCloseEventArgs(withResult));
        }

        public event EventHandler<RequestCloseEventArgs> RequestCloseDialog;
    }
}
