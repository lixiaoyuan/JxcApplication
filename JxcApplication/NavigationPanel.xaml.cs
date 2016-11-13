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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;

namespace JxcApplication
{
    /// <summary>
    /// NavigationPanel.xaml 的交互逻辑
    /// </summary>
    public partial class NavigationPanel : UserControl
    {
        public NavigationPanel()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ParentWindowProperty = DependencyProperty.Register(
            "ParentWindow", typeof(MainWindow), typeof(NavigationPanel), new PropertyMetadata(default(MainWindow)));

        public MainWindow ParentWindow
        {
            get { return (MainWindow)GetValue(ParentWindowProperty); }
            set { SetValue(ParentWindowProperty, value); }
        }
    }
}
