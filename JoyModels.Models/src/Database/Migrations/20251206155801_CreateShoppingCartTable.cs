using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class CreateShoppingCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shopping_cart",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "utf8mb4_uca1400_ai_ci"),
                    user_uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "utf8mb4_uca1400_ai_ci"),
                    model_uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "utf8mb4_uca1400_ai_ci"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.uuid);
                    table.ForeignKey(
                        name: "shopping_cart_items_ibfk_1",
                        column: x => x.user_uuid,
                        principalTable: "users",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "shopping_cart_items_ibfk_2",
                        column: x => x.model_uuid,
                        principalTable: "models",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_uca1400_ai_ci");

            migrationBuilder.CreateIndex(
                name: "model_uuid2",
                table: "shopping_cart",
                column: "model_uuid");

            migrationBuilder.CreateIndex(
                name: "uq_user_model",
                table: "shopping_cart",
                columns: new[] { "user_uuid", "model_uuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_uuid4",
                table: "shopping_cart",
                column: "user_uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shopping_cart");
        }
    }
}
