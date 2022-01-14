using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;

namespace UserInterfaceTemplate.Infrastructure.Dialogs
{
    public class VmUserConfirmation : IDialogResultHelper
    {

        public VmUserConfirmation(string title, string question)
        {
            UserConfirms = new RelayCommand(OnConfirm);
            UserDoesNotConfirm = new RelayCommand(OnUnconfirm);
            Title = title;
            Question = question;
            UserConfirmation = false;
        }

        public bool UserConfirmation { get; set; }

        public string Title { get; }

        public string Question { get; }

        public ICommand UserConfirms { get; }

        public ICommand UserDoesNotConfirm { get; }

        private void OnConfirm()
        {
            UserConfirmation = true;
            CloseDialog(true);
        }

        private void OnUnconfirm()
        {
            CloseDialog(false);
        }

        public void CloseDialog(bool withResult = false)
        {
            RequestCloseDialog?.Invoke(this, new RequestCloseEventArgs(withResult));
        }

        public event EventHandler<RequestCloseEventArgs> RequestCloseDialog;
    }
}
