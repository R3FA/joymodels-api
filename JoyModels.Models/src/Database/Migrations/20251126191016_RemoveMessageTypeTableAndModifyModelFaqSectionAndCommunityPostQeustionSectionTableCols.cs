using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMessageTypeTableAndModifyModelFaqSectionAndCommunityPostQeustionSectionTableCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "community_post_question_section_ibfk_2",
                table: "community_post_question_section");

            migrationBuilder.DropForeignKey(
                name: "community_post_question_section_ibfk_4",
                table: "community_post_question_section");

            migrationBuilder.DropForeignKey(
                name: "model_faq_section_ibfk_2",
                table: "model_faq_section");

            migrationBuilder.DropForeignKey(
                name: "model_faq_section_ibfk_4",
                table: "model_faq_section");

            migrationBuilder.DropTable(
                name: "message_types");

            migrationBuilder.DropIndex(
                name: "message_type_uuid",
                table: "model_faq_section");

            migrationBuilder.DropIndex(
                name: "message_type_uuid",
                table: "community_post_question_section");

            migrationBuilder.DropColumn(
                name: "message_type_uuid",
                table: "model_faq_section");

            migrationBuilder.DropColumn(
                name: "message_type_uuid",
                table: "community_post_question_section");

            migrationBuilder.RenameIndex(
                name: "user_target_uuid",
                table: "user_followers",
                newName: "user_target_uuid");

            migrationBuilder.RenameIndex(
                name: "user_uuid",
                table: "pending_users",
                newName: "user_uuid3");

            migrationBuilder.RenameIndex(
                name: "user_uuid",
                table: "models",
                newName: "user_uuid1");

            migrationBuilder.RenameColumn(
                name: "user_target_uuid",
                table: "model_faq_section",
                newName: "parent_message_uuid");

            migrationBuilder.RenameColumn(
                name: "user_origin_uuid",
                table: "model_faq_section",
                newName: "user_uuid");

            migrationBuilder.RenameIndex(
                name: "user_target_uuid",
                table: "model_faq_section",
                newName: "parent_message_uuid");

            migrationBuilder.RenameIndex(
                name: "user_origin_uuid",
                table: "model_faq_section",
                newName: "user_uuid2");

            migrationBuilder.RenameColumn(
                name: "user_target_uuid",
                table: "community_post_question_section",
                newName: "parent_message_uuid");

            migrationBuilder.RenameColumn(
                name: "user_origin_uuid",
                table: "community_post_question_section",
                newName: "user_uuid");

            migrationBuilder.RenameIndex(
                name: "user_target_uuid",
                table: "community_post_question_section",
                newName: "parent_message_uuid");

            migrationBuilder.RenameIndex(
                name: "user_origin_uuid",
                table: "community_post_question_section",
                newName: "user_uuid");

            migrationBuilder.AddForeignKey(
                name: "community_post_question_section_ibfk_parent",
                table: "community_post_question_section",
                column: "parent_message_uuid",
                principalTable: "community_post_question_section",
                principalColumn: "uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "model_faq_section_ibfk_parent",
                table: "model_faq_section",
                column: "parent_message_uuid",
                principalTable: "model_faq_section",
                principalColumn: "uuid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "community_post_question_section_ibfk_parent",
                table: "community_post_question_section");

            migrationBuilder.DropForeignKey(
                name: "model_faq_section_ibfk_parent",
                table: "model_faq_section");

            migrationBuilder.RenameIndex(
                name: "user_target_uuid",
                table: "user_followers",
                newName: "user_target_uuid");

            migrationBuilder.RenameIndex(
                name: "user_uuid3",
                table: "pending_users",
                newName: "user_uuid");

            migrationBuilder.RenameIndex(
                name: "user_uuid1",
                table: "models",
                newName: "user_uuid");

            migrationBuilder.RenameColumn(
                name: "user_uuid",
                table: "model_faq_section",
                newName: "user_origin_uuid");

            migrationBuilder.RenameColumn(
                name: "parent_message_uuid",
                table: "model_faq_section",
                newName: "user_target_uuid");

            migrationBuilder.RenameIndex(
                name: "user_uuid2",
                table: "model_faq_section",
                newName: "user_origin_uuid");

            migrationBuilder.RenameIndex(
                name: "parent_message_uuid",
                table: "model_faq_section",
                newName: "user_target_uuid");

            migrationBuilder.RenameColumn(
                name: "user_uuid",
                table: "community_post_question_section",
                newName: "user_origin_uuid");

            migrationBuilder.RenameColumn(
                name: "parent_message_uuid",
                table: "community_post_question_section",
                newName: "user_target_uuid");

            migrationBuilder.RenameIndex(
                name: "user_uuid",
                table: "community_post_question_section",
                newName: "user_origin_uuid");

            migrationBuilder.RenameIndex(
                name: "parent_message_uuid",
                table: "community_post_question_section",
                newName: "user_target_uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "message_type_uuid",
                table: "model_faq_section",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "message_type_uuid",
                table: "community_post_question_section",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "message_types",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    message_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_uca1400_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.uuid);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_uca1400_ai_ci");

            migrationBuilder.CreateIndex(
                name: "message_type_uuid",
                table: "model_faq_section",
                column: "message_type_uuid");

            migrationBuilder.CreateIndex(
                name: "message_type_uuid",
                table: "community_post_question_section",
                column: "message_type_uuid");

            migrationBuilder.CreateIndex(
                name: "message_name",
                table: "message_types",
                column: "message_name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "community_post_question_section_ibfk_2",
                table: "community_post_question_section",
                column: "user_target_uuid",
                principalTable: "users",
                principalColumn: "uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "community_post_question_section_ibfk_4",
                table: "community_post_question_section",
                column: "message_type_uuid",
                principalTable: "message_types",
                principalColumn: "uuid");

            migrationBuilder.AddForeignKey(
                name: "model_faq_section_ibfk_2",
                table: "model_faq_section",
                column: "user_target_uuid",
                principalTable: "users",
                principalColumn: "uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "model_faq_section_ibfk_4",
                table: "model_faq_section",
                column: "message_type_uuid",
                principalTable: "message_types",
                principalColumn: "uuid");
        }
    }
}
