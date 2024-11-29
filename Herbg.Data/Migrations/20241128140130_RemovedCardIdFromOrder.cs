using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Herbg.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCardIdFromOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
