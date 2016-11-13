namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransactionType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.Short(nullable: false),
                        CodeName = c.String(),
                        Name = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TransactionType");
        }
    }
}
