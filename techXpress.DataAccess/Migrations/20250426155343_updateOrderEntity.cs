using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace techXpress.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientPhoneNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientPhoneNumber",
                table: "Orders");
        }
    }
}
