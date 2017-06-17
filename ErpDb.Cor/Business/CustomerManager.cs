using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using BusinessDb.Cor.EntityModels;
using Utilities;

namespace BusinessDb.Cor.Business
{
    public class CustomerManager
    {
        private ApplicationDbContext _entities;

        private CustomerManager()
        {
            _entities = new ApplicationDbContext();
        }

        public static CustomerManager Create()
        {
            return new CustomerManager();
        }

        public static Customer Find(Guid id)
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                return db.Customer.Find(id);
            }
        }
        [Obsolete]
        public Customer GetCustomerInfo(Guid id, ref decimal balance, ref decimal credibility, ref string giveAddress,
            ref string giveArea, ref string acontackTel, ref Guid acontackId, ref string customAcontactName,
            ref string customAcontactTel, ref Guid responsibleSalesman)
        {
            var firstOrDefault = _entities.Customer.AsNoTracking().First(a => a.Id == id);
            if (firstOrDefault != null)
            {
                if (firstOrDefault.Balance != null)
                    balance = firstOrDefault.Balance.Value;
                else
                    balance = 0;
                if (firstOrDefault.Credibility != null) credibility = firstOrDefault.Credibility.Value;
                else
                    credibility = 0;
                if (firstOrDefault.AcontactId != null) acontackId = firstOrDefault.AcontactId.Value;
                giveAddress = firstOrDefault.Address;
                acontackTel = firstOrDefault.Tel;
                giveArea = firstOrDefault.Area;
                if (!string.IsNullOrWhiteSpace(firstOrDefault.AcontactName))
                    customAcontactName = firstOrDefault.AcontactName;
                if (!string.IsNullOrWhiteSpace(firstOrDefault.AcontactTel))
                    customAcontactTel = firstOrDefault.AcontactTel;
                if (firstOrDefault.ResponsibleSalesman.HasValue)
                    responsibleSalesman = firstOrDefault.ResponsibleSalesman.Value;
                return firstOrDefault;
            }
            return null;
        }

        public static ObservableCollection<Customer> QueryLookUp()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Customer.Where(a => a.Enable.HasValue && a.Enable.Value).ToObservableCollection();
            }
        }

        public static decimal GetCustomerAllOrderPrice(Guid customerId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.GetCustomerAllOrderPrice(customerId);
            }
        }
        public ObservableCollection<Customer> QueCustomers()
        {
            _entities = new ApplicationDbContext();
            _entities.Customer.Load();
            return _entities.Customer.Local;
        }
        /// <summary>
        /// 只显示业务员负责的客户
        /// </summary>
        /// <param name="salesmanUserId"></param>
        /// <returns></returns>
        public ObservableCollection<Customer> QueCustomers(Guid responsibleSalesmanId)
        {
            _entities = new ApplicationDbContext();
            _entities.Customer.Where(a => a.ResponsibleSalesman == responsibleSalesmanId).Load();
            return _entities.Customer.Local;
        }
        /// <summary>
        ///     是否有未保存数据
        /// </summary>
        /// <returns></returns>
        public bool UnSave()
        {
            foreach (var customer in _entities.Customer)
            {
                var temCus = _entities.Entry(customer);
                if ((temCus.State != EntityState.Unchanged) && (temCus.State != EntityState.Detached))
                    return true;
            }
            return false;
        }

        public bool Save()
        {
            return _entities.SaveChanges() > 0;
        }

        public void Remove(Customer suppliers)
        {
            _entities.Customer.Remove(suppliers);
        }

        public void Remove(IEnumerable<Customer> supplierses)
        {
            _entities.Customer.RemoveRange(supplierses);
        }

        //public Customer Find(Guid id)
        //{
        //    return _entities.Customer.Find(id);
        //}

        public string GetMaxCode()
        {
            return _entities.Customer.Max(a => a.Code);
        }
    }
}