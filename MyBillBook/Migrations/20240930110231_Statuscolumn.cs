using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBillBook.Migrations
{
    /// <inheritdoc />
    public partial class Statuscolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "sales",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "sales");
        }
    }
}
