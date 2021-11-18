using System.Collections.Generic;

namespace MatApp.Infrastructure.Dialogs
{
    public interface IDialogService
    {

        /// <summary>
        /// shows a custom user dialog
        /// </summary>
        /// <param name="title"></param>
        /// <param name="datacontext"></param>
        /// <returns></returns>
        bool? ShowCustomUserDialog(string title, object datacontext, bool fixedWindow = true);

        /// <summary>
        /// Display a child window (non modal)
        /// </summary>
        /// <param name="title">Title of the Child Window</param>
        /// <param name="datacontext">DataContext of the Child</param>
        void ShowCustomChildWindow(string title, object datacontext);

        /// <summary>
        /// Shows a yes/no/abort Messagebox
        /// </summary>
        /// <param name="confirmationMsg"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        bool ShowConfirmationDialog(string confirmationMsg, string title);

        /// <summary>
        /// Shows a standard Messagebox
        /// </summary>
        /// <param name="title"></param>
        /// <param name="popupMessage"></param>
        void ShowUserPopup(string title, string popupMessage);

        /// <summary>
        /// Display indication of error
        /// </summary>
        /// <param name="title">The Popup Title</param>
        /// <param name="popupMessage">The Error Message</param>
        void ShowErrorPopup(string title, string popupMessage);

        /// <summary>
        /// Open a single file with the possible extension(s), starting at the given directory
        /// </summary>
        /// <param name="defaultDirectory">Initial directory of the dialog window</param>
        /// <param name="fileFilters">Allowed File Extensions, example: "*.txt"</param>
        /// <param name="resultFileName">User selected file path</param>
        /// <returns>false, if user cancelled selection</returns>
        bool TryOpenFileDialog(string defaultDirectory,IEnumerable<FileDialogFilter> fileFilters, out string resultFileName);

        /// <summary>
        /// Open specific files, starting at the default location (My Documents)
        /// </summary>
        /// <param name="fileFilters">Allowed File Extensions, example: "*.txt"</param>
        /// <param name="resultFileName">User selected file path</param>
        /// <returns>false, if user cancelled selection</returns>
        bool TryOpenFileDialog(IEnumerable<FileDialogFilter> fileFilters, out string resultFileName);

        /// <summary>
        /// Open any file, starting selection dialog at the given directory
        /// </summary>
        /// <param name="defaultDirectory">Initial directory of the dialog window</param>
        /// <param name="resultFileName">User selected file path</param>
        /// <returns>false, if user cancelled selection</returns>
        bool TryOpenFileDialog(string defaultDirectory, out string resultFileName);

        /// <summary>
        /// Open any file, starting at "My Documents"
        /// </summary>
        /// <param name="resultFileName">User selected file path</param>
        /// <returns>false, if user cancelled selection</returns>
        bool TryOpenFileDialog(out string resultFileName);
        

        /// <summary>
        /// Select multiple files starting selection dialog from the given initial directory
        /// </summary>
        /// <param name="defaultDirectory">Initial folder of the dialog window</param>
        /// <param name="fileFilters">Allowed File Extensions, example: "*.txt"</param>
        /// <param name="resultFileNames">The selected files</param>
        /// <returns>false, if user cancelled selection</returns>
        bool TryOpenMultiFileDialog(string defaultDirectory, IEnumerable<FileDialogFilter> fileFilters, out string[] resultFileNames);

        /// <summary>
        /// Select multiple files starting from the default directory (My Documents)
        /// </summary>
        /// <param name="fileFilters">Allowed File Extensions, example: "*.txt"</param>
        /// <param name="resultFileNames">The selected files</param>
        /// <returns>false, if user cancelled selection</returns>
        bool TryOpenMultiFileDialog(IEnumerable<FileDialogFilter> fileFilters, out string[] resultFileNames);

        /// <summary>
        /// Select multiple files starting selection dialog from the given initial directory
        /// </summary>
        /// <param name="defaultDirectory">Initial folder of the dialog window</param>
        /// <param name="resultFileNames">The selected files</param>
        /// <returns>false, if user cancelled selection</returns>
        bool TryOpenMultiFileDialog(string defaultDirectory, out string[] resultFileNames);

        /// <summary>
        /// Select multiple files of any file type starting selection at "My Documents"
        /// </summary>
        /// <param name="resultFileNames">The selected files</param>
        /// <returns>false, if user cancelled selection</returns>
        bool TryOpenMultiFileDialog(out string[] resultFileNames);

        /// <summary>
        /// Opens a file save dialog and returns true if a filename was set by the user
        /// </summary>
        /// <param name="defaultDirectory">Initial directory for the dialog</param>
        /// <param name="defaultName">Suggested name of the file in the dialog</param>
        /// <param name="filter">File extension file</param>
        /// <param name="resultFileName">result file name</param>
        bool TrySaveFileDialog(string defaultDirectory, string defaultName, FileDialogFilter fileFilter, out string resultFileName);


        /// <summary>
        /// Opens a file save dialog and returns true if a filename was set by the user
        /// </summary>
        /// <param name="defaultName">Suggested name of the file in the dialog</param>
        /// <param name="filter">File extension file</param>
        /// <param name="resultFileName">result file name</param>
        /// <returns></returns>
        bool TrySaveFileDialog(string defaultName, FileDialogFilter filter, out string resultFileName);

        /// <summary>
        /// Trys to select a folder from a folder browser dialog
        /// </summary>
        bool TryOpenFolderDialog(out string folderName);

        /// <summary>
        /// Creates a progress handling object for your tasks and opens a progress window
        /// </summary>
        /// <param name="popupTitle">Window Title</param>
        /// <param name="headline">Window Headline</param>
        /// <param name="minimumProgress">usually 0</param>
        /// <param name="maximumProgress">usually 1 (or 100)</param>
        /// <returns></returns>
        ProgressHandling ShowProgressPopup(double minimumProgress, double maximumProgress, string popupTitle, string headline);

    }
}
