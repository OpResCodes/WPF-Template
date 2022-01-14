using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace UserInterfaceTemplate.Infrastructure.BaseModels
{
    /// <summary>
    /// Collection-class to bind to ListBox, DataGrid etc.
    /// </summary>
    public class SelectableItem : ObservableObject
    {
        protected bool _isEnabled;
        protected bool _isSelected;
        protected object _data;

        public SelectableItem(object data) : this(data,false,true) { }

        public SelectableItem(object data, bool isSelected) : this(data, isSelected, true) { }

        public SelectableItem(object data, bool isSelected, bool isEnabled)
        {
            Data = data;
            _isEnabled = isEnabled;
            _isSelected = isSelected;
        }

        /// <summary>
        /// Called if enabled state changed
        /// </summary>
        public event EventHandler IsEnabledChanged;

        /// <summary>
        /// Called if selection changed
        /// </summary>
        public event EventHandler IsSelectedChanged;

        /// <summary>
        /// The original data object
        /// </summary>
        public object Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        /// <summary>
        /// True if the item is enabled
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value) return;
                SetProperty(ref _isEnabled, value);
                IsEnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// True if the item is selected
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                SetProperty(ref _isSelected, value);
                IsSelectedChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        public override string ToString()
        {
            return Data.ToString() ?? "empty";
        }
    }
}
