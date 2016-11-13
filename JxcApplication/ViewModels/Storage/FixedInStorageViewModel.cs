using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BusinessDb.Cor.Business;
using BusinessDb.Cor;
using BusinessDb.Cor.EntityModels;

namespace JxcApplication.ViewModels.Storage
{

    public class FixedInStorageViewModel : ProductInBaseViewModel
    {
        private AcontactManager _acontactManager;
        public override string Title { get { return "固定资产入库单"; } }

        /// <summary>
        /// 联系人列表
        /// </summary>
        public ObservableCollection<Acontact> AcontactsList { get; set; }


        protected override string ProductType()
        {
            //固定资产
            return "GZ";
        }

        protected override string OrderType()
        {
            //固定资产入库
            return "GR";
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            _acontactManager= AcontactManager.Create();
            AcontactsList = _acontactManager.QueAcontacts();
        }

        protected override void InitNewOrder()
        {
            base.InitNewOrder();
            ProductInStorage.StorageId =null;
            ProductInStorage.BuyTime = DBUnit.GetDbTime();
            ProductInStorage.WarrantyExpirationTime = ProductInStorage.BuyTime.Value.AddMonths(6);
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
            if (ProductInStorage.BuyTime>ProductInStorage.WarrantyExpirationTime)
            {
                ShowNotification("购买日期不能大于保修日期!","失败:");
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
            }
            return true;

        }

        public FixedInStorageViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}
