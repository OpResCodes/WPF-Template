using UserInterfaceTemplate.Infrastructure.BaseModels;

namespace UserInterfaceTemplate.Infrastructure
{
    public interface IMainController
    {
        void DisplayViewModel(VmBase viewModel);

        void DisplayViewModel<T>() where T : VmBase;

        VmBase SelectedViewModel { get; set; }
    }
}
