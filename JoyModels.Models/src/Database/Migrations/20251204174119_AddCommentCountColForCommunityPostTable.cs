using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentCountColForCommunityPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "community_post_comment_count",
                table: "community_posts",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            // Increment comment count when new community post comment is created
            migrationBuilder.Sql(@"
               CREATE TRIGGER trg_community_post_comment_insert
                AFTER INSERT ON community_post_question_section
                FOR EACH ROW
                BEGIN
                    UPDATE community_posts
                    SET community_post_comment_count = community_post_comment_count + 1
                    WHERE uuid = NEW.community_post_uuid;
                END;");
            
            // Decrement comment count when community post comment is deleted
            migrationBuilder.Sql(@"
               CREATE TRIGGER trg_community_post_comment_delete
                BEFORE DELETE ON community_post_question_section
                FOR EACH ROW
                BEGIN
                    DECLARE reply_count INT;
                    
                    SELECT COUNT(*) INTO reply_count 
                    FROM community_post_question_section 
                    WHERE parent_message_uuid = OLD.uuid;
                    
                    UPDATE community_posts
                    SET community_post_comment_count = community_post_comment_count - 1 - reply_count
                    WHERE uuid = OLD. community_post_uuid;
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_community_post_comment_insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_community_post_comment_delete;");
            
            migrationBuilder.DropColumn(
                name: "community_post_comment_count",
                table: "community_posts");
        }
    }
}
