using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Herbg.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductsAndCategories1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: "https://media.istockphoto.com/id/598931180/photo/basil-sage-dill-and-thyme-herbs.jpg?s=1024x1024&w=is&k=20&c=QgnLxS6TDDPoh_bbVAVqaXTe5TbyjIFge9sxSGSA__s=");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagePath",
                value: "https://media.istockphoto.com/id/504069254/photo/fresh-herbs-on-wooden-background.jpg?s=1024x1024&w=is&k=20&c=TdPOhT3xUMRt03AzvSfo8NKgzKusHbXMJv9Omw7zenw=");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagePath",
                value: "https://media.istockphoto.com/id/545799832/photo/two-cups-of-healthy-herbal-tea-with-mint-cinnamon-dried.jpg?s=1024x1024&w=is&k=20&c=kRpRimF1ufgUaXyl-wcFkQKvnU4YbMFwtpqKlFhG9oM=");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagePath",
                value: "https://media.istockphoto.com/id/546775666/photo/dried-herbs-and-essential-oils.jpg?s=1024x1024&w=is&k=20&c=4AK88NpTGMeqCViwoizSxc0B4Wr4nIsxia9hkboaA3M=");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: "https://media.istockphoto.com/id/622039222/photo/assortment-of-dry-tea-in-glass-bowls-on-wooden-surface.jpg?s=1024x1024&w=is&k=20&c=4ggjpaDqyMDatq_O6q59BsCFs7VFmk9YDysYr7KDcRY=");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagePath",
                value: "https://media.istockphoto.com/id/585048326/photo/herbal-oil-and-lavender-flowers.jpg?s=1024x1024&w=is&k=20&c=jNZn3EevtNDr53UcPFilreSd1LOPyK0h5q784h9J8Ns=");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagePath",
                value: "https://media.istockphoto.com/id/1309541100/photo/fresh-rosemary-herb-on-a-wooden-background-top-view-rosemary-with-copy-space-cooking-concept.jpg?s=1024x1024&w=is&k=20&c=rw7K_nYTr-64KBmWlFZ6UrWuGrvV2-88kccf-SG8JwE=m");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagePath",
                value: "https://media.istockphoto.com/id/535913985/photo/basil-in-a-clay-pot.jpg?s=1024x1024&w=is&k=20&c=GnxsfcEjKTGdOUEfHvq1E2Pr8ZBcYiu-n0GABgGSyNA=");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
