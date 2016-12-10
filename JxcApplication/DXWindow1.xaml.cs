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
            this.DataContext= new TestDataContent();
        }
    }

    public class TestDataContent
    {
        public string Name { get; set; }

        //private 
    }

    public class Data
    {
        public string LastName { get; set; }
    }
}
