namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSystemUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemUser", "HealthCertificateExpired", c => c.DateTime());
            AddColumn("dbo.SystemUser", "IdCard", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.SystemUser", "Education", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.SystemUser", "School", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.SystemUser", "Address", c => c.String(maxLength: 300, unicode: false));
            AddColumn("dbo.SystemUser", "Remark", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemUser", "Remark");
            DropColumn("dbo.SystemUser", "Address");
            DropColumn("dbo.SystemUser", "School");
            DropColumn("dbo.SystemUser", "Education");
            DropColumn("dbo.SystemUser", "IdCard");
            DropColumn("dbo.SystemUser", "HealthCertificateExpired");
        }
    }
}
