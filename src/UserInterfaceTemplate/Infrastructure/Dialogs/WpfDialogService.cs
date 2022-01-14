using UserInterfaceTemplate.Infrastructure.BaseViews;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

/* ViewModel is the DataContext of the Dialog View and must implement ICustomDialogResultHelper
 *   
 * EXAMPLE Viewmodel:  
 * 
 *  public void CloseDialog(bool withResult = false)
 *  {
 *     RequestCloseDialog?.Invoke(this, new RequestCloseEventArgs(withResult));
 *  }
 *  public event EventHandler<RequestCloseEventArgs> RequestCloseDialog;
 * 
 * ---------
 * To call the Dialog use implemented IDialogService:
 * 
 *  var xyz = myViewModelForTheDialog();
 *  
 *  bool? DialogResult = DialogService.ShowCustomUserDialog("Title of dialog window",xyz);
 *  //grab results:
 *  if (dialogresult.HasValue && dialogresult.Value==true)
 *  {
 *      do stuff with xyz (get values from state)
 *  }
 * 
 * */


namespace UserInterfaceTemplate.Infrastructure.Dialogs
{
    public class WpfDialogService : IDialogService
    {
        #region dialogs and popups

        public bool? ShowCustomUserDialog(string title, object datacontext, bool fixedWindow = true)
        {
            var dialogWindow = new DialogWindow();
            dialogWindow.Title = title;
            dialogWindow.DialogPresenter.Content = datacontext; //view for the datacontext defined in Vm2View.xaml

            if (!fixedWindow)
            {
                dialogWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                dialogWindow.BorderThickness = new Thickness(0);
            }
            else
            {
                dialogWindow.Owner = Application.Current.MainWindow;
                dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            return dialogWindow.ShowDialog();
        }

        public void ShowCustomChildWindow(string title, object datacontext)
        {
            ChildWindow win = new ChildWindow()
            {
                Title = title
            };
            win.DialogPresenter.Content = datacontext;
            win.Owner = Application.Current.MainWindow;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Show();
        }

        public bool ShowConfirmationDialog(string confirmationMsg, string title)
        {
            var vm = new VmUserConfirmation(title, confirmationMsg);
            var result = ShowCustomUserDialog(title, vm, true);
            return vm.UserConfirmation;
        }

        public void ShowUserPopup(string title, string popupMessage)
        {
            var vm = new VmUserPopup(title, popupMessage);
            ShowCustomUserDialog(title, vm);
        }

        public void ShowErrorPopup(string title, string popupMessage)
        {
            var vm = new VmErrorPopup(title, popupMessage);
            ShowCustomUserDialog(title, vm);
        }

        #endregion

        #region Open Files

        public bool TryOpenFileDialog(string defaultDirectory, IEnumerable<FileDialogFilter> fileFilters, out string resultFileName)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = CheckDir(defaultDirectory);
            openFileDialog.Filter = FileDialogFilter.GetMultiFilterString(fileFilters);
            openFileDialog.Multiselect = false;
            bool userHasSelected = openFileDialog.ShowDialog() == true;
            resultFileName = (userHasSelected) ? openFileDialog.FileName : string.Empty;
            return userHasSelected;
        }

        public bool TryOpenFileDialog(IEnumerable<FileDialogFilter> fileFilters, out string resultFileName)
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return TryOpenFileDialog(dir, fileFilters, out resultFileName);
        }


        public bool TryOpenFileDialog(string defaultDirectory, out string resultFileName)
        {
            FileDialogFilter[] f = new FileDialogFilter[] { FileDialogFilter.AllFilesFilter };
            return TryOpenFileDialog(defaultDirectory, f, out resultFileName);
        }


        public bool TryOpenFileDialog(out string resultFileName)
        {
            FileDialogFilter[] f = new FileDialogFilter[] { FileDialogFilter.AllFilesFilter };
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return TryOpenFileDialog(dir, f, out resultFileName);
        }

        public bool TryOpenMultiFileDialog(string defaultDirectory, IEnumerable<FileDialogFilter> fileFilters, out string[] ResultFileNames)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = CheckDir(defaultDirectory);
            openFileDialog.Filter = FileDialogFilter.GetMultiFilterString(fileFilters);
            openFileDialog.Multiselect = true;
            bool userHasSelected = openFileDialog.ShowDialog() == true;
            ResultFileNames = (userHasSelected) ? openFileDialog.FileNames : new string[0];
            return userHasSelected;
        }

        public bool TryOpenMultiFileDialog(IEnumerable<FileDialogFilter> fileFilters, out string[] ResultFileNames)
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return TryOpenMultiFileDialog(dir, fileFilters, out ResultFileNames);
        }

        public bool TryOpenMultiFileDialog(string defaultDirectory, out string[] ResultFileNames)
        {
            FileDialogFilter[] fileFilters = new FileDialogFilter[] { FileDialogFilter.AllFilesFilter };
            return TryOpenMultiFileDialog(defaultDirectory, fileFilters, out ResultFileNames);
        }

        public bool TryOpenMultiFileDialog(out string[] ResultFileNames)
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            FileDialogFilter[] fileFilters = new FileDialogFilter[] { FileDialogFilter.AllFilesFilter };
            return TryOpenMultiFileDialog(dir, fileFilters, out ResultFileNames);
        }

        #endregion

        #region Open Folder

        public bool TryOpenFolderDialog(out string folderName)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog folderDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            bool? result = folderDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                folderName = folderDialog.SelectedPath;
                return true;
            }
            else
            {
                folderName = string.Empty;
                return false;
            }
        }

        #endregion

        #region Save Files Dialog

        public bool TrySaveFileDialog(string defaultDirectory, string defaultName, FileDialogFilter fileFilter, out string resultFileName)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = CheckDir(defaultDirectory);
            dialog.FileName = defaultName;
            dialog.DefaultExt = fileFilter.FileExtensions[0];
            dialog.Filter = fileFilter.ToString();

            bool? result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                resultFileName = dialog.FileName;
                return true;
            }
            else
            {
                resultFileName = string.Empty;
                return false;
            }
        }

        public bool TrySaveFileDialog(string defaultName, FileDialogFilter filter, out string resultFileName)
        {
            string defDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return TrySaveFileDialog(defDir, defaultName, filter, out resultFileName);
        }


        #endregion


        public ProgressHandling ShowProgressPopup(double minimumProgress, double maximumProgress,string popupTitle, string headline)
        {
            ProgressHandling handling = new ProgressHandling();
            VmProgressReport progVm = new VmProgressReport(minimumProgress,maximumProgress,
                (Progress<double>)handling.ProgressReport, handling.CancellationTokenSource)
            {
                HeadlineText = headline
            };
            ShowCustomChildWindow(popupTitle, progVm);
            return handling;
        }

        private string CheckDir(string dir)
        {
            if (Directory.Exists(dir))
                return dir;

            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}
