using UserInterfaceTemplate.Infrastructure;
using UserInterfaceTemplate.Infrastructure.BaseModels;
using Microsoft.Extensions.Logging;

namespace UserInterfaceTemplate.Viewmodels
{
    public class MainVm : VmBase
    {
        private string _title;

        public MainVm(IMainController mainController)
        {
            Title = _config["Title"] ?? "Nicht konfiguriert.";
            _log.LogDebug("Main Viewmodel erzeugt.");
            MainController = mainController;
            mainController.DisplayViewModel<VmOne>();
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public IMainController MainController { get; }
    }
}
