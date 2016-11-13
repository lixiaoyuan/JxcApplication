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
using ApplicationDb.Cor.Business;
using DevExpress.Xpf.Core;

namespace JxcApplication
{
    /// <summary>
    /// ModiftyPassword.xaml 的交互逻辑
    /// </summary>
    public partial class ModiftyPassword : DevExpress.Xpf.Core.DXWindow
    {
        public ModiftyPassword()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextEditUserName.Text) || string.IsNullOrWhiteSpace(TextEditPassword.Password) || string.IsNullOrWhiteSpace(TextEditNewPassword.Password) || TextEditPassword.Password == TextEditNewPassword.Password)
            {
                DXMessageBox.Show("输入规范，密码为空或重复!");
                return;
            }
            SystemAccountManager userManager= SystemAccountManager.Create();
            var result = userManager.ModiftyPassword(TextEditUserName.Text, TextEditPassword.Password,
                TextEditNewPassword.Password);
            if (result)
            {
                DXMessageBox.Show("修改成功!");
            }
            else
            {
                DXMessageBox.Show("修改失败!");
            }
        }
    }
}
