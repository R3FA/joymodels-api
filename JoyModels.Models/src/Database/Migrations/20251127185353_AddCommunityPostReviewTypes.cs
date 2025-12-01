using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunityPostReviewTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "community_post_review_types",
                columns: new[] { "uuid", "review_name" },
                values: new object[,]
                {
                    { new Guid("2e5c75b5-532f-4f5c-b86c-a9bdceb69e80"), "Positive" },
                    { new Guid("86e4b752-2f94-4034-b22f-6dbf806b0fde"), "Negative" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "community_post_review_types",
                keyColumn: "uuid",
                keyValue: new Guid("2e5c75b5-532f-4f5c-b86c-a9bdceb69e80"));

            migrationBuilder.DeleteData(
                table: "community_post_review_types",
                keyColumn: "uuid",
                keyValue: new Guid("86e4b752-2f94-4034-b22f-6dbf806b0fde"));
        }
    }
}
