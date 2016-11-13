namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModiftyProductOutStorage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductOutStorage", "AcontackTel", c => c.String(maxLength: 60, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductOutStorage", "AcontackTel", c => c.String(maxLength: 20, unicode: false));
        }
    }
}
