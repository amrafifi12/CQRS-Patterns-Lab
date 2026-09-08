using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleSeparateDb.Api.WriteInfrastructure.Persistence.Migrations.WriteMigrations
{
    /// <inheritdoc />
    public partial class AddOutboxMessage2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AggregateId",
                table: "OutboxMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AggregateId",
                table: "OutboxMessages");
        }
    }
}
