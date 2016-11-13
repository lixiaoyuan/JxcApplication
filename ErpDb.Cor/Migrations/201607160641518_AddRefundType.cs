namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRefundType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RefundType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 20, unicode: false),
                        Name = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RefundType");
        }
    }
}
