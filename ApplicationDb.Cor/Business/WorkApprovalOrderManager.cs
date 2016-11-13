using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDb.Cor.EntityModels;
using ApplicationDb.Cor.Helper;
using ApplicationDb.Cor.Model;
using Utilities;

namespace ApplicationDb.Cor.Business
{
    public class WorkApprovalOrderManager : IDisposable
    {
        private ApplicationDbContext db;

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }

        public static string InserOrder(WorkApprovalOrder order)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    //插入审批队列表
                    var approvalUsers =
                        db.WorkApprovalUser.Where(a => a.WorkApprovalId == order.WorkApprovalId)
                            .OrderBy(a => a.Sort)
                            .Select(a => a.UserId)
                            .ToArray();
                    var sort = 0;
                    foreach (var user in approvalUsers)
                    {
                        db.WorkApprovalOrderUser.Add(new WorkApprovalOrderUser
                        {
                            Id = Guid.NewGuid(),
                            WorkApprovalOrderId = order.Id,
                            UserId = user,
                            ResultType = WorkApprovalOrderResult.Approvaling.ToString(),
                            Sort = sort++
                        });
                    }
                    if (approvalUsers.Any())
                    {
                        order.NextApprovalUserId = approvalUsers[0];
                    }
                    else
                    {
                        //插入抄送队列
                        order.OrderStatusType = WorkApprovalOrderStatus.Completed.ToString();
                        order.OrderResultType = WorkApprovalOrderResult.Agree.ToString();
                        order.StopTime = DBUnit.GetDbTime();
                        var approvalCopyUsers =
                        db.WorkApprovalCopyUser.Where(a => a.WorkApprovalId == order.WorkApprovalId)
                            .OrderBy(a => a.Sort)
                            .Select(a => a.UserId)
                            .ToArray();
                        foreach (var user in approvalCopyUsers)
                        {
                            db.WorkApprovalOrderCopyUser.Add(new WorkApprovalOrderCopyUser()
                            {
                                Id = Guid.NewGuid(),
                                WorkApprovalOrderId = order.Id,
                                UserId = user,IsSeed = false
                            });
                        }
                    }
                    db.WorkApprovalOrder.Add(order);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }


        public static WorkApprovalOrderManager Create()
        {
            return new WorkApprovalOrderManager();
        }

        private void DbInit()
        {
            if (db != null)
            {
                db.Dispose();
                db = new ApplicationDbContext();
            }
            else
            {
                db = new ApplicationDbContext();
            }
        }

        /// <summary>
        /// 待审批
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<WorkApprovalOrder> QueryWaitApprovalOrders(Guid userId,DateTime starTime,DateTime endTime)
        {
            DbInit();
            return
                db.WorkApprovalOrder.Where(a => a.NextApprovalUserId == userId && DbFunctions.DiffDays(starTime, a.StartingTime) >= 0 && DbFunctions.DiffDays(endTime, a.StartingTime) <= 0)
                    .OrderBy(a => a.StartingTime)
                    .ToObservableCollection();
        }
        /// <summary>
        /// 我审批的
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<WorkApprovalOrder> QueryMeApprovalOrders(Guid userId, DateTime starTime, DateTime endTime)
        {
            DbInit();
            return
                db.WorkApprovalOrderUser.Where(a => a.UserId == userId  && a.ResultType != WorkApprovalOrderResult.Approvaling.ToString()).OrderByDescending(a => a.ModiftyTime)
                    .Join(db.WorkApprovalOrder.Where(a=> DbFunctions.DiffDays(starTime, a.StartingTime) >= 0 && DbFunctions.DiffDays(endTime, a.StartingTime) <= 0), a => a.WorkApprovalOrderId, a => a.Id, ((approvalOrderUser, workApprovalOrder) => workApprovalOrder))
                    .ToObservableCollection();
        }
        /// <summary>
        /// 我发起的审批
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<WorkApprovalOrder> QueryMeNewApprovalOrders(Guid userId, DateTime starTime, DateTime endTime)
        {
            DbInit();
            return
                db.WorkApprovalOrder.Where(a => a.UserId == userId && DbFunctions.DiffDays(starTime, a.StartingTime) >= 0 && DbFunctions.DiffDays(endTime, a.StartingTime) <= 0)
                    .OrderByDescending(a => a.StartingTime)
                    .ToObservableCollection();

        }

        /// <summary>
        /// 抄送我的
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<WorkApprovalOrder> QueryCopyMeWorkApprovalOrders(Guid userId, DateTime starTime, DateTime endTime, bool isSeed)
        {
            DbInit();
            return
                db.WorkApprovalOrder.Where(a=> DbFunctions.DiffDays(starTime, a.StartingTime) >= 0 && DbFunctions.DiffDays(endTime, a.StartingTime) <= 0)
                                        .Join(db.WorkApprovalOrderCopyUser.Where(a => a.UserId == userId && a.IsSeed.HasValue && a.IsSeed == isSeed),
                                        a => a.Id,
                                        b => b.WorkApprovalOrderId, 
                                        (a, b) => a).ToObservableCollection();
        } 

        public  Task<string> AddedResultsAsync(WorkApprovalOrder workApprovalOrder, Guid userId,WorkApprovalOrderResult result)
        {
            return Task<string>.Factory.StartNew(() =>
            {
                var tran=db.Database.BeginTransaction();
                try
                {
                    DateTime dbTime = DBUnit.GetDbTime();

                    if (!workApprovalOrder.NextApprovalUserId.HasValue)
                    {
                        tran.Rollback();
                        return "当前审批已完成!";
                    }
                    if (result == WorkApprovalOrderResult.Goback)
                    {//撤销
                        workApprovalOrder.NextApprovalUserId = null;
                        workApprovalOrder.OrderResultType = WorkApprovalOrderResult.Goback.ToString();
                        workApprovalOrder.OrderStatusType = WorkApprovalOrderStatus.Completed.ToString();
                        workApprovalOrder.StopTime = dbTime;
                        db.SaveChanges();
                        tran.Commit();
                        return null;
                    }

                    if (workApprovalOrder.NextApprovalUserId != userId)
                    {
                        tran.Rollback();
                        return "您不是当前审批人员！";
                    }

                    //保存当前记录结果
                    var approvalOrderUser = db.WorkApprovalOrderUser.FirstOrDefault(
                        a => a.WorkApprovalOrderId == workApprovalOrder.Id && a.UserId == userId);
                    if (approvalOrderUser != null)
                    {
                        approvalOrderUser.ResultType = result.ToString();
                        approvalOrderUser.ModiftyTime = dbTime;
                    }
                    db.SaveChanges();

                    if (result == WorkApprovalOrderResult.Refuse)
                    {
                        //拒绝，结束审批进程
                        workApprovalOrder.NextApprovalUserId = null;
                        workApprovalOrder.OrderResultType = WorkApprovalOrderResult.Refuse.ToString();
                        workApprovalOrder.OrderStatusType = WorkApprovalOrderStatus.Completed.ToString();
                        workApprovalOrder.StopTime = dbTime;
                    }
                    else
                    {
                        //同意
                        var nextUser = db.WorkApprovalOrderUser.Where(a => a.WorkApprovalOrderId == workApprovalOrder.Id 
                                                                        && a.ResultType == WorkApprovalOrderResult.Approvaling.ToString())
                           .OrderBy(a => a.Sort)
                           .FirstOrDefault();
                        if (nextUser != null)
                            workApprovalOrder.NextApprovalUserId = nextUser.UserId;
                        else
                        {
                            //通过，结束审批进程，抄送
                            workApprovalOrder.NextApprovalUserId = null;
                            workApprovalOrder.OrderStatusType = WorkApprovalOrderStatus.Completed.ToString();
                            workApprovalOrder.OrderResultType = WorkApprovalOrderResult.Agree.ToString();
                            workApprovalOrder.StopTime = dbTime;

                            var copyDic =
                                db.WorkApprovalCopyUser.Where(a => a.WorkApprovalId == workApprovalOrder.WorkApprovalId).OrderBy(a=>a.Sort)
                                    .ToList();
                            foreach (WorkApprovalCopyUser copyUserDic in copyDic)
                            {
                                db.WorkApprovalOrderCopyUser.Add(new WorkApprovalOrderCopyUser()
                                {
                                    Id = Guid.NewGuid(),
                                    WorkApprovalOrderId = workApprovalOrder.Id,
                                    UserId = copyUserDic.UserId,
                                    IsSeed = false,
                                    ModiftyTime = null
                                });
                            }
                        }
                    }
                    db.SaveChanges();
                    tran.Commit();
                    return null;
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    return e.Message;
                }
            });
        }

        /// <summary>
        /// 标记为已读
        /// </summary>
        /// <returns></returns>
        public Task<string> AddedRead(WorkApprovalOrder workApprovalOrder, Guid userId)
        {
            return Task<string>.Factory.StartNew(() =>
            {
                var tran = db.Database.BeginTransaction();
                try
                {
                    var result =
                        db.WorkApprovalOrderCopyUser.FirstOrDefault(
                            a => a.WorkApprovalOrderId == workApprovalOrder.Id && a.UserId == userId);
                    if (result == null)
                    {
                        tran.Rollback();
                        return "不存在此审批，请刷新!";
                    }
                    if (result.IsSeed.HasValue && result.IsSeed.Value)
                    {
                        tran.Rollback();
                        return "此审批已阅，请刷新!";
                    }
                    result.IsSeed = true;
                    result.ModiftyTime = DBUnit.GetDbTime();
                    db.SaveChanges();
                    tran.Commit();
                    return null;
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    return e.Message;
                }
            });
        }
    }
}