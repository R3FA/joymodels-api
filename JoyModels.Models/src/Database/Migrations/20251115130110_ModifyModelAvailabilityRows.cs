using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class ModifyModelAvailabilityRows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "model_availability",
                keyColumn: "uuid",
                keyValue: new Guid("cc462d60-2532-49e4-8fb4-4bf44e3d1e04"));

            migrationBuilder.UpdateData(
                table: "model_availability",
                keyColumn: "uuid",
                keyValue: new Guid("e1a7390c-ca8c-4914-83f2-15b0b3b96391"),
                column: "availability_name",
                value: "Public");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "model_availability",
                keyColumn: "uuid",
                keyValue: new Guid("e1a7390c-ca8c-4914-83f2-15b0b3b96391"),
                column: "availability_name",
                value: "Store");

            migrationBuilder.InsertData(
                table: "model_availability",
                columns: new[] { "uuid", "availability_name" },
                values: new object[] { new Guid("cc462d60-2532-49e4-8fb4-4bf44e3d1e04"), "Community" });
        }
    }
}
