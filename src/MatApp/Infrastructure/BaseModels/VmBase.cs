using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MatApp.Infrastructure.Dialogs;
using MatApp.Infrastructure.ExceptionHandling;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MatApp.Infrastructure.BaseModels
{
    public class VmBase : ObservableRecipient, IExceptionHandler
    {
        protected readonly IConfiguration _config;
        protected readonly ILogger _log;
        protected readonly IDialogService _dialogService;
        protected readonly UserSettingsService _userSettingsService;
        private Dictionary<string, List<IRelayCommand>> _cmdLookup;
        private readonly IExceptionHandler[] _exceptionHandler;

        public VmBase()
        {
            var services = App.AppHost.Services;
            _config = services.GetService<IConfiguration>();
            _exceptionHandler = services.GetServices<IExceptionHandler>().ToArray();
            _dialogService = services.GetService<IDialogService>();
            _userSettingsService = services.GetService<UserSettingsService>();
            Type tlogger = typeof(ILogger<>);
            Type t = tlogger.MakeGenericType(this.GetType());
            _log = (ILogger)services.GetService(t);
            _cmdLookup = new Dictionary<string, List<IRelayCommand>>();
            this.PropertyChanged += UpdateCommandAvailability;
        }


        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private void UpdateCommandAvailability(object sender, PropertyChangedEventArgs e)
        {
            if (!_cmdLookup.ContainsKey(e.PropertyName))
                return;

            var commandsToCheck = _cmdLookup[e.PropertyName];
            foreach (var item in commandsToCheck)
            {
                item.NotifyCanExecuteChanged();
            }
        }

        protected void CommandWatchProperty(string propertyName, IRelayCommand relayCommand)
        {
            if (!_cmdLookup.TryGetValue(propertyName, out List<IRelayCommand> commands))
            {
                commands = new List<IRelayCommand>();
                _cmdLookup.Add(propertyName, commands);
            }
            commands.Add(relayCommand);
        }

        public void HandleException(Exception ex)
        {
            for (int i = 0; i < _exceptionHandler.Length; i++)
            {
                _exceptionHandler[i].HandleException(ex);
            }
        }
    }
}
