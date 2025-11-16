using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddModelReviewTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "model_review_types",
                columns: new[] { "uuid", "review_name" },
                values: new object[,]
                {
                    { new Guid("2e3b8189-4df1-42a0-8590-d3e6ec8b91d6"), "Positive" },
                    { new Guid("410b9090-04da-4d0e-a641-719da1962d94"), "Negative" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "model_review_types",
                keyColumn: "uuid",
                keyValue: new Guid("2e3b8189-4df1-42a0-8590-d3e6ec8b91d6"));

            migrationBuilder.DeleteData(
                table: "model_review_types",
                keyColumn: "uuid",
                keyValue: new Guid("410b9090-04da-4d0e-a641-719da1962d94"));
        }
    }
}
