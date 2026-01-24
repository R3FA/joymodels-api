using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    actor_uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    target_user_uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    notification_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    message = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_read = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    read_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    related_entity_uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    related_entity_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.uuid);
                    table.ForeignKey(
                        name: "notifications_ibfk_1",
                        column: x => x.actor_uuid,
                        principalTable: "users",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "notifications_ibfk_2",
                        column: x => x.target_user_uuid,
                        principalTable: "users",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "actor_uuid",
                table: "notifications",
                column: "actor_uuid");

            migrationBuilder.CreateIndex(
                name: "created_at",
                table: "notifications",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "is_read",
                table: "notifications",
                column: "is_read");

            migrationBuilder.CreateIndex(
                name: "target_user_uuid",
                table: "notifications",
                column: "target_user_uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notifications");
        }
    }
}
