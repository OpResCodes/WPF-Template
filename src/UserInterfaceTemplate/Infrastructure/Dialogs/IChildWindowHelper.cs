namespace UserInterfaceTemplate.Infrastructure.Dialogs
{
    public interface IChildWindowHelper : IDialogResultHelper
    {
        void WindowRequestsClose(object sender, RequestCloseEventArgs e);
    }
}
