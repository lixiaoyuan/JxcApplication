namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResponsibleSalesman : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "ResponsibleSalesman", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "ResponsibleSalesman");
        }
    }
}
