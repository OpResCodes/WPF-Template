using CommunityToolkit.Mvvm.Input;
using UserInterfaceTemplate.Infrastructure;
using UserInterfaceTemplate.Infrastructure.BaseModels;
using UserInterfaceTemplate.Infrastructure.Dialogs;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterfaceTemplate.Viewmodels
{
    public class VmOne : VmBase
    {
        private string _text;
        private readonly IMainController _mainController;

        public VmOne(IMainController mainController)
        {
            _text = string.Format("Dies ist Viewmodel 1. Es wurde um {0} erstellt.",
                DateTime.Now.ToString());
            _log.LogDebug("vmOne erstellt.");
            _mainController = mainController;

            CreateSampleList();
            ChangeView = new RelayCommand(() => _mainController.DisplayViewModel<VmTwo>());

            //Setup Command to auto-watch properties for enabling/disabling it
            ClickCommand = new RelayCommand(
                execute: ChangeNameField,
                canExecute: () => _nameField.Equals("Matthes") && NameList.SelectedItems.Count() > 1);
            CommandWatchProperty(nameof(NameField), ClickCommand);
            CommandWatchProperty(nameof(NameList), ClickCommand);

            //The SelectableItemCollection informs about a property change of the collection based
            //on a selection change
            //whitin the collection (the observablecollections propertychanged event is unfortunately private!)
            NameList.SelectionChanged += (r, m) => OnPropertyChanged(nameof(NameList));


            //gets settings from the user settings service
            _log.LogDebug("Some Usersettings: {0}",_userSettingsService.UserSettings.SomeUserSetting);
        }

        private void CreateSampleList()
        {
            string[] personen = new string[]
            {
                "Peter Pan", "Ralf Möller","Klaus Keks","Rudolph Völler","Werner Beinhart"
            };
            NameList = new SelectableItemCollection(personen);
        }

        public string MeinText
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }


        private string _nameField = string.Empty;
        public string NameField
        {
            get => _nameField;
            set => SetProperty(ref _nameField, value);
        }

        private void ChangeNameField()
        {
            NameField = "Ich bin wieder leer.";
        }

        public SelectableItemCollection NameList { get; private set; }

        public RelayCommand ChangeView { get; }

        public RelayCommand ClickCommand { get; }

    }

    public class VmTwo : VmBase
    {
        private string _text;
        private bool _commandsEnabled;

        public VmTwo()
        {
            _text = string.Format("Dies ist Viewmodel 2. Es wurde um {0} erstellt.",
                DateTime.Now.ToString());
            _log.LogDebug("VmTwo erstellt.");

            SelectFolderCommand = new RelayCommand(OnSelectFolder,
                () => _commandsEnabled);
            CommandWatchProperty(nameof(CommandsEnabled), SelectFolderCommand);

            SelectFileCommand = new RelayCommand(OnSelectFile,
                () => _commandsEnabled);
            CommandWatchProperty(nameof(CommandsEnabled), SelectFileCommand);

            SelectMultiFilesCommand = new RelayCommand(OnSelectMultpleFiles,
                () => _commandsEnabled);
            CommandWatchProperty(nameof(CommandsEnabled), SelectMultiFilesCommand);

            SelectFileSimpleCommand = new RelayCommand(OnSelectFileSimple,
                () => _commandsEnabled);
            CommandWatchProperty(nameof(CommandsEnabled), SelectFileSimpleCommand);

            SaveFilesCommand = new AsyncRelayCommand(OnSaveFile,
                () => _commandsEnabled);
            CommandWatchProperty(nameof(CommandsEnabled), SaveFilesCommand);

            ProgressDemoCommand = new AsyncRelayCommand(OnTestProgress,
                () => _commandsEnabled);
            CommandWatchProperty(nameof(CommandsEnabled), ProgressDemoCommand);

            CommandsEnabled = false;
        }

        public string MeinText
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }


        public bool CommandsEnabled
        {
            get => _commandsEnabled;
            set => SetProperty(ref _commandsEnabled, value);
        }


        public RelayCommand SelectFolderCommand { get; }

        public RelayCommand SelectFileCommand { get; }

        public RelayCommand SelectMultiFilesCommand { get; }

        public IRelayCommand SelectFileSimpleCommand { get; }

        public IAsyncRelayCommand SaveFilesCommand { get; }

        public IAsyncRelayCommand ProgressDemoCommand { get; }

        private void OnSelectFile()
        {
            FileDialogFilter filterTxt = new FileDialogFilter("Text Dateien", "*.txt", "*.csv");
            FileDialogFilter filterXls = new FileDialogFilter("Excel Tabellen", "*.xlsx", "*.xlsm", "*.xls");
            if (_dialogService.TryOpenFileDialog(new FileDialogFilter[] { filterTxt, filterXls }, out string fileName))
            {
                _dialogService.ShowUserPopup("Datei gewählt", $"Die folgende Datei wurde gewählt:\n{fileName}");
            }
        }

        private void OnSelectFileSimple()
        {
            if (_dialogService.TryOpenFileDialog(out string fileName))
            {
                _dialogService.ShowUserPopup("Datei gewählt", fileName);
            }
        }

        private void OnSelectMultpleFiles()
        {
            FileDialogFilter filter = new FileDialogFilter("PDF Dateien", "*.pdf");
            if (_dialogService.TryOpenMultiFileDialog(new FileDialogFilter[] { filter }, out string[] pdfFiles))
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < pdfFiles.Length; i++)
                {
                    sb.AppendLine($" - {pdfFiles[0]}");
                }
                _dialogService.ShowUserPopup("Dateien gewählt", $"Die folgenden Dateien wurden ausgewählt:\n{sb.ToString()}");
            }
        }

        private async Task OnSaveFile()
        {
            CommandsEnabled = false;
            try
            {
                string text = "Dies ist ein Text.";
                if (_dialogService.TrySaveFileDialog("Testdatei.txt", new FileDialogFilter("Text Datei", "*.txt"), out string saveFilePath))
                {
                    using (StreamWriter w = new StreamWriter(saveFilePath))
                    {
                        await w.WriteAsync(text);
                    }
                    _dialogService.ShowUserPopup("Gespeichert", "Die Datei wurde gespeichert.");
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                CommandsEnabled = true;
            }
        }

        private void OnSelectFolder()
        {
            if (_dialogService.TryOpenFolderDialog(out string folder))
            {
                _dialogService.ShowUserPopup("Select Folder",
                    $"Selected folder:\n{folder}");
            }
        }

        private async Task OnTestProgress()
        {
            CommandsEnabled = false;
            int j = 0;
            int iter = 20;
            try
            {
                var progHandling = _dialogService.ShowProgressPopup(0, iter, "Computation", "Doing some heavy computation...");
                for (int i = 1; i <= iter; i++)
                {
                    await ExpensiveOperation();
                    j = i;
                    progHandling.CancellationTokenSource.Token.ThrowIfCancellationRequested();
                    progHandling.ProgressReport.Report(i);
                }

            }
            catch (OperationCanceledException)
            {
                _dialogService.ShowErrorPopup("Operation cancelled.", $"The operation was cancelled after iteration number {j}!");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                CommandsEnabled = true;
            }
        }

        private Task ExpensiveOperation()
        {
            return Task.Delay(200);
        }


    }
}
