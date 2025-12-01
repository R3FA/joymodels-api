using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunityPostTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "community_post_types",
                columns: new[] { "uuid", "community_post_name" },
                values: new object[,]
                {
                    { new Guid("458c69e7-3d86-44c2-a9c1-336354d81643"), "Guide" },
                    { new Guid("662b1c39-8e30-4567-a874-d1188a88a8fb"), "Post" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "community_post_types",
                keyColumn: "uuid",
                keyValue: new Guid("458c69e7-3d86-44c2-a9c1-336354d81643"));

            migrationBuilder.DeleteData(
                table: "community_post_types",
                keyColumn: "uuid",
                keyValue: new Guid("662b1c39-8e30-4567-a874-d1188a88a8fb"));
        }
    }
}
