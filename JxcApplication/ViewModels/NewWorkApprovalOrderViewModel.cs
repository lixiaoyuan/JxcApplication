using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.EntityModels;
using ApplicationDb.Cor.Model;
using BusinessDb.Cor.Business;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.LayoutControl;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{
    [POCOViewModel]
    public class NewWorkApprovalOrderViewModel : ViewModelTabItem
    {
        public NewWorkApprovalOrderViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
            ButtonSubmitCommand = new DelegateCommand<object>(ButtonSubmit);
        }

        public static ObservableCollection<ForleaveType> ForleaveTypes
        {
            get { return ForleaveTypeManager.QueryLookup(); }
        }

        public virtual ObservableCollection<WorkApproval> Tiles { get; set; }
        public DelegateCommand<object> ButtonSubmitCommand { get; set; }
        public virtual int SelectIndex { get; set; }
        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            Tiles = WorkApprovalManager.QueryLookUp();
        }

        private async void ButtonSubmit(object comit)
        {
            if (comit == null)
            {
                return;
            }
            var type = comit.GetType();
            var prop = type.GetProperty("WorkApproval");
            var workapproval = prop.GetValue(comit, null) as WorkApproval;
            if (workapproval == null)
            {
                return;
            }
            var formData = type.GetProperty("FromData").GetValue(comit, null);
            if (formData == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(await AddWorkApprovalOrderAsync(workapproval, formData)))
            {
                ShowNotification("提交成功!");
                SelectIndex = -1;
                RaisePropertyChanged("SelectIndex");
            }
            else
                ShowNotification("提交失败!");
        }

        //public async void TileClicked(object o)
        //{
        //    var tile = o as Tile;
        //    if (tile == null)
        //        return;
        //    var clickApproval = tile.DataContext as WorkApproval;
        //    if ((clickApproval == null) || string.IsNullOrWhiteSpace(clickApproval.FormDataTemplate) ||
        //        string.IsNullOrWhiteSpace(clickApproval.Name) || string.IsNullOrWhiteSpace(clickApproval.DataType))
        //        return;
        //    var dataType = Type.GetType(clickApproval.DataType);
        //    if (dataType == null)
        //        return;
        //    var data = Activator.CreateInstance(dataType);
        //    var diag = CreateDialogWindow(clickApproval.FormDataTemplate, data, "提交", $"{clickApproval.Name}审批申请");
        //    var result = diag.ShowDialog();
        //    if (!result.HasValue || !result.Value)
        //        return;
        //    if (string.IsNullOrWhiteSpace(await AddWorkApprovalOrderAsync(clickApproval, data)))
        //        ShowNotification("提交成功!");
        //    else
        //        ShowNotification("提交失败!");
        //}

        private Task<string> AddWorkApprovalOrderAsync(WorkApproval workApproval, object data)
        {
            return Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    var approvalOrder = new WorkApprovalOrder
                    {
                        Id = Guid.NewGuid(),
                        WorkApprovalId = workApproval.Id,
                        UserId = App.GlobalApp.LoginUser.Id,
                        UserName = App.GlobalApp.LoginUser.Name,
                        OrderStatusType = WorkApprovalOrderStatus.Underway.ToString(),
                        OrderResultType = WorkApprovalOrderResult.Approvaling.ToString(),
                        StartingTime = DBUnit.GetDbTime()
                    };
                    using (var ms = new MemoryStream())
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(ms, data);
                        approvalOrder.FormData = ms.ToArray();
                    }
                    WorkApprovalOrderManager.InserOrder(approvalOrder);
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                return null;
            });
        }
    }
}