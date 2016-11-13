using System;
using System.Collections.ObjectModel;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.EntityModels;
using BusinessDb.Cor.Models.Report;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using JxcApplication.ViewModels.Inherit;

namespace JxcApplication.ViewModels
{
    [POCOViewModel]
    public class LastStoreQueViewModel : ViewModelTabItem
    {
        protected StoreManager StoreManager;
        private ReportManager _reportManager;


        public Guid? StorageId { get; set; }
        /// <summary>
        ///     仓库下拉列表
        /// </summary>
        public ObservableCollection<Store> StoresLookUp { get; set; }

        public ObservableCollection<ProductStock> ProductStocks { get; set; }


        protected override void OnInitializeInRuntime()
        {
            StorageId = null;
            if (StoreManager == null)
            {
                StoreManager = StoreManager.Create();
                StoresLookUp = StoreManager.QueStores();
            }
            if (_reportManager==null)
            {
                _reportManager=ReportManager.Create();
            }
            base.OnInitializeInRuntime();
        }

        /// <summary>
        ///     仓库选择发生改变
        /// </summary>
        /// <param name="e"></param>
        public void StoreEditValueChanged(EditValueChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                Guid storeId = Guid.Parse(e.NewValue.ToString());
                if (storeId == Guid.Empty)
                {
                    return;
                }
                ProductStocks = _reportManager.QueStoreProductStocks(storeId);
                RaisePropertyChanged("ProductStocks");
            }
        }

        public LastStoreQueViewModel(Guid menuId, string caption) : base(menuId, caption)
        {
        }
    }
}