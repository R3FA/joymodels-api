using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHelperUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "uuid",
                keyValue: new Guid("90051a72-89ea-48ca-8543-43f5843148c8"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "uuid", "role_name" },
                values: new object[] { new Guid("90051a72-89ea-48ca-8543-43f5843148c8"), "Helper" });
        }
    }
}
