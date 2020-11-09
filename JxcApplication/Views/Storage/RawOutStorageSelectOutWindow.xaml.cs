using BusinessDb.Cor;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.EntityModels;
using BusinessDb.Cor.Models;
using DevExpress.Mvvm.Native;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
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

namespace JxcApplication.Views.Storage
{
    public partial class RawOutStorageSelectOutWindow : Window
    {
        private class Data
        {
            /// <summary>
            /// 入库单号
            /// </summary>
            public Guid InStorageId { get; set; }

            /// <summary>
            /// 出库单号id，未保存到数据库
            /// </summary>
            public Guid OutStorageId { get; set; }

            /// <summary>
            /// 出库产品名称
            /// </summary>
            public string OutProductName { get; set; }

            /// <summary>
            /// 出库产品规格
            /// </summary>
            public string OutProductSpec { get; set; }

            /// <summary>
            /// 该出库单需要出库的总数量
            /// </summary>
            public decimal OutStock { get; set; }

            /// <summary>
            /// 生产日期
            /// </summary>
            public DateTime? OutProductTime { get; set; }

            /// <summary>
            /// 入库单剩余数量
            /// </summary>
            public decimal InLastStock { get; set; }

            /// <summary>
            /// 扣除数量
            /// </summary>
            public decimal UseQuantity { get; set; }

            public string Name
            {
                get
                {
                    return string.Format("名称:{0},规格:{1}", OutProductName, OutProductSpec);
                }
            }
        }

        private ApplicationDbContext _entities;

        /// <summary>
        /// 是否需要重新计算
        /// </summary>
        private bool _needReCacul = false;

        public static RawOutStorageSelectOutWindow Create()
        {
            return new RawOutStorageSelectOutWindow();
        }

        private RawOutStorageSelectOutWindow()
        {
            InitializeComponent();

        }

        public string InsertProductInOrder(ProductOutStorage productOutStorage, ObservableCollection<ProductOutStorageDetail> details)
        {
            try
            {
                _entities.ProductInStorageDetail.Detacheds(_entities);
                _entities.ProductOutInStorageDetail.Detacheds(_entities);

                DateTime nowDateTime = DBUnit.GetDbTime();
                string ip = global::Utilities.Utilities.GetLocalIpAddress();

                GridControlMain.ItemsSource = null;
                ObservableCollection<Data> tmpSource = new ObservableCollection<Data>();

                foreach (ProductOutStorageDetail detail in details)
                {
                    if (detail.ProductId == null || detail.ProductId.Value == Guid.Empty)
                    {
                        return string.Format("开单失败:{0} 产品Id为空!", detail.ProductCode);
                    }
                    var storageDetails = _entities.ProductInStorageDetail
                        .Include(x => x.ProductInStorage)
                        .Where(a => a.StorageId == productOutStorage.StorageId && a.ProductId == detail.ProductId && a.LastStock > 0)
                        .OrderBy(a => a.ProductInStorage.ProducingDate).Select(x => new { data = x, time = x.ProductInStorage.ProducingDate }).ToList();

                    #region 判断库存,或去当前仓库当前产品的剩余数量

                    var sumStock = storageDetails.Sum(a => a.data.LastStock);
                    if (sumStock < detail.OutStock)
                    {
                        return String.Format("开单失败:{0} 库存不足!", detail.ProductCode);
                    }

                    #endregion

                    decimal needStock = detail.OutStock;
                    foreach (var inStorageDetail in storageDetails)
                    {
                        Data item = new Data();
                        item.InStorageId = inStorageDetail.data.Id;
                        item.OutStorageId = detail.Id;
                        item.OutProductName = detail.ProductName;
                        item.OutProductSpec = detail.ProductSpecification;
                        item.OutStock = detail.OutStock;
                        item.OutProductTime = inStorageDetail.time;
                        item.InLastStock = inStorageDetail.data.LastStock;
                        tmpSource.Add(item);

                        if (needStock > 0)
                        {
                            //库存扣除调整
                            if ((inStorageDetail.data.LastStock >= needStock))
                            {
                                item.UseQuantity = needStock;
                                needStock = 0;
                            }
                            else
                            {
                                item.UseQuantity = inStorageDetail.data.LastStock;
                                needStock -= inStorageDetail.data.LastStock;
                                inStorageDetail.data.LastStock = 0;
                            }
                        }
                    }
                    //判断库存是否足够扣完
                    if (needStock > 0)
                    {
                        return String.Format("开单失败:{0} 尝试扣库存失败!", detail.ProductCode);
                    }

                }

                GridControlMain.ItemsSource = tmpSource;
                var res = ShowDialog();
                if (res.HasValue && res.Value)
                {
                    _entities.ProductInStorageDetail.Detacheds(_entities);
                    _entities.ProductOutInStorageDetail.Detacheds(_entities);

                    var source = GridControlMain.ItemsSource as ObservableCollection<Data>;
                    List<Guid> lockedList = new List<Guid>();

                    foreach (var x in source.Where(x => x.UseQuantity > 0))
                    {
                        var inDetail = _entities.ProductInStorageDetail.First(s => s.Id == x.InStorageId);
                        if (x.UseQuantity > inDetail.LastStock)
                        {
                            return "保存失败，请重试!";
                        }
                        inDetail.LastStock -= x.UseQuantity;
                        //记录出入库对照明细
                        ProductOutInStorageDetail addOutInStorage = new ProductOutInStorageDetail()
                        {
                            Id = Guid.NewGuid(),
                            PutInStorageId = x.InStorageId,
                            PutOutStorageId = x.OutStorageId,
                            OriginalStock = x.InLastStock,
                            SubtractStock = x.UseQuantity,
                            LasetStock = inDetail.LastStock,
                            CreateUserId = productOutStorage.CreateUserId,
                            CreateDate = nowDateTime,
                            CreateIp = ip
                        };
                        _entities.ProductOutInStorageDetail.Add(addOutInStorage);

                        //记录已扣库存的入库主单或者退货入库主单
                        //入库单
                        if (inDetail.PutInId.Value != Guid.Empty)
                        {
                            if (!lockedList.Contains(inDetail.PutInId.Value))
                            {
                                lockedList.Add(inDetail.PutInId.Value);
                            }
                        }
                    }
                    // 保存数据
                    var tran = _entities.Database.BeginTransaction();
                    try
                    {
                        if (lockedList.Any())
                        {
                            //更新入库单标记为已扣库存
                            _entities.Database.ExecuteSqlCommand($"UPDATE dbo.[ProductInStorage] SET StatusFlag=1 WHERE Id IN('{string.Join("','", lockedList)}')");
                        }
                        //更新主记录的订单总金额
                        productOutStorage.SumPrice = details.Sum(x => x.SumPrice);
                        _entities.SaveChanges();
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        return "保存失败: "+e.Message;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                GC.Collect();
            }
        }


        public CategoriesOrder<ProductOutStorage, ProductOutStorageDetail> GetInsertRawOutOrder()
        {

            _entities = new ApplicationDbContext();
            ProductOutStorage productOutStorage = new ProductOutStorage() { Id = Guid.NewGuid() };
            _entities.ProductOutStorage.Add(productOutStorage);
            _entities.ProductOutStorage.Where(a => a.Id == productOutStorage.Id).Load();
            _entities.ProductOutStorageDetail.Where(a => a.PutOutId == productOutStorage.Id).Load();
            return new CategoriesOrder<ProductOutStorage, ProductOutStorageDetail>()
            {
                MasterStorage = productOutStorage,
                Details = _entities.ProductOutStorageDetail.Local
            };
        }

        private void gridDragDropManager_DragOver(object sender, DevExpress.Xpf.Grid.DragDrop.GridDragOverEventArgs e)
        {
            //e.ShowDragInfo = false;
            if (e.DraggedRows.Count == 0)
            {
                return;
            }

            var row = e.DraggedRows[0] as Data;
            var targetRow = e.TargetRow as Data;

            if (row == null || targetRow == null)
            {
                e.AllowDrop = false;
                return;
            }

            e.AllowDrop = row.OutStorageId == targetRow.OutStorageId;


        }

        private void gridDragDropManager_Drop(object sender, DevExpress.Xpf.Grid.DragDrop.GridDropEventArgs e)
        {
            if (e.DraggedRows.Count == 0)
            {
                return;
            }

            var row = e.DraggedRows[0] as Data;
            var targetRow = e.TargetRow as Data;

            if (row == null || targetRow == null)
            {
                e.Handled = true;
                return;
            }

            e.Handled = !(row.OutStorageId == targetRow.OutStorageId );
            _needReCacul = !e.Handled;
        }

        private void gridDragDropManager_Dropped(object sender, DevExpress.Xpf.Grid.DragDrop.GridDroppedEventArgs e)
        {
            try
            {
                GridControlMain.BeginDataUpdate();

                if (!_needReCacul) return;
                _needReCacul = false;


                var row = e.TargetRow as Data;
                if (row == null)
                {
                    return;
                }
                var source = GridControlMain.ItemsSource as ObservableCollection<Data>;
                if (source == null)
                {
                    return;
                }

                _entities.ProductInStorageDetail.Detacheds(_entities);
                _entities.ProductOutInStorageDetail.Detacheds(_entities);

                var adjustData = source.Where(x => x.OutStorageId == row.OutStorageId).ToList();

                if (adjustData == null || adjustData.Count == 0)
                {
                    return;
                }
                var total = adjustData.First().OutStock;

                foreach (var i in adjustData)
                {
                    if (total <= 0)
                    {
                        i.UseQuantity = 0;
                    }
                    else if (total <= i.InLastStock)
                    {
                        i.UseQuantity = total;
                        total -= i.UseQuantity;
                    }
                    else
                    {
                        i.UseQuantity = i.InLastStock;
                        total -= i.UseQuantity;
                    }
                }

            }
            finally{
                GridControlMain.EndDataUpdate();

            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
