using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BusinessDb.Cor.Business;
using DevExpress.Xpf.Grid;
using BusinessDb.Cor;

namespace JxcApplication.ViewModels.Storage
{
    public class ProductInStorageViewModel : ProductInBaseViewModel
    {
        public override string Title { get { return "成品入库单"; } }
        
        protected override string ProductType()
        {
            return "CP";
        }

        protected override string OrderType()
        {
            return "CR";
        }

        protected override void OnInitializeInRuntime()
        {
            StoreManager = StoreManager.Create();
            StoresLookUp = StoreManager.QueSellStores();
            base.OnInitializeInRuntime();
        }

        protected override void InitNewOrder()
        {
            base.InitNewOrder();
            ProductInStorage.ProducingDate = DBUnit.GetDbTime();
       
            RaisePropertyChanged("ProductInStorage");
            GC.Collect();
        }

        protected override bool SaveProductOrderBefor()
        {
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


            ProductInStorage.StatusFlag = 0;
            if (IsNewOrder)
            {
                ProductInStorage.Id = Guid.NewGuid();
                ProductInStorage.Code = ProductOrderManager.GetNewOrderCode(OrderType());
                ProductInStorage.OrderType = OrderType();
                ProductInStorage.CreteUserId= App.GlobalApp.LoginUser.Id;
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
            foreach (var detail in Details)
            {
                detail.OrderType = OrderType();
                if (IsNewOrder|| detail.Id == Guid.Empty)
                {
                    detail.Id = Guid.NewGuid();
                }
                detail.PutInId = ProductInStorage.Id;
                detail.StorageId = ProductInStorage.StorageId;
                detail.LastStock = detail.OriginalStock;
                ProductInStorage.SumPrice += detail.SumPrice;
            }

            return true;
        }

        public ProductInStorageViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}
