using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class DropUserTokenCreatedAtCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CONSTRAINT_1",
                table: "user_tokens");

            migrationBuilder.DropColumn(
                name: "token_created_at",
                table: "user_tokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "token_created_at",
                table: "user_tokens",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddCheckConstraint(
                name: "CONSTRAINT_1",
                table: "user_tokens",
                sql: "`token_expiration_date` > `token_created_at`");
        }
    }
}
