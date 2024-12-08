using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Herbg.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddShippingInformationToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "AspNetUsers",
                newName: "ShippingInformationAddress");

            migrationBuilder.AddColumn<string>(
                name: "ShippingInformationCity",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingInformationCountry",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingInformationFullName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingInformationZip",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingInformationCity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShippingInformationCountry",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShippingInformationFullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShippingInformationZip",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ShippingInformationAddress",
                table: "AspNetUsers",
                newName: "Address");
        }
    }
}
