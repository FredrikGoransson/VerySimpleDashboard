using FluentMigrator;

namespace VerySimpleDashboard.Data.SqlStorage.Migrations
{
    [Migration(20140925001)]
    // ReSharper disable once InconsistentNaming
    public class _20140925_001_InitialDatabase : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn("Name").AsString().Nullable();

            Create.Table("Project")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn("Name").AsString().Nullable()
                .WithColumn("UserId").AsGuid().ForeignKey("ProjectToUserFK", "User", "Id");

            Create.Table("Table")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn("Name").AsString().Nullable()
                .WithColumn("ProjectId").AsGuid().ForeignKey("TableToProjectFK", "Project", "Id");

            Create.Table("DataRow")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey().WithDefault(SystemMethods.NewGuid)
                .WithColumn("Data").AsBinary().Nullable()
                .WithColumn("TableId").AsGuid().ForeignKey("DataRowToTableFK", "Table", "Id");
        }

        public override void Down()
        {
            Delete.Table("DataRow");
            Delete.Table("Table");
            Delete.Table("Project");
            Delete.Table("User");
        }
    }
}
