using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHeightAndWidthColsFromCommunityPostPicturesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CONSTRAINT_1",
                table: "community_post_pictures");
            
            migrationBuilder.DropColumn(
                name: "picture_height",
                table: "community_post_pictures");

            migrationBuilder.DropColumn(
                name: "picture_width",
                table: "community_post_pictures");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "picture_height",
                table: "community_post_pictures",
                type: "int(11)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "picture_width",
                table: "community_post_pictures",
                type: "int(11)",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.AddCheckConstraint(
                name: "CONSTRAINT_1",
                table: "community_post_pictures",
                sql: "picture_width > 0 and picture_height > 0");
        }
    }
}
