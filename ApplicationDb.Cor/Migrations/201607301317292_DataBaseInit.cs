namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataBaseInit : DbMigration
    {
        public override void Up()
        {
           
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthToolButton", "MenuId", "dbo.AuthMenu");
            DropForeignKey("dbo.AuthRoleMenu", "MenuId", "dbo.AuthMenu");
            DropForeignKey("dbo.AuthMenuToolButton", "MenuId", "dbo.AuthMenu");
            DropForeignKey("dbo.AuthMenuToolButton", "ToolButtonId", "dbo.AuthToolButton");
            DropForeignKey("dbo.AuthUserRole", "RoleId", "dbo.AuthRole");
            DropForeignKey("dbo.AuthUserRole", "UserId", "dbo.SystemUser");
            DropForeignKey("dbo.AuthRoleMenu", "RoleId", "dbo.AuthRole");
            DropForeignKey("dbo.AuthMenuToolButton", "RoleId", "dbo.AuthRole");
            DropIndex("dbo.AuthToolButton", new[] { "MenuId" });
            DropIndex("dbo.AuthRoleMenu", new[] { "MenuId" });
            DropIndex("dbo.AuthMenuToolButton", new[] { "MenuId" });
            DropIndex("dbo.AuthMenuToolButton", new[] { "ToolButtonId" });
            DropIndex("dbo.AuthUserRole", new[] { "RoleId" });
            DropIndex("dbo.AuthUserRole", new[] { "UserId" });
            DropIndex("dbo.AuthRoleMenu", new[] { "RoleId" });
            DropIndex("dbo.AuthMenuToolButton", new[] { "RoleId" });
            DropTable("dbo.Zhangtao");
            DropTable("dbo.SystemProgram");
            DropTable("dbo.AuthToolButton");
            DropTable("dbo.SystemUser");
            DropTable("dbo.AuthUserRole");
            DropTable("dbo.AuthRoleMenu");
            DropTable("dbo.AuthRole");
            DropTable("dbo.AuthMenuToolButton");
            DropTable("dbo.AuthMenu");
        }
    }
}
