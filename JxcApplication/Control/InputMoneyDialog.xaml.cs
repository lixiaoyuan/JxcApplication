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
using DevExpress.Xpf.Core;
using JxcApplication.ViewModels;


namespace JxcApplication.Control
{
    /// <summary>
    /// Interaction logic for InputMoneyDialog.xaml
    /// </summary>
    public partial class InputMoneyDialog : DXDialogWindow
    {
        public InputMoneyDialog()
        {
            InitializeComponent();
            TextBoxInputMoney.Focus();
            TextBoxInputMoney.SelectAll();
        }

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
        {
            var valError = Validation.GetErrors(TextBoxInputMoney);
            if (valError.Count > 0)
            {
                var erroeMsg = string.Join("\n", valError.Select(a => a.ErrorContent.ToString()));
                DXMessageBox.Show(erroeMsg);
                return;
            }
            var datacontext = this.DataContext as InputMoneyDialogViewModel;
            if (datacontext == null) 
                return;
            //输入验证
            if (datacontext.ThisSum > datacontext.ThisMoney)
            {
                DXMessageBox.Show("金额输入错误!");
                return;
            }
            DialogResult = true;
            //throw new NotImplementedException();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }

    public class InputMoneyDialogViewModel : ViewModelBaseCor
    {
        /// <summary>
        /// 本次总金额
        /// </summary>
        public decimal ThisSum { get; set; }
        /// <summary>
        /// 客户余额
        /// </summary>
        public decimal CustomerSum { get; set; }
        /// <summary>
        /// 需充值金额
        /// </summary>
        public decimal NeedMoney { get; set; }
        /// <summary>
        /// 本次预交金额
        /// </summary>
        public decimal ThisMoney { get; set; }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            NeedMoney = ThisSum - CustomerSum;
            RaisePropertyChanged("NeedMoney");
        }
    }
}
