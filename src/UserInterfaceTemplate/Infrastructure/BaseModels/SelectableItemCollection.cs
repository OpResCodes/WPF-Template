using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace UserInterfaceTemplate.Infrastructure.BaseModels
{
    /// <summary>
    /// Collection of <seealso cref="SelectableItem"/>.
    /// A Helperclass to create a collection of selectable items and tracking the selected items
    /// Hilfsklasse zum Erstellen der Auflistung und Nachverfolgen der ausgewählten Elemente
    /// </summary>
    public class SelectableItemCollection : ObservableCollection<SelectableItem>
    {
        public SelectableItemCollection(IEnumerable<SelectableItem> items)
        {
            foreach (var si in items)
            {
                Add(si);
            }
        }

        public SelectableItemCollection(IEnumerable<object> dataObjects)
        {
            foreach (var dataObj in dataObjects)
            {
                Add(new SelectableItem(dataObj));
            }
        }

        public SelectableItemCollection() : base() { }

        /// <summary>
        /// Factory-Methode zum Erstellen der Auflistung
        /// </summary>
        /// <param name="items">Liste der Datenobjekte</param>
        /// <returns>Liste der SelectableItems</returns>
        public static SelectableItemCollection FromItems(IEnumerable items)
        {
            var coll = new SelectableItemCollection();
            foreach (var dataObject in items)
            {
                coll.Add(new SelectableItem(dataObject));
            }
            return coll;
        }

        private void ItemSelectionChanged(object sender, EventArgs e)
        {
            SelectionChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItems)));
            base.OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedItems)));
        }

        public event PropertyChangedEventHandler SelectionChanged;

        protected override void InsertItem(int index, SelectableItem item)
        {
            base.InsertItem(index, item);
            item.IsSelectedChanged += ItemSelectionChanged;
        }

        protected override void RemoveItem(int index)
        {
            this[index].IsSelectedChanged -= ItemSelectionChanged;
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (var selectableItem in this)
            {
                selectableItem.IsSelectedChanged -= ItemSelectionChanged;
            }
            base.ClearItems();
        }

        /// <summary>
        /// Returns alls selected items
        /// </summary>
        public IEnumerable<SelectableItem> SelectedItems
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (base[i].IsSelected)    
                        yield return base[i];
                }
            }
        }

        /// <summary>
        /// Returns the data contained in the selected items
        /// </summary>
        public IEnumerable<object> SelectedDataObjects
        {
            get
            {
                return SelectedItems.Select(item => item.Data);
            }
        }
    }
}
