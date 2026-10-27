using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace ProyectoTaller6.Migrations
{
    /// <inheritdoc />
    public partial class AgregarColorAVentasYCarrito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "CarritoDetalles",
                type: "longtext",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "ProductoVariantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "longtext", nullable: false),
                    Talle = table.Column<string>(type: "longtext", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoVariantes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductoVariantes");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "VentaDetalles");

           

            migrationBuilder.DropColumn(
                name: "Color",
                table: "CarritoDetalles");
        }
    }
}
