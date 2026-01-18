using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YemekTarifleri.Migrations
{
    /// <inheritdoc />
    public partial class AddHazirlamaSuresiAndZorlukSeviyesi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HazirlamaSuresi",
                table: "Tarifler",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZorlukSeviyesi",
                table: "Tarifler",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HazirlamaSuresi",
                table: "Tarifler");

            migrationBuilder.DropColumn(
                name: "ZorlukSeviyesi",
                table: "Tarifler");
        }
    }
}
