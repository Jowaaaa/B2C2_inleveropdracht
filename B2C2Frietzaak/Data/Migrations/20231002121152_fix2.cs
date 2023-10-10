using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B2C2Frietzaak.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Sauces_SauceId1",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sauces",
                table: "Sauces");

            migrationBuilder.DropIndex(
                name: "IX_Products_SauceId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SauceId",
                table: "Sauces");

            migrationBuilder.DropColumn(
                name: "SauceId1",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Sauces",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sauces",
                table: "Sauces",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SauceId",
                table: "Products",
                column: "SauceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Sauces_SauceId",
                table: "Products",
                column: "SauceId",
                principalTable: "Sauces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Sauces_SauceId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sauces",
                table: "Sauces");

            migrationBuilder.DropIndex(
                name: "IX_Products_SauceId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Sauces");

            migrationBuilder.AddColumn<string>(
                name: "SauceId",
                table: "Sauces",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SauceId1",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sauces",
                table: "Sauces",
                column: "SauceId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SauceId1",
                table: "Products",
                column: "SauceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Sauces_SauceId1",
                table: "Products",
                column: "SauceId1",
                principalTable: "Sauces",
                principalColumn: "SauceId");
        }
    }
}
