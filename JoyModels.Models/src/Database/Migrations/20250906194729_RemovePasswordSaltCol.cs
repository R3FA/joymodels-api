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

            migrationBuilder.DropColumn(
                name: "password_salt",
                table: "users");

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "uuid", "role_name" },
                values: new object[,]
                {
                    { new Guid("03ee6412-91e7-4352-a614-23b09a779814"), "Root" },
                    { new Guid("3758f6b0-834f-4cd5-8725-ef718639e5b7"), "Admin" },
                    { new Guid("3c093d83-18eb-41a4-9d62-c8d34e63d83c"), "Unverified" },
                    { new Guid("9d532540-9d12-41be-88a2-4fe750f73972"), "Undefined" },
                    { new Guid("b2d041cc-438b-49dc-9e2e-034f0cd88af2"), "User" },
                    { new Guid("fee0de0a-f25d-4c5e-bb38-253c47e1964d"), "Helper" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("03ee6412-91e7-4352-a614-23b09a779814"));

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("3758f6b0-834f-4cd5-8725-ef718639e5b7"));

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("3c093d83-18eb-41a4-9d62-c8d34e63d83c"));

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("9d532540-9d12-41be-88a2-4fe750f73972"));

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("b2d041cc-438b-49dc-9e2e-034f0cd88af2"));

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("fee0de0a-f25d-4c5e-bb38-253c47e1964d"));

            migrationBuilder.AddColumn<string>(
                name: "password_salt",
                table: "users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                collation: "utf8mb4_uca1400_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

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
    }
}
