using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestWpfAsync
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button b=sender as Button;
            if (b==null)
            {
                return;
            }
            b.Content = "已单击";
            await TestAsync();
            b.Content = "按钮";
            MessageBox.Show("5秒完成");
        }

        private Task<bool> TestAsync()
        {
            return Task<bool>.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return false;
            });
        } 
    }
}
