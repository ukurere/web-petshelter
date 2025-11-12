using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_petshelter.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel_20251112 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adoptions_Animals_AnimalId1",
                table: "Adoptions");

            migrationBuilder.DropIndex(
                name: "IX_Adoptions_AnimalId1",
                table: "Adoptions");

            migrationBuilder.DropColumn(
                name: "AnimalId1",
                table: "Adoptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnimalId1",
                table: "Adoptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adoptions_AnimalId1",
                table: "Adoptions",
                column: "AnimalId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Adoptions_Animals_AnimalId1",
                table: "Adoptions",
                column: "AnimalId1",
                principalTable: "Animals",
                principalColumn: "Id");
        }
    }
}
