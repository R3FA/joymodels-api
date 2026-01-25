using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSetNullOnReportReviewedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "reports_ibfk_2",
                table: "reports");

            migrationBuilder.AddForeignKey(
                name: "reports_ibfk_2",
                table: "reports",
                column: "reviewed_by_uuid",
                principalTable: "users",
                principalColumn: "uuid",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "reports_ibfk_2",
                table: "reports");

            migrationBuilder.AddForeignKey(
                name: "reports_ibfk_2",
                table: "reports",
                column: "reviewed_by_uuid",
                principalTable: "users",
                principalColumn: "uuid");
        }
    }
}
