using Microsoft.EntityFrameworkCore.Migrations;

namespace Leiflix.Migrations
{
    public partial class ok : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fav",
                table: "Filme");

            migrationBuilder.DropColumn(
                name: "Viewed",
                table: "Filme");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "Fav",
                table: "Filme",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Viewed",
                table: "Filme",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
