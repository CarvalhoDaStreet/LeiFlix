using Microsoft.EntityFrameworkCore.Migrations;

namespace Leiflix.Migrations
{
    public partial class t : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filme_Sessao_SessaoId",
                table: "Filme");

            migrationBuilder.DropIndex(
                name: "IX_Filme_SessaoId",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "SessaoId",
                table: "Filme");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessaoId",
                table: "Filme",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Filme_SessaoId",
                table: "Filme",
                column: "SessaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Filme_Sessao_SessaoId",
                table: "Filme",
                column: "SessaoId",
                principalTable: "Sessao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
