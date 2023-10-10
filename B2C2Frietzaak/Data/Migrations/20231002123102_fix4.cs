using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B2C2Frietzaak.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "CartItems");

            migrationBuilder.AddColumn<int>(
                name: "CartItemId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CartItemId",
                table: "Products",
                column: "CartItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CartItems_CartItemId",
                table: "Products",
                column: "CartItemId",
                principalTable: "CartItems",
                principalColumn: "CartItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CartItems_CartItemId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CartItemId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CartItemId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
