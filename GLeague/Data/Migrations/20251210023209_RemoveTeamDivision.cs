using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GLeague.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTeamDivision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Divisions_Seasons_SeasonId",
                table: "Divisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Drafts_Divisions_DivisionId",
                table: "Drafts");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Divisions_DivisionId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Divisions_DivisionId",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Divisions",
                table: "Divisions");

            migrationBuilder.RenameTable(
                name: "Divisions",
                newName: "Division");

            migrationBuilder.RenameIndex(
                name: "IX_Divisions_SeasonId",
                table: "Division",
                newName: "IX_Division_SeasonId");

            migrationBuilder.AlterColumn<int>(
                name: "DivisionId",
                table: "Teams",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Division",
                table: "Division",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Division_Seasons_SeasonId",
                table: "Division",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drafts_Division_DivisionId",
                table: "Drafts",
                column: "DivisionId",
                principalTable: "Division",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Division_DivisionId",
                table: "Games",
                column: "DivisionId",
                principalTable: "Division",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Division_DivisionId",
                table: "Teams",
                column: "DivisionId",
                principalTable: "Division",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Division_Seasons_SeasonId",
                table: "Division");

            migrationBuilder.DropForeignKey(
                name: "FK_Drafts_Division_DivisionId",
                table: "Drafts");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Division_DivisionId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Division_DivisionId",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Division",
                table: "Division");

            migrationBuilder.RenameTable(
                name: "Division",
                newName: "Divisions");

            migrationBuilder.RenameIndex(
                name: "IX_Division_SeasonId",
                table: "Divisions",
                newName: "IX_Divisions_SeasonId");

            migrationBuilder.AlterColumn<int>(
                name: "DivisionId",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Divisions",
                table: "Divisions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Divisions_Seasons_SeasonId",
                table: "Divisions",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drafts_Divisions_DivisionId",
                table: "Drafts",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Divisions_DivisionId",
                table: "Games",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Divisions_DivisionId",
                table: "Teams",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
