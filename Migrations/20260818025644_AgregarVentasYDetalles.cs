using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoTaller6.Migrations
{
    /// <inheritdoc />
    public partial class AgregarVentasYDetalles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_Clientes_ClienteId",
                table: "Ventas");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioRed",
                table: "Clientes",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "RedSocial",
                table: "Clientes",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalles_ProductoId",
                table: "VentaDetalles",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_UsuarioRed_RedSocial",
                table: "Clientes",
                columns: new[] { "UsuarioRed", "RedSocial" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VentaDetalles_Productos_ProductoId",
                table: "VentaDetalles",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_Clientes_ClienteId",
                table: "Ventas",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VentaDetalles_Productos_ProductoId",
                table: "VentaDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_Clientes_ClienteId",
                table: "Ventas");

            migrationBuilder.DropIndex(
                name: "IX_VentaDetalles_ProductoId",
                table: "VentaDetalles");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_UsuarioRed_RedSocial",
                table: "Clientes");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioRed",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "RedSocial",
                table: "Clientes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_Clientes_ClienteId",
                table: "Ventas",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
