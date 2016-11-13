namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModiftyCustomer2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customer", "AcontactName", c => c.String(maxLength: 30, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customer", "AcontactName", c => c.String(maxLength: 20, unicode: false));
        }
    }
}
