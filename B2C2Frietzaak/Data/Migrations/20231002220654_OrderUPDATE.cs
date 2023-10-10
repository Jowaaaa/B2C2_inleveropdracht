using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B2C2Frietzaak.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderUPDATE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Total",
                table: "Orders",
                type: "real",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Orders");

        }
    }
}
