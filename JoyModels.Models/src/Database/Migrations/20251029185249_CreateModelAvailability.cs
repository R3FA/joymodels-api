using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class CreateModelAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "model_availability",
                columns: new[] { "uuid", "availability_name" },
                values: new object[,]
                {
                    { new Guid("65dcc8bf-4a87-4cc3-83e9-ee9dfdc937d9"), "Hidden" },
                    { new Guid("cc462d60-2532-49e4-8fb4-4bf44e3d1e04"), "Community" },
                    { new Guid("e1a7390c-ca8c-4914-83f2-15b0b3b96391"), "Store" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "model_availability",
                keyColumn: "uuid",
                keyValue: new Guid("65dcc8bf-4a87-4cc3-83e9-ee9dfdc937d9"));

            migrationBuilder.DeleteData(
                table: "model_availability",
                keyColumn: "uuid",
                keyValue: new Guid("cc462d60-2532-49e4-8fb4-4bf44e3d1e04"));

            migrationBuilder.DeleteData(
                table: "model_availability",
                keyColumn: "uuid",
                keyValue: new Guid("e1a7390c-ca8c-4914-83f2-15b0b3b96391"));
        }
    }
}
