using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace techXpress.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderTableAddSessionIdAndPayamentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Payments",
                newName: "PaymentId");

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Payments",
                newName: "Id");
        }
    }
}
