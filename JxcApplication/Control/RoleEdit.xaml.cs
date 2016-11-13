using System.Windows;
using DevExpress.Xpf.Core;

namespace JxcApplication.Control
{
    /// <summary>
    /// Interaction logic for RoleEdit.xaml
    /// </summary>
    public partial class RoleEdit : DXWindow
    {
        public RoleEdit()
        {
            InitializeComponent();
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }
    }
}
