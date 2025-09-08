using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "uuid", "role_name" },
                values: new object[,]
                {
                    { new Guid("2107d612-ac38-4390-aeb6-276cf55b42bb"), "Unverified" },
                    { new Guid("44b4be39-2884-462a-98c0-cb3b5eb9c3dd"), "Admin" },
                    { new Guid("6aa5d268-c259-4054-b604-ff545eaa2f1e"), "Undefined" },
                    { new Guid("90051a72-89ea-48ca-8543-43f5843148c8"), "Helper" },
                    { new Guid("c5c8ab92-5933-4122-a804-6533516aeb5d"), "Root" },
                    { new Guid("efd09239-1461-4740-86ae-94ed49137a36"), "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("2107d612-ac38-4390-aeb6-276cf55b42bb"));

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("44b4be39-2884-462a-98c0-cb3b5eb9c3dd"));

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("6aa5d268-c259-4054-b604-ff545eaa2f1e"));

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("90051a72-89ea-48ca-8543-43f5843148c8"));

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("c5c8ab92-5933-4122-a804-6533516aeb5d"));

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("efd09239-1461-4740-86ae-94ed49137a36"));
        }
    }
}
