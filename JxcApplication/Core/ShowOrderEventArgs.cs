using System;
using System.Windows;
using JxcApplication.Control;

namespace JxcApplication.Core
{
    public class ShowOrderEventArgs : RoutedEventArgs
    {
        public ShowOrderEventArgs(Guid orderId, object source) : base(OrderInBrowser.ShowOrderEvent, source)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; set; }
    }
}