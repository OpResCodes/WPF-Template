using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MatApp.Infrastructure;
using MatApp.Infrastructure.BaseModels;
using System;

namespace MatApp.Viewmodels
{
    public class MainController : ObservableObject, IMainController
    {
        private readonly IServiceProvider _services;

        public MainController(IServiceProvider services)
        {
            _services = services;
            Navigate = new RelayCommand<string>(MainMenu);
        }

        private VmBase _selectedVm;
        public VmBase SelectedViewModel
        {
            get => _selectedVm;
            set => SetProperty(ref _selectedVm, value);
        }

        public RelayCommand<string> Navigate { get; }

        public void MainMenu(string view)
        {
            switch (view)
            {
                case "vm1":
                    DisplayViewModel<VmOne>();
                    break;
                case "vm2":
                    DisplayViewModel<VmTwo>();
                    break;
                default:
                    break;
            }
        }

        public void DisplayViewModel(VmBase viewModel)
        {
            if (_selectedVm != null)
            {
                _selectedVm.IsActive = false;
            }
            SelectedViewModel = viewModel;
            if (_selectedVm != null)
            {
                _selectedVm.IsActive = true;
            }
        }

        public void DisplayViewModel<T>() where T : VmBase
        {
            var t = typeof(T);
            var vm = (T)_services.GetService(t);
            DisplayViewModel(vm);
        }

    }
}
