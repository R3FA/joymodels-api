using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemovePasswordSaltCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.DropColumn(
                name: "password_salt",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.AddColumn<string>(
                name: "password_salt",
                table: "users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_uca1400_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
