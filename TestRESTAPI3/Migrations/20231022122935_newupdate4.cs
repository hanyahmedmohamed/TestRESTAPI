using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestRESTAPI3.Migrations
{
    /// <inheritdoc />
    public partial class newupdate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Items");
        }
    }
}
