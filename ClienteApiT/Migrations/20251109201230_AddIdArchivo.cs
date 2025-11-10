using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClienteApiT.Migrations
{
    /// <inheritdoc />
    public partial class AddIdArchivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ArchivosClientes",
                newName: "IdArchivo");

            migrationBuilder.AddColumn<string>(
                name: "UrlFoto1",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlFoto2",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlFoto3",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlFoto1",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "UrlFoto2",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "UrlFoto3",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "IdArchivo",
                table: "ArchivosClientes",
                newName: "Id");
        }
    }
}
