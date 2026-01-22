using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddStripeCustomerIdAndFixOrderIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "stripe_payment_intent_id",
                table: "orders");

            migrationBuilder.AddColumn<string>(
                name: "stripe_customer_id",
                table: "users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb4_uca1400_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "stripe_payment_intent_id",
                table: "orders",
                column: "stripe_payment_intent_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "stripe_payment_intent_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "stripe_customer_id",
                table: "users");

            migrationBuilder.CreateIndex(
                name: "stripe_payment_intent_id",
                table: "orders",
                column: "stripe_payment_intent_id",
                unique: true);
        }
    }
}
