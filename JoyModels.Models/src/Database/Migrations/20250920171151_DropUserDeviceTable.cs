using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class DropUserDeviceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_tokens_ibfk_2",
                table: "user_tokens");

            migrationBuilder.DropTable(
                name: "user_devices");

            migrationBuilder.DropIndex(
                name: "user_device_uuid",
                table: "user_tokens");

            migrationBuilder.DropColumn(
                name: "user_device_uuid",
                table: "user_tokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_device_uuid",
                table: "user_tokens",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "user_devices",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    device_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_uca1400_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.uuid);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_uca1400_ai_ci");

            migrationBuilder.CreateIndex(
                name: "user_device_uuid",
                table: "user_tokens",
                column: "user_device_uuid");

            migrationBuilder.CreateIndex(
                name: "device_name",
                table: "user_devices",
                column: "device_name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "user_tokens_ibfk_2",
                table: "user_tokens",
                column: "user_device_uuid",
                principalTable: "user_devices",
                principalColumn: "uuid");
        }
    }
}
