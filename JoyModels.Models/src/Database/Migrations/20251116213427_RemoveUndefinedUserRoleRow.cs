using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUndefinedUserRoleRow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("6aa5d268-c259-4054-b604-ff545eaa2f1e"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "uuid", "role_name" },
                values: new object[] { new Guid("6aa5d268-c259-4054-b604-ff545eaa2f1e"), "Undefined" });
        }
    }
}
