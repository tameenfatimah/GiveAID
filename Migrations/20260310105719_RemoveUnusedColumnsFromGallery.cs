using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiveAID.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedColumnsFromGallery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Users_UserId1",
                table: "Galleries");

            migrationBuilder.DropIndex(
                name: "IX_Galleries_UserId1",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Galleries");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Galleries",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Users_UserId",
                table: "Galleries");

            migrationBuilder.DropIndex(
                name: "IX_Galleries_UserId",
                table: "Galleries");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Galleries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Galleries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Galleries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Galleries_UserId1",
                table: "Galleries",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Users_UserId1",
                table: "Galleries",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
