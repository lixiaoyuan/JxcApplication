using System;
using System.Windows;
using System.Windows.Controls;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Models;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using JxcApplication.Core;

namespace JxcApplication.Control
{
    /// <summary>
    ///     OrderBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class OrderInBrowser : UserControl
    {
        public enum OrderInBrowserShowType
        {
            InOrder,
            ReturnInOrder,
            OutOrder,
            ChargeOrder,
            ExpensesOrder
        }

        public static RoutedEvent ShowOrderEvent = EventManager.RegisterRoutedEvent("ShowOrder", RoutingStrategy.Bubble,
            typeof(EventHandler<ShowOrderEventArgs>), typeof(OrderInBrowser));

        private ProductOrderManager _productOrderManager;
        private bool _dateChanged;
        public OrderInBrowser()
        {
            InitializeComponent();
            Loaded += OrderInBrowser_Loaded;
        }

        public string OrderType { get; set; }
        public OrderInBrowserShowType ShowType { get; set; }

        public event RoutedEventHandler ShowOrder
        {
            add { AddHandler(ShowOrderEvent, value); }
            remove { RemoveHandler(ShowOrderEvent, value); }
        }

        private void OrderInBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            _productOrderManager = ProductOrderManager.Create();
            DateEdit.DateTime = DateTime.Now;
        }

        private void DateEdit_OnEditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            if (!_dateChanged)
            {
                _dateChanged = true;
                return;
            }
            switch (ShowType)
            {
                case OrderInBrowserShowType.InOrder:
                    if (string.IsNullOrWhiteSpace(OrderType))
                        return;
                    GridControlOrderBrowser.ItemsSource = _productOrderManager.GetHistoryInOrder(OrderType,
                        (DateTime) e.NewValue);
                    break;
                case OrderInBrowserShowType.ChargeOrder:
                    GridControlOrderBrowser.ItemsSource =
                        _productOrderManager.GetHistoryChargeOrder((DateTime) e.NewValue);
                    break;
                case OrderInBrowserShowType.ExpensesOrder:
                    if (string.IsNullOrWhiteSpace(OrderType))
                        return;
                    GridControlOrderBrowser.ItemsSource = _productOrderManager.GetHistoryExpensesOrder(OrderType,
                        (DateTime) e.NewValue);
                    break;
                case OrderInBrowserShowType.OutOrder:
                    if (string.IsNullOrWhiteSpace(OrderType))
                        return;
                    GridControlOrderBrowser.ItemsSource = _productOrderManager.GetHistoryOutOrder(OrderType,
                        (DateTime) e.NewValue);
                    break;
                case OrderInBrowserShowType.ReturnInOrder:
                    if (string.IsNullOrWhiteSpace(OrderType))
                        return;
                    GridControlOrderBrowser.ItemsSource = _productOrderManager.GetHistoryReturnInOrder(OrderType,
                        (DateTime) e.NewValue);
                    break;
            }
        }

        private void GridControlOrderBrowser_OnSelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            var orderBrowser = (OrderBrowser) e.NewItem;
            RaiseEvent(new ShowOrderEventArgs(orderBrowser.Id,this));
        }

        private void TableViewOrderBrowser_OnRowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            var tableView = (TableView) e.Source;
            if (e.HitInfo.InRow)
            {
                var orderBrowser = (OrderBrowser) tableView.Grid.GetRow(e.HitInfo.RowHandle);
                RaiseEvent(new ShowOrderEventArgs(orderBrowser.Id,this));
            }
        }
    }
}