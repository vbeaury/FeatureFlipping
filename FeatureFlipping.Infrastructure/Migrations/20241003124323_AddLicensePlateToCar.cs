using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeatureFlipping.Migrations
{
    /// <inheritdoc />
    public partial class AddLicensePlateToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LicensePlate",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicensePlate",
                table: "Cars");
        }
    }
}
