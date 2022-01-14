using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public class FileDialogFilter
    {
        public string Description;
        public List<string> FileExtensions;

        public FileDialogFilter(string description, params string[] fileExtensions)
        {
            if (fileExtensions.Length < 1)
                throw new ArgumentException(nameof(fileExtensions));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException(nameof(description));

            Description = description;
            FileExtensions = new List<string>();
            for (int i = 0; i < fileExtensions.Length; i++)
            {
                FileExtensions.Add(CheckExtensionFormat(fileExtensions[i]));
            }
        }

        private string CheckExtensionFormat(string str)
        {
            str = str.Trim();
            if (str.StartsWith("*."))
                return str;
            if (str.StartsWith("."))
                return $"*{str}";

            throw new ArgumentException($"Invalid file extension");
        }

        public static FileDialogFilter AllFilesFilter
        {
            get => new FileDialogFilter("All files (*.*)", "*.*");
        }

        public static string GetMultiFilterString(IEnumerable<FileDialogFilter> fileFilters)
        {
            //openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            StringBuilder b = new StringBuilder();
            int i = 1; int n = fileFilters.Count();
            foreach (var item in fileFilters)
            {
                b.Append(item.ToString());
                if (i < n)
                    b.Append("|");
                i++;
            }
            return b.ToString();
        }

        public override string ToString()
        {
            //openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            StringBuilder b = new StringBuilder();
            b.Append(Description);
            b.Append("|");
            for (int i = 0; i < FileExtensions.Count - 1; i++)
            {
                b.Append(FileExtensions[i]);
                b.Append(";");
            }
            b.Append(FileExtensions[^1]);
            return b.ToString();
        }
    }
}
