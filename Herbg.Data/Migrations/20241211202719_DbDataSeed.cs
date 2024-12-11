using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Herbg.Data.Migrations
{
    /// <inheritdoc />
    public partial class DbDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShippingInformationZip",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: "https://media.istockphoto.com/id/955162416/photo/various-leaves-of-tea-and-spices-on-wooden-background.jpg?s=1024x1024&w=is&k=20&c=nGcLCxUl-mWMwpQWnngE9fC4AgzcQYho8FcjMsu0Ta8=");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagePath",
                value: "https://media.istockphoto.com/id/610584404/photo/eco-craft-christmas-gift-boxes.jpg?s=1024x1024&w=is&k=20&c=HmXz1r7xDQBg83Mvcqn9roHR-3G5LzqHz5OfE_QqDB0=");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Basil Plant");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImagePath", "IsDeleted", "ManufactorerId", "Name", "Price" },
                values: new object[,]
                {
                    { 5, 4, "Eucalyptus Essential Oil: Known for its refreshing scent and healing properties, it aids in relaxation, focus, and relieving respiratory issues.", "https://media.istockphoto.com/id/1053121442/photo/eucalyptus-essential-oil.jpg?s=1024x1024&w=is&k=20&c=94_16e0iLljAy_9fFWKUQa_6DhCM5MYCv1FmtrSnKIU=", false, 2, "Eucalyptus Essential Oil", 15.99m },
                    { 6, 3, "Peppermint Tea: A refreshing herbal tea known for its soothing properties that help with digestion and reduce stress.", "https://media.istockphoto.com/id/1355250220/photo/dry-herbal-green-tea-in-a-plate-with-cup-of-tea-and-eucalyptus-leaves-top-view-bright.jpg?s=1024x1024&w=is&k=20&c=06yP4sIe09ceFdG9nzGqCtEa54QqJAt1QogPZwED3X4=", false, 2, "Peppermint Tea", 11.49m },
                    { 7, 3, "Chamomile Tea: A calming herbal tea made from chamomile flowers, perfect for winding down before bed and promoting better sleep.", "https://media.istockphoto.com/id/1134246421/photo/cup-of-chamomile-tea-with-tea-bag.jpg?s=1024x1024&w=is&k=20&c=sHiQcQ0RND7zkmcXsbcAgnUIXBRG1ax54f0zzwJQ8ZM=", false, 3, "Chamomile Tea", 12.50m },
                    { 8, 4, "Aloe Vera Gel: Known for its soothing and healing properties, ideal for skincare, sunburns, and hydration.", "https://media.istockphoto.com/id/1215011574/photo/aloe-vera-gel-close-up-sliced-aloe-vera-plants-leaf-and-gel-with-wooden-spoon.jpg?s=1024x1024&w=is&k=20&c=fm8eVWQIF53Q0-zRZTMgXB3Ve3D2_vA1JpbYwlU5-hk=", false, 1, "Aloe Vera Gel", 19.99m },
                    { 9, 2, "Turmeric Powder: A powerful antioxidant and anti-inflammatory spice, perfect for adding flavor to dishes or making turmeric tea.", "https://media.istockphoto.com/id/965503302/photo/turmeric-powder-and-roots.jpg?s=1024x1024&w=is&k=20&c=U3hFkU4b8ODTK_9o4kMYEQbfW_JGC5-FbFd4uZQaHSE=", false, 3, "Turmeric Powder", 8.99m },
                    { 10, 4, "Rosemary Essential Oil: A versatile oil for aromatherapy, known for its energizing, memory-boosting, and stress-relieving benefits.", "https://media.istockphoto.com/id/589137972/photo/small-bottle-of-burdock-extract.jpg?s=1024x1024&w=is&k=20&c=vH22VbCqRIPBstTDj6n5vuKjIICEiIxRf7fgFZZ9lmw=", false, 3, "Rosemary Essential Oil", 13.99m },
                    { 11, 1, "Sage Bundle: Dried sage leaves for smudging and cleansing, offering spiritual and emotional benefits for relaxation and focus.", "https://media.istockphoto.com/id/1460937551/photo/smoldering-white-sage-smudge-stick.jpg?s=1024x1024&w=is&k=20&c=agi9O7Z28qVwfViQBdyJA20okfFw62_30RszMAv-tHA=", false, 2, "Sage Bundle", 9.99m },
                    { 12, 3, "Lemongrass Tea: A refreshing, citrusy tea made from lemongrass, known for its ability to promote digestion and reduce anxiety.", "https://media.istockphoto.com/id/546792864/photo/thai-herbal-drinks-lemon-grass.jpg?s=1024x1024&w=is&k=20&c=Mz-14vRpNlWawyOF3BtwS-gDrpgvF-Y1id9-wcigFRc=", false, 1, "Lemongrass Tea", 11.25m },
                    { 13, 2, "Cinnamon Stick Pack: A bundle of dried cinnamon sticks, perfect for brewing in tea, adding to cooking, or for aromatic purposes.", "https://media.istockphoto.com/id/1214648653/photo/cinnamon-sticks-with-rope-isolated-on-white-background.jpg?s=1024x1024&w=is&k=20&c=et2CT61Hat3vgIhQ1ScDtiRszza7khjNXkACwLmCPcg=", false, 1, "Cinnamon Stick Pack", 6.50m },
                    { 14, 4, "Aromatherapy Diffuser: Enhance your space with the calming effects of essential oils. This ultrasonic diffuser disperses oils for a soothing atmosphere, perfect for relaxation, focus, and stress relief.", "https://media.istockphoto.com/id/1366419259/photo/humidifier-with-steam-moisturizing-air-at-home.jpg?s=1024x1024&w=is&k=20&c=0N59shgTzPOYmnLtHcdw8MbSMh2ta792-mi6TOPKYv8=", false, 2, "Aromatherapy Diffuser", 20.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.AlterColumn<string>(
                name: "ShippingInformationZip",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagePath",
                value: "https://media.istockphoto.com/id/622039222/photo/assortment-of-dry-tea-in-glass-bowls-on-wooden-surface.jpg?s=1024x1024&w=is&k=20&c=4ggjpaDqyMDatq_O6q59BsCFs7VFmk9YDysYr7KDcRY=");

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
                column: "Name",
                value: "Basil plant");
        }
    }
}
