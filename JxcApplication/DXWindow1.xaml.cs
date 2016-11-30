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
using DevExpress.Xpf.Core;


namespace JxcApplication
{
    /// <summary>
    /// Interaction logic for DXWindow1.xaml
    /// </summary>
    public partial class DXWindow1 : DXWindow
    {
        public DXWindow1()
        {
            InitializeComponent();
            this.Loaded += DXWindow1_Loaded;
        }

        private void DXWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            List<TestData> datas=new  List<TestData>();
            datas.Add(new TestData() { Name = "收件箱"});
            datas.Add(new TestData() { Name = "发件箱"});
            datas.Add(new TestData() { Name = "回收站"});
            datas.Add(new TestData() { Name = "草稿箱"});
            ListBoxEditBox.ItemsSource = datas;
        }
    }

    public class TestData
    {
        public string Name { get; set; }
    }
}
