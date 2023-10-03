using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B2C2Frietzaak.Data.Migrations
{
    /// <inheritdoc />
    public partial class sauceOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SauceId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_SauceId",
                table: "OrderItems",
                column: "SauceId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Sauces_SauceId",
                table: "OrderItems",
                column: "SauceId",
                principalTable: "Sauces",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Sauces_SauceId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_SauceId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "SauceId",
                table: "OrderItems");
        }
    }
}
