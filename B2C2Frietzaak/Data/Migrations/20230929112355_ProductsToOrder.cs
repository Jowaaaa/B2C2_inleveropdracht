using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B2C2Frietzaak.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductsToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderItems",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderItems",
                table: "Orders");
        }
    }
}
