using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiveAID.Migrations
{
    /// <inheritdoc />
    public partial class CleanGalleryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Users_UserId",
                table: "Galleries");

            migrationBuilder.DropIndex(
                name: "IX_Galleries_UserId",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Galleries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Galleries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Galleries_UserId",
                table: "Galleries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Users_UserId",
                table: "Galleries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
