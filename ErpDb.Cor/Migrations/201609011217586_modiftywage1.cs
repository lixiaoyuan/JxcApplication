namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modiftywage1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.WageMap", newName: "Wage");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Wage", newName: "WageMap");
        }
    }
}
