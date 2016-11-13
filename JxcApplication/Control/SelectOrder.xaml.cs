using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Models;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using JxcApplication.ViewModels;

namespace JxcApplication.Control
{
    /// <summary>
    ///     Interaction logic for SelectOrder.xaml
    /// </summary>
    public partial class SelectOrder : DXDialogWindow
    {
        public SelectOrder()
        {
            InitializeComponent();
        }
    }

    public class SelectOrderViewModel : ViewModelBaseCor
    {
        private DateTime _lasTime;
        private GridControl _gridControl;
        private ProductOrderManager _productOrderManager;


        public ObservableCollection<SelectChargeOrder> OrderDetails { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public IEnumerable<SelectChargeOrder> SelectItems;
        public Guid CustomId;
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            _productOrderManager = ProductOrderManager.Create();
            _lasTime = DateTime.Now.AddDays(-1);

            var now = DBUnit.GetDbTime();
            StartTime = DateTime.Parse(now.ToShortDateString());
            EndTime = DateTime.Parse(now.AddDays(1).ToShortDateString());
            EndTime = EndTime.AddHours(23 - EndTime.Hour);
            EndTime = EndTime.AddMinutes(59 - EndTime.Minute);
            EndTime = EndTime.AddSeconds(59 - EndTime.Second);


            InitOrderDetails();
        }

        private void InitOrderDetails()
        {
            if (DateTime.Now - _lasTime < TimeSpan.FromMilliseconds(1000)||CustomId==Guid.Empty)
            {
                return;
            }
            _lasTime = DateTime.Now;
            OrderDetails = _productOrderManager.GetOrders(CustomId, StartTime, EndTime);
            RaisePropertyChanged("OrderDetails");
        }

        public void ChangedTime(int i)
        {
            var now = DBUnit.GetDbTime();
            switch (i)
            {
                case 0://今天
                    StartTime = DateTime.Parse(now.ToShortDateString());
                    EndTime = DateTime.Parse(now.ToShortDateString()).AddDays(1).AddMilliseconds(-1);
                    break;
                case 1://昨天
                    StartTime = DateTime.Parse(now.AddDays(-1).ToShortDateString());
                    EndTime = DateTime.Parse(now.ToShortDateString()).AddMilliseconds(-1);
                    break;
                case 2://本周
                    StartTime = DateTime.Parse(now.AddDays(-(double) now.DayOfWeek + 1).ToShortDateString());
                    EndTime = DateTime.Parse(now.AddDays(+(7 - (double) now.DayOfWeek)).ToShortDateString()).AddDays(1).AddMilliseconds(-1);
                    break;
                case 3://上周
                    StartTime = DateTime.Parse(now.AddDays(-(6 + (double) now.DayOfWeek)).ToShortDateString());
                    EndTime = DateTime.Parse(now.AddDays(-(double) now.DayOfWeek).ToShortDateString()).AddDays(1).AddMilliseconds(-1);
                    break;
                case 4://本月
                    StartTime = DateTime.Parse(now.AddDays(-(now.Day - 1)).ToShortDateString());
                    EndTime = DateTime.Parse(now.AddMonths(1).AddDays(-now.Day).ToShortDateString()).AddDays(1).AddMilliseconds(-1);
                    break;
                case 5://上月
                    StartTime = DateTime.Parse(now.AddMonths(-1).AddDays(-(now.Day - 1)).ToShortDateString());
                    EndTime = DateTime.Parse(now.AddDays(-now.Day).ToShortDateString()).AddDays(1).AddMilliseconds(-1);
                    break;
            }
            //EndTime = EndTime.AddHours(23 - EndTime.Hour);
            //EndTime = EndTime.AddMinutes(59 - EndTime.Minute);
            //EndTime = EndTime.AddSeconds(59 - EndTime.Second);
            RaisePropertiesChanged("StartTime", "EndTime");
        }

        public void OnStartTimeChanging(object newTime)
        {
            InitOrderDetails();
        }

        public void OnEndTimeChanging(object newTime)
        {
            InitOrderDetails();
        }

        public void ItemsSourceChanged(GridControl opControl)
        {
            _gridControl = opControl;
            opControl.SelectedItem = null;
        }

        public void ButtonOk(DXDialogWindow rootDialogWindow)
        {
            if (_gridControl == null || _gridControl.SelectedItems == null || _gridControl.SelectedItems.Count == 0)
            {
                rootDialogWindow.DialogResult = true;
                return;
            }
            SelectItems = _gridControl.SelectedItems.Cast<SelectChargeOrder>();
            rootDialogWindow.DialogResult = true;
        }
        public void ButtonCancel(DXDialogWindow rootDialogWindow)
        {
            rootDialogWindow.DialogResult = false;
        }

        public void CustomColumnDisplayText(CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName== "SumPrice" || e.Column.FieldName == "Paid" || e.Column.FieldName == "UnPay")
            {
                e.DisplayText = string.Format("{0:f2}", e.Value);
            }
        }
    }
}