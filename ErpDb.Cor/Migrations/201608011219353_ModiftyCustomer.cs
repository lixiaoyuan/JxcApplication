namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModiftyCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "AcontactName", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.Customer", "AcontactTel", c => c.String(maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "AcontactTel");
            DropColumn("dbo.Customer", "AcontactName");
        }
    }
}
