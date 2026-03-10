using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiveAID.Migrations
{
    /// <inheritdoc />
    public partial class addmissingproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Partners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "NGOs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Galleries",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisteredAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "NGOs");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Galleries");

            migrationBuilder.AddColumn<string>(
                name: "DateTime",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
