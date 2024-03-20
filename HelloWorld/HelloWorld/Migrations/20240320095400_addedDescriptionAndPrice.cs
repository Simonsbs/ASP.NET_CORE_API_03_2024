using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelloWorld.Migrations
{
    /// <inheritdoc />
    public partial class addedDescriptionAndPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Products",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");
        }
    }
}
