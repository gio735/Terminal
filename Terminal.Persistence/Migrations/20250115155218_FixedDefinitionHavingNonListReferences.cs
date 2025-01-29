using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Terminal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixedDefinitionHavingNonListReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Definitions_References_ReferencesId",
                table: "Definitions");

            migrationBuilder.DropIndex(
                name: "IX_Definitions_ReferencesId",
                table: "Definitions");

            migrationBuilder.DropColumn(
                name: "ReferencesId",
                table: "Definitions");

            migrationBuilder.AddColumn<int>(
                name: "DefinitionId",
                table: "References",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_References_DefinitionId",
                table: "References",
                column: "DefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_References_Definitions_DefinitionId",
                table: "References",
                column: "DefinitionId",
                principalTable: "Definitions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_References_Definitions_DefinitionId",
                table: "References");

            migrationBuilder.DropIndex(
                name: "IX_References_DefinitionId",
                table: "References");

            migrationBuilder.DropColumn(
                name: "DefinitionId",
                table: "References");

            migrationBuilder.AddColumn<int>(
                name: "ReferencesId",
                table: "Definitions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_ReferencesId",
                table: "Definitions",
                column: "ReferencesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Definitions_References_ReferencesId",
                table: "Definitions",
                column: "ReferencesId",
                principalTable: "References",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
