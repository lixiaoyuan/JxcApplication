using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
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

namespace JxcApplication.Views.Sell
{
    /// <summary>
    /// InputTrackingNumberWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputTrackingNumberWindow : Window
    {
        public InputTrackingNumberWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(trackingNumber.Text))
            {
                DXMessageBox.Show("快递单号不能为空！");
                return;
            }
            this.DialogResult = true;
        }
    }
}
