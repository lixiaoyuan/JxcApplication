namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSystemUserRemind : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemUser", "RemindBirthday", c => c.Boolean());
            AddColumn("dbo.SystemUser", "RemindHealthCertificateExpired", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemUser", "RemindHealthCertificateExpired");
            DropColumn("dbo.SystemUser", "RemindBirthday");
        }
    }
}
