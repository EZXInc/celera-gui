using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EZXWPFLibrary.Helpers;

namespace AutoFilterDataGrid.ViewModel
{
    public class ShowHideColumnManagerVM : ViewModelBase
    {
        ObservableCollection<string> availableItems;
        public ObservableCollection<string> AvailableItems
        {
            get { return availableItems; }
            set 
            { 
                availableItems = value; 
                this.RaisePropertyChanged(vm => vm.AvailableItems); 
            }
        }

        ObservableCollection<string> visibleItems;
        public ObservableCollection<string> VisibleItems
        {
            get { return visibleItems; }
            set 
            { 
                visibleItems = value; 
                this.RaisePropertyChanged(vm => vm.VisibleItems); 
            }
        }

        private string selectedAvailableItem;
        public string SelectedAvailableItem
        {
            get { return selectedAvailableItem; }
            set
            {
                selectedAvailableItem = value;
                this.RaisePropertyChanged(vm => vm.SelectedAvailableItem);
                UpdateCommandState();
            }
        }



        private string selectedVisibleItem;
        public string SelectedVisibleItem
        {
            get { return selectedVisibleItem; }
            set
            {
                selectedVisibleItem = value;
                this.RaisePropertyChanged(vm => vm.SelectedVisibleItem);
                UpdateCommandState();
            }
        }

        private int selectedAvailableIndex
        {
            get
            {
                return AvailableItems.IndexOf(SelectedAvailableItem);
            }
        }

        private int selectedVisibleIndex
        {
            get
            {
                return VisibleItems.IndexOf(SelectedVisibleItem);
            }
        }

        public ICommand MoveRightCommand { get; private set; }
        public ICommand MoveLeftCommand { get; private set; }
        public ICommand MoveUpCommand { get; private set; }
        public ICommand MoveDownCommand { get; private set; }

        public ShowHideColumnManagerVM()
        {
            this.MoveRightCommand = new DelegateCommand(MoveRight, CanMoveRight);
            this.MoveLeftCommand = new DelegateCommand(MoveLeft, CanMoveLeft);
            this.MoveUpCommand = new DelegateCommand(MoveUp, CanMoveUp);
            this.MoveDownCommand = new DelegateCommand(MoveDown, CanMoveDown);
        }

        public void Init(IEnumerable<string> availableItems, IEnumerable<string> visibleItems)
        {
            this.AvailableItems = new ObservableCollection<string>(availableItems);
            this.VisibleItems = new ObservableCollection<string>(visibleItems);
        }

        public bool CanMoveRight(object param)
        {
            return !string.IsNullOrEmpty(SelectedAvailableItem);
        }
        public bool CanMoveLeft(object param)
        {
            return !string.IsNullOrEmpty(SelectedVisibleItem);
        }
        public bool CanMoveUp(object param)
        {
            return !string.IsNullOrEmpty(SelectedVisibleItem) && selectedVisibleIndex > 0;
        }
        public bool CanMoveDown(object param)
        {
            return !string.IsNullOrEmpty(SelectedVisibleItem) && selectedVisibleIndex < VisibleItems.Count - 1;
        }

        public void MoveRight(object parameter)
        {
            string item = SelectedAvailableItem;
            int movedItemIndex = selectedAvailableIndex;
            AvailableItems.RemoveAt(selectedAvailableIndex);
            //if there isn't any selected item in visible list insert at last
            if (selectedVisibleIndex < 0)
            {
                VisibleItems.Add(item);
            }
            else
            {
                VisibleItems.Insert(selectedVisibleIndex + 1, item);
            }

            //select moved item in target list
            SelectedVisibleItem = item;
            //select next available item
            //if next item available selct that item
            if (movedItemIndex < AvailableItems.Count)
            {
                SelectedAvailableItem = AvailableItems[movedItemIndex];
            }
            // if next item not available select previous item
            else if (movedItemIndex == AvailableItems.Count && movedItemIndex > 0)
            {
                SelectedAvailableItem = AvailableItems[movedItemIndex - 1];
            }
            UpdateCommandState();

        }
        public void MoveLeft(object parameter)
        {
            string item = SelectedVisibleItem;
            int movedItemIndex = selectedVisibleIndex;
            VisibleItems.RemoveAt(selectedVisibleIndex);
            AvailableItems.Add(item);
            //select moved item in target list
            SelectedAvailableItem = item;
            //select next available item
            //if next item available selct that item
            if (movedItemIndex < VisibleItems.Count)
            {
                SelectedVisibleItem = VisibleItems[movedItemIndex];
            }
            // if next item not available select previous item
            else if (movedItemIndex == VisibleItems.Count && movedItemIndex > 0)
            {
                SelectedVisibleItem = VisibleItems[movedItemIndex - 1];
            }

            UpdateCommandState();
        }
        public void MoveUp(object parameter)
        {
            VisibleItems.Move(selectedVisibleIndex, selectedVisibleIndex - 1);
            UpdateCommandState();
        }
        public void MoveDown(object parameter)
        {
            VisibleItems.Move(selectedVisibleIndex, selectedVisibleIndex + 1);
            UpdateCommandState();
        }

        private void UpdateCommandState()
        {
            MoveRightCommand.CanExecute(null);
            MoveLeftCommand.CanExecute(null);
            MoveUpCommand.CanExecute(null);
            MoveDownCommand.CanExecute(null);
        }

    }
}
