using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reports",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    reporter_uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    reported_entity_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    reported_entity_uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    reason = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    reviewed_by_uuid = table.Column<Guid>(type: "char(36)", nullable: true),
                    reviewed_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.uuid);
                    table.ForeignKey(
                        name: "reports_ibfk_1",
                        column: x => x.reporter_uuid,
                        principalTable: "users",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "reports_ibfk_2",
                        column: x => x.reviewed_by_uuid,
                        principalTable: "users",
                        principalColumn: "uuid");
                });

            migrationBuilder.CreateIndex(
                name: "reported_entity_uuid",
                table: "reports",
                column: "reported_entity_uuid");

            migrationBuilder.CreateIndex(
                name: "reporter_uuid",
                table: "reports",
                column: "reporter_uuid");

            migrationBuilder.CreateIndex(
                name: "reviewed_by_uuid",
                table: "reports",
                column: "reviewed_by_uuid");

            migrationBuilder.CreateIndex(
                name: "status1",
                table: "reports",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "uq_reporter_entity",
                table: "reports",
                columns: new[] { "reporter_uuid", "reported_entity_uuid" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reports");
        }
    }
}
