using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrainHub.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAutorRelationshipToArtigo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AutorId",
                table: "Artigos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.DropColumn(
                name: "Autor",
                table: "Artigos");

            migrationBuilder.CreateIndex(
                name: "IX_Artigos_AutorId",
                table: "Artigos",
                column: "AutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Artigos_Usuarios_AutorId",
                table: "Artigos",
                column: "AutorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artigos_Usuarios_AutorId",
                table: "Artigos");

            migrationBuilder.DropIndex(
                name: "IX_Artigos_AutorId",
                table: "Artigos");

            migrationBuilder.DropColumn(
                name: "AutorId",
                table: "Artigos");

            migrationBuilder.AddColumn<string>(
                name: "Autor",
                table: "Artigos",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
