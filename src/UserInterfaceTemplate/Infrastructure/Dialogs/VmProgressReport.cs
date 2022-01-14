using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading;

namespace UserInterfaceTemplate.Infrastructure.Dialogs
{
    /// <summary>
    /// ViewModel to bind against a progress report view
    /// </summary>
    internal class VmProgressReport : ObservableObject, IChildWindowHelper
    {
        private readonly CancellationTokenSource _cts;
        private readonly Progress<double> _progress;
        private bool _showCancelButton = false;
        private string _headlineText = "Work in Progress...";
        private double _minimum = 0.0;
        private double _maximum = 1.0;
        private double _currentProgress = 0.0;

        public event EventHandler<RequestCloseEventArgs> RequestCloseDialog;

        public VmProgressReport(double min, double max, Progress<double> progressReport, CancellationTokenSource cancellationTokenSource = null)
        {
            Minimum = min;
            Maximum = max;
            _cts = cancellationTokenSource;
            _progress = progressReport;
            ShowCancelButton = (_cts != null);
            progressReport.ProgressChanged += ProgressReport_ProgressChanged;
            CancelCommand = new RelayCommand(OnCancel, CanExecuteCancel);
        }
        
        public bool ShowCancelButton
        {
            get => _showCancelButton;
            set => SetProperty(ref _showCancelButton, value);
        }

        public RelayCommand CancelCommand { get; set; }

        public string HeadlineText
        {
            get => _headlineText;
            set => SetProperty(ref _headlineText, value);
        }

        public double Minimum
        {
            get => _minimum;
            set => SetProperty(ref _minimum, value);
        }
        
        public double Maximum
        {
            get => _maximum;
            set => SetProperty(ref _maximum, value);
        }
                
        public double CurrentProgress
        {
            get => _currentProgress;
            set
            {
                SetProperty(ref _currentProgress, value);
                if(_currentProgress >= _maximum)
                    CloseDialog(true);                
            }
        }

        private void OnCancel()
        {
            HeadlineText = "Cancelling...";
            if (_cts != null)
            {
                _cts.Cancel();
                CloseDialog(false);
            }
        }

        private bool CanExecuteCancel()
        {
            return (_cts != null);
        }

        public void CloseDialog(bool withResult = false)
        {
            _progress.ProgressChanged -= ProgressReport_ProgressChanged;
            RequestCloseDialog?.Invoke(this, new RequestCloseEventArgs(withResult));
        }

        public void WindowRequestsClose(object sender, RequestCloseEventArgs e)
        {
            CloseDialog(e.DialogResult);
        }

        private void ProgressReport_ProgressChanged(object sender, double e)
        {
            this.CurrentProgress = e;
        }

    }
}
