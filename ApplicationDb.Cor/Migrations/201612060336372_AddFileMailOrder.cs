namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFileMailOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileCabinets",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FilName = c.String(maxLength: 200, unicode: false),
                        FormatType = c.Int(nullable: false),
                        Data = c.Binary(),
                        Size = c.Long(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MailOrder",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        IsDaft = c.Boolean(nullable: false),
                        IsReply = c.Boolean(nullable: false),
                        IsUnread = c.Boolean(nullable: false),
                        ReplyMailId = c.Guid(),
                        FormUser = c.Guid(),
                        FormAddress = c.String(maxLength: 30, unicode: false),
                        FormUserName = c.String(maxLength: 30, unicode: false),
                        ToUser = c.Guid(),
                        ToAddress = c.String(maxLength: 30, unicode: false),
                        ToUserName = c.String(maxLength: 30, unicode: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        Subject = c.String(maxLength: 100, unicode: false),
                        ConetntFileId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MailOrder");
            DropTable("dbo.FileCabinets");
        }
    }
}
