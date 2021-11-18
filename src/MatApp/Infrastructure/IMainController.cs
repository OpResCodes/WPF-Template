using MatApp.Infrastructure.BaseModels;

namespace MatApp.Infrastructure
{
    public interface IMainController
    {
        void DisplayViewModel(VmBase viewModel);

        void DisplayViewModel<T>() where T : VmBase;

        VmBase SelectedViewModel { get; set; }
    }
}
