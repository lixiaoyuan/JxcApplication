namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addForleaveType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForleaveType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 30, unicode: false),
                        Name = c.String(maxLength: 100, unicode: false),
                        Sort = c.Int(nullable: false),
                        Enable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ForleaveType");
        }
    }
}
