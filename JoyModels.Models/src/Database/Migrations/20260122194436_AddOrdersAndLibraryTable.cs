using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdersAndLibraryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "model_uuid5",
                table: "user_model_likes",
                newName: "model_uuid3");

            migrationBuilder.RenameIndex(
                name: "user_uuid6",
                table: "shopping_cart",
                newName: "user_uuid4");

            migrationBuilder.RenameIndex(
                name: "model_uuid4",
                table: "shopping_cart",
                newName: "model_uuid2");

            migrationBuilder.RenameIndex(
                name: "user_uuid5",
                table: "pending_users",
                newName: "user_uuid3");

            migrationBuilder.RenameIndex(
                name: "user_uuid2",
                table: "models",
                newName: "user_uuid1");

            migrationBuilder.RenameIndex(
                name: "model_uuid2",
                table: "model_reviews",
                newName: "model_uuid1");

            migrationBuilder.RenameIndex(
                name: "user_uuid3",
                table: "model_faq_section",
                newName: "user_uuid2");

            migrationBuilder.RenameIndex(
                name: "model_uuid1",
                table: "model_faq_section",
                newName: "model_uuid");

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    user_uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    model_uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stripe_payment_intent_id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.uuid);
                    table.ForeignKey(
                        name: "orders_ibfk_1",
                        column: x => x.user_uuid,
                        principalTable: "users",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "orders_ibfk_2",
                        column: x => x.model_uuid,
                        principalTable: "models",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_uca1400_ai_ci");

            migrationBuilder.CreateTable(
                name: "libraries",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    user_uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    model_uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    order_uuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    acquired_at = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.uuid);
                    table.ForeignKey(
                        name: "libraries_ibfk_1",
                        column: x => x.user_uuid,
                        principalTable: "users",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "libraries_ibfk_2",
                        column: x => x.model_uuid,
                        principalTable: "models",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "libraries_ibfk_3",
                        column: x => x.order_uuid,
                        principalTable: "orders",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_uca1400_ai_ci");

            migrationBuilder.CreateIndex(
                name: "model_uuid",
                table: "libraries",
                column: "model_uuid");

            migrationBuilder.CreateIndex(
                name: "order_uuid",
                table: "libraries",
                column: "order_uuid");

            migrationBuilder.CreateIndex(
                name: "uq_library_user_model",
                table: "libraries",
                columns: new[] { "user_uuid", "model_uuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_uuid1",
                table: "libraries",
                column: "user_uuid");

            migrationBuilder.CreateIndex(
                name: "model_uuid3",
                table: "orders",
                column: "model_uuid");

            migrationBuilder.CreateIndex(
                name: "status",
                table: "orders",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "stripe_payment_intent_id",
                table: "orders",
                column: "stripe_payment_intent_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_uuid4",
                table: "orders",
                column: "user_uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "libraries");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.RenameIndex(
                name: "model_uuid3",
                table: "user_model_likes",
                newName: "model_uuid5");

            migrationBuilder.RenameIndex(
                name: "user_uuid4",
                table: "shopping_cart",
                newName: "user_uuid6");

            migrationBuilder.RenameIndex(
                name: "model_uuid2",
                table: "shopping_cart",
                newName: "model_uuid4");

            migrationBuilder.RenameIndex(
                name: "user_uuid3",
                table: "pending_users",
                newName: "user_uuid5");

            migrationBuilder.RenameIndex(
                name: "user_uuid1",
                table: "models",
                newName: "user_uuid2");

            migrationBuilder.RenameIndex(
                name: "model_uuid1",
                table: "model_reviews",
                newName: "model_uuid2");

            migrationBuilder.RenameIndex(
                name: "user_uuid2",
                table: "model_faq_section",
                newName: "user_uuid3");

            migrationBuilder.RenameIndex(
                name: "model_uuid",
                table: "model_faq_section",
                newName: "model_uuid1");
        }
    }
}
