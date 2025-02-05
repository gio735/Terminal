using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Terminal.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SomeUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefinitionReference_References_ReferenceId",
                table: "DefinitionReference");

            migrationBuilder.DropForeignKey(
                name: "FK_Definitions_Definitions_DefinitionId",
                table: "Definitions");

            migrationBuilder.DropForeignKey(
                name: "FK_References_Definitions_DefinitionId",
                table: "References");

            migrationBuilder.DropIndex(
                name: "IX_References_DefinitionId",
                table: "References");

            migrationBuilder.DropIndex(
                name: "IX_Definitions_DefinitionId",
                table: "Definitions");

            migrationBuilder.DropColumn(
                name: "DefinitionId",
                table: "References");

            migrationBuilder.DropColumn(
                name: "DefinitionId",
                table: "Definitions");

            migrationBuilder.RenameColumn(
                name: "ReferenceId",
                table: "DefinitionReference",
                newName: "ReferencesId");

            migrationBuilder.RenameIndex(
                name: "IX_DefinitionReference_ReferenceId",
                table: "DefinitionReference",
                newName: "IX_DefinitionReference_ReferencesId");

            migrationBuilder.CreateTable(
                name: "DefinitionDefinition",
                columns: table => new
                {
                    DefinitionId = table.Column<int>(type: "int", nullable: false),
                    SimilarsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefinitionDefinition", x => new { x.DefinitionId, x.SimilarsId });
                    table.ForeignKey(
                        name: "FK_DefinitionDefinition_Definitions_DefinitionId",
                        column: x => x.DefinitionId,
                        principalTable: "Definitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DefinitionDefinition_Definitions_SimilarsId",
                        column: x => x.SimilarsId,
                        principalTable: "Definitions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefinitionDefinition_SimilarsId",
                table: "DefinitionDefinition",
                column: "SimilarsId");

            migrationBuilder.AddForeignKey(
                name: "FK_DefinitionReference_References_ReferencesId",
                table: "DefinitionReference",
                column: "ReferencesId",
                principalTable: "References",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefinitionReference_References_ReferencesId",
                table: "DefinitionReference");

            migrationBuilder.DropTable(
                name: "DefinitionDefinition");

            migrationBuilder.RenameColumn(
                name: "ReferencesId",
                table: "DefinitionReference",
                newName: "ReferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_DefinitionReference_ReferencesId",
                table: "DefinitionReference",
                newName: "IX_DefinitionReference_ReferenceId");

            migrationBuilder.AddColumn<int>(
                name: "DefinitionId",
                table: "References",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefinitionId",
                table: "Definitions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_References_DefinitionId",
                table: "References",
                column: "DefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_DefinitionId",
                table: "Definitions",
                column: "DefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DefinitionReference_References_ReferenceId",
                table: "DefinitionReference",
                column: "ReferenceId",
                principalTable: "References",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Definitions_Definitions_DefinitionId",
                table: "Definitions",
                column: "DefinitionId",
                principalTable: "Definitions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_References_Definitions_DefinitionId",
                table: "References",
                column: "DefinitionId",
                principalTable: "Definitions",
                principalColumn: "Id");
        }
    }
}
