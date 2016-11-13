using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using ApplicationDb.Cor.EntityModels;
using ApplicationDb.Cor.Model;
using Utilities;

namespace ApplicationDb.Cor.Business
{
    public class RibbonNodeManager
    {
        private RibbonNodeManager()
        {
        }

        public static RibbonNodeManager Create()
        {
            return new RibbonNodeManager();
        }

        #region 读取Ribbon菜单
        public  Task<RibbonNodeCollection> GetAuthRibbonAsync( Guid userId,Guid rootRibbonId,string systemId)
        {
            return  Task.Factory.StartNew(() =>
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var data = db.GetUserRibbon(userId, rootRibbonId, systemId).ToObservableCollection();
                    var t= ConvertToNodeCollection(rootRibbonId, new RibbonNodeCollection(), data);
                    return t;
                }
            },CancellationToken.None,TaskCreationOptions.None, TaskScheduler.Default);
        }

        private RibbonNodeCollection ConvertToNodeCollection(Guid? parentId, RibbonNodeCollection collection, ObservableCollection<AuthRibbonNode> nodes)
        {
            if (collection == null)
            {
                collection = new RibbonNodeCollection();
            }
            foreach (RibbonNodeModel node in nodes.Where(a => a.ParentId == parentId))
            {
                collection.Add(node);
                if (node.Children == null)
                {
                    node.Children = new RibbonNodeCollection();
                }
                ConvertToNodeCollection(node.Id, node.Children, nodes);
            }
            return collection;
        }
        #endregion

        #region 维护Ribbon菜单字典

        public Task<ObservableCollection<AuthRibbonNode>> GetAuthRibbonMenuAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                using (ApplicationDbContext db=new ApplicationDbContext())
                {
                     return db.AuthRibbonNode.Where(a => !a.ParentId.HasValue && a.NodeType == RibbonNodeType.RibbonRoot&&a.Enable).OrderBy(a=>a.Sort).ToObservableCollection();
                }
            });
        }

        public Task<ObservableCollection<AuthRibbonNode>> GetMenuRibbonNodeAsync(Guid menudId)
        {
            return Task.Factory.StartNew(() =>
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    return db.GetMenuRibbonNode(menudId).ToObservableCollection();
                }
            });
        }

        #region AuthRibbonMenu

        public Task<string> SaveAuthRibbonMenuSortAsync(ObservableCollection<AuthRibbonNode> data)
        {
            return Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        foreach (AuthRibbonNode node in data)
                        {
                            db.Entry(node).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                return null;
            });
        }

        public Task<string> AddAuthRibbonMenuAsync(AuthRibbonNode data)
        {
            return Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        db.AuthRibbonNode.Add(data);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                return null;
            });
        }

        public Task<string> EditAuthRibbonMenuAsync(AuthRibbonNode data)
        {
            return Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        db.Entry(data).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                return null;
            });
        }

        #endregion


        #endregion
    }
}