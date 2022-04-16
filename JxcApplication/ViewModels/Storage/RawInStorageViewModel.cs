using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BusinessDb.Cor.Business;
using BusinessDb.Cor;
using BusinessDb.Cor.EntityModels;
using DevExpress.Xpf.Grid;

namespace JxcApplication.ViewModels.Storage
{
    public class RawInStorageViewModel : ProductInBaseViewModel
    {
        private SuppliersManager _suppliersManager;

        public override string Title { get { return "消耗品入库单"; } }
        
        /// <summary>
        /// 供应商列表
        /// </summary>
        public ObservableCollection<Suppliers> SuppliersList { get; set; }

        protected override string ProductType()
        {
            //原料
            return "YL";
        }

        protected override string OrderType()
        {
            //原料入库
            return "YR";
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            _suppliersManager=SuppliersManager.Create();

            SuppliersList = _suppliersManager.QueSupplierses();
            RaisePropertyChanged("SuppliersList");
        }

        protected override void InitNewOrder()
        {
            base.InitNewOrder();
            ProductInStorage.ProducingDate = DBUnit.GetDbTime();
            //ProductInStorage.StorageId = StoresLookUp.Count > 0 ? StoresLookUp[0].Id : Guid.Empty;
            RaisePropertyChanged("ProductInStorage");
            GC.Collect();

        }

        protected override bool SaveProductOrderBefor()
        {
            //throw new NotImplementedException();
            if (ProductInStorage.StorageId == null || ProductInStorage.StorageId == Guid.Empty)
            {
                ShowNotification("请选择入库仓库!", "失败:");
                return false;
            }
            if (ProductInStorage.ProducingDate == null)
            {
                ShowNotification("请选择生成日期!", "失败:");
                return false;
            }
            if (ProductInStorage.Suppliers == null || ProductInStorage.Suppliers.Value == Guid.Empty)
            {
                ShowNotification("请选择供应商!", "失败:");
                return false;
            }

            ProductInStorage.StatusFlag = 0;
            if (IsNewOrder)
            {
                ProductInStorage.Id = Guid.NewGuid();
                ProductInStorage.Code = ProductOrderManager.GetNewOrderCode(OrderType());
                ProductInStorage.OrderType = OrderType();
                ProductInStorage.CreteUserId = App.GlobalApp.LoginUser.Id;
                ProductInStorage.CreteDate = DBUnit.GetDbTime();
                ProductInStorage.CreteIp = Utilities.Utilities.GetLocalIpAddress();
            }
            else
            {
                ProductInStorage.ModiftyDate = DBUnit.GetDbTime();
                ProductInStorage.ModiftyIp = Utilities.Utilities.GetLocalIpAddress();
                ProductInStorage.ModiftyUserId = App.GlobalApp.LoginUser.Id;

            }
            ProductInStorage.SumPrice = 0;
            short sort = 0;
            foreach (var detail in Details)
            {
                detail.OrderType = OrderType();
                if (IsNewOrder || detail.Id == Guid.Empty)
                {
                    detail.Id = Guid.NewGuid();
                }
                detail.PutInId = ProductInStorage.Id;
                detail.StorageId = ProductInStorage.StorageId;
                detail.LastStock = detail.OriginalStock;
                ProductInStorage.SumPrice += detail.SumPrice;
                detail.SortCode = sort++;
            }
            return true;
        }

        public override void CellValueChanged(CellValueChangedEventArgs e)
        {
            base.CellValueChanged(e);

            var editRow = e.Cell.Row as ProductInStorageDetail;
            if (editRow == null)
            {
                return;
            }

            if (editRow.ProducingDate == null)
            {
                editRow.ProducingDate = DBUnit.GetDbTime();
            }
        }

        public RawInStorageViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}
