using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B2C2Frietzaak.Data.Migrations
{
    /// <inheritdoc />
    public partial class orderChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Orders_OrderId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_OrderId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Cart");

            migrationBuilder.AddColumn<float>(
                name: "Total",
                table: "Orders",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_OrderId",
                table: "CartItems",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                table: "CartItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Orders_OrderId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_OrderId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "CartItems");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Cart",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cart_OrderId",
                table: "Cart",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Orders_OrderId",
                table: "Cart",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");
        }
    }
}
