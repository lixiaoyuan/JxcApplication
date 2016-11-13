using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ApplicationDb.Cor;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;


namespace JxcApplication.Control
{
    /// <summary>
    /// Interaction logic for SelectUser.xaml
    /// </summary>
    public partial class SelectUser : DXWindow
    {
        public SelectUser()
        {
            InitializeComponent();
        }
    }

    public class SelectUserViewModel : ViewModelSource
    {
        private GridControl _gridControl;
        public ObservableCollection<SystemUser> SystemUsers { get; set; }
        public IEnumerable<SystemUser> SelectItems { get; set; }

        public DelegateCommand<ItemsSourceChangedEventArgs> ItemSourceChangedCommand { get; set; }
        public DelegateCommand<DXWindow> ButtonOkCommand { get; set; }
        public DelegateCommand<DXWindow> ButtonCancelCommand { get; set; }
        public SelectUserViewModel()
        {
            ItemSourceChangedCommand=new DelegateCommand<ItemsSourceChangedEventArgs>(ItemSourceChanged);
            ButtonOkCommand=new DelegateCommand<DXWindow>(ButtonOk);
            ButtonCancelCommand=new DelegateCommand<DXWindow>(ButtonCancel);
        }

        private void ButtonOk(DXWindow obj)
        {
            if (_gridControl == null)
            {
                obj.DialogResult = false;
                SelectItems=new List<SystemUser>();
                return;
            }
            SelectItems = _gridControl.SelectedItems == null ? new List<SystemUser>() : _gridControl.SelectedItems.Cast<SystemUser>();
            obj.DialogResult = true;
        }

        private void ButtonCancel(DXWindow obj)
        {
            obj.DialogResult = false;
            SelectItems = new List<SystemUser>();
        }

        private void ItemSourceChanged(ItemsSourceChangedEventArgs obj)
        {
            obj.Source.SelectedItem = null;
            _gridControl = obj.Source as GridControl;
        }
    }
}
