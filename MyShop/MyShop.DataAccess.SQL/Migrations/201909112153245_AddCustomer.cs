namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "UserId", c => c.String());
            DropColumn("dbo.Customers", "UnserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "UnserId", c => c.String());
            DropColumn("dbo.Customers", "UserId");
        }
    }
}
