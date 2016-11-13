using BusinessDb.Cor.Helper;

namespace BusinessDb.Cor
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using BusinessDb.Cor.EntityModels;
    public partial class ApplicationDbContext : DbContext
    {
#if DEBUG
        public ApplicationDbContext()
            : base("data source=.;initial catalog=Application_First;user id=sa;password=123456;MultipleActiveResultSets=True;App=EntityFramework")
        {
        }
#else
        public ApplicationDbContext()
            : base(ConnectStringHelper.ConnectString)
        {
          
        }
#endif 

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountRecord> AccountRecord { get; set; }
        public virtual DbSet<Acontact> Acontact { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Charge> Charge { get; set; }
        public virtual DbSet<ChargeDetails> ChargeDetails { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerAccountRecord> CustomerAccountRecord { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductInStorage> ProductInStorage { get; set; }
        public virtual DbSet<ProductInStorageDetail> ProductInStorageDetail { get; set; }
        public virtual DbSet<ProductOutInStorageDetail> ProductOutInStorageDetail { get; set; }
        public virtual DbSet<ProductOutStorage> ProductOutStorage { get; set; }
        public virtual DbSet<ProductOutStorageDetail> ProductOutStorageDetail { get; set; }
        public virtual DbSet<ProductReturnInStorage> ProductReturnInStorage { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<GenerateOrder> GenerateOrder { get; set; }
        public virtual DbSet<OrderType> OrderType { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<RefundType> RefundType { get; set; }
        public virtual DbSet<BusinessDb.Cor.EntityModels.TransactionType> TransactionType { get; set; }
        public virtual DbSet<Expenses> Expenses { get; set; }
        public virtual DbSet<ExpensesDetail> ExpensesDetail { get; set; }
        public virtual DbSet<CostType> CostType { get; set; }
        public virtual DbSet<Wage> Wage { get; set; }
        public virtual DbSet<WageDetail> WageDetail { get; set; }
        public DbSet<ProductAsType> ProductAsType { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Account
            //AccountRecord
            //Acontact
            //Area
            //Charge
            //ChargeDetails
            //Customer
            //CustomerAccountRecord
            //Product
            //ProductInStorage
            //ProductInStorageDetail
            //ProductOutInStorageDetail
            //ProductOutStorage
            //ProductOutStorageDetail
            //ProductReturnInStorage
            //Store
            //Suppliers
            //GenerateOrder
            //OrderType
            //ProductType
            modelBuilder.Configurations.AddFromAssembly(this.GetType().Assembly);
            
        }



    }
}
