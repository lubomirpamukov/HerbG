using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Herbg.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductsAndCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: "https://www.istockphoto.com/photo/basil-sage-dill-and-thyme-herbs-gm598931180-102776863?searchscope=image%2Cfilm");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagePath",
                value: "https://www.istockphoto.com/photo/fresh-herbs-on-wooden-background-gm504069254-81682909?searchscope=image%2Cfilm");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagePath",
                value: "https://www.istockphoto.com/photo/two-cups-of-healthy-herbal-tea-with-mint-cinnamon-dried-gm545799832-98496663?searchscope=image%2Cfilm");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagePath",
                value: "https://www.istockphoto.com/photo/dried-herbs-and-essential-oils-gm546775666-98703909?searchscope=image%2Cfilm");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: "https://www.istockphoto.com/photo/assortment-of-dry-tea-in-glass-bowls-on-wooden-surface-gm622039222-108837373?searchscope=image%2Cfilm");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagePath",
                value: "https://www.istockphoto.com/photo/herbal-oil-and-lavender-flowers-gm585048326-100237719?searchscope=image%2Cfilm");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagePath",
                value: "https://www.istockphoto.com/photo/fresh-rosemary-herb-on-a-wooden-background-top-view-rosemary-with-copy-space-cooking-gm1309541100-399220748?searchscope=image%2Cfilm");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagePath",
                value: "https://www.istockphoto.com/photo/basil-in-a-clay-pot-gm535913985-57380182?searchscope=image%2Cfilm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Categories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: "/images/categories/medical-herb-category.jpg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagePath",
                value: "/images/categories/culinary-category.jpg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagePath",
                value: "/images/categories/herbal-teas.jpg");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagePath",
                value: "/images/categories/aromatherapy-herbs.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: "/images/products/herbal-tea-mix.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagePath",
                value: "/images/products/lavender-oil.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagePath",
                value: "/images/products/rosmery-pack.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagePath",
                value: "/images/products/basil-plant.jpg");
        }
    }
}
