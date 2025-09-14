using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class ExpandOtpCodeLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "otp_code",
                table: "pending_users",
                type: "char(12)",
                fixedLength: true,
                maxLength: 12,
                nullable: false,
                collation: "utf8mb4_uca1400_ai_ci",
                oldClrType: typeof(string),
                oldType: "char(8)",
                oldFixedLength: true,
                oldMaxLength: 8)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_uca1400_ai_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "otp_code",
                table: "pending_users",
                type: "char(8)",
                fixedLength: true,
                maxLength: 8,
                nullable: false,
                collation: "utf8mb4_uca1400_ai_ci",
                oldClrType: typeof(string),
                oldType: "char(12)",
                oldFixedLength: true,
                oldMaxLength: 12)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_uca1400_ai_ci");
        }
    }
}
