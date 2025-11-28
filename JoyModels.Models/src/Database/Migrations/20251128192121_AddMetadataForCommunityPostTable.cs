using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddMetadataForCommunityPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "community_post_dislikes",
                table: "community_posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "community_post_likes",
                table: "community_posts",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            // Liking a community post
            migrationBuilder.Sql(@"
               CREATE TRIGGER trg_community_post_like_insert
                AFTER INSERT ON community_post_user_reviews
                FOR EACH ROW
                BEGIN
                    IF NEW.review_type_uuid = (SELECT uuid FROM community_post_review_types WHERE review_name = 'Positive') THEN
                        UPDATE community_posts
                        SET community_post_likes = community_post_likes + 1
                        WHERE uuid = NEW.community_post_uuid;
                    END IF;
                END;");
            
            // Unliking a liked community post
            migrationBuilder.Sql(@"
               CREATE TRIGGER trg_community_post_like_delete
                AFTER DELETE ON community_post_user_reviews
                FOR EACH ROW
                BEGIN
                    IF OLD.review_type_uuid = (SELECT uuid FROM community_post_review_types WHERE review_name = 'Positive') THEN
                        UPDATE community_posts
                        SET community_post_likes = community_post_likes - 1
                        WHERE uuid = OLD.community_post_uuid;
                    END IF;
                END;");
            
            // Disliking a community post
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_community_post_dislike_insert
                AFTER INSERT ON community_post_user_reviews
                FOR EACH ROW
                BEGIN
                    IF NEW.review_type_uuid = (SELECT uuid FROM community_post_review_types WHERE review_name = 'Negative') THEN
                        UPDATE community_posts
                        SET community_post_dislikes = community_post_dislikes + 1
                        WHERE uuid = NEW.community_post_uuid;
                    END IF;
                END;");
            
            // Unliking a disliked community post
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_community_post_dislike_delete
                AFTER DELETE ON community_post_user_reviews
                FOR EACH ROW
                BEGIN
                    IF OLD.review_type_uuid = (SELECT uuid FROM community_post_review_types WHERE review_name = 'Negative') THEN
                        UPDATE community_posts
                        SET community_post_dislikes = community_post_dislikes - 1
                        WHERE uuid = OLD.community_post_uuid;
                    END IF;
                END;");
            
            // If the state is changed from like to dislike
            // If the state is changed from dislike to like
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_community_post_review_update
                AFTER UPDATE ON community_post_user_reviews
                FOR EACH ROW
                BEGIN
                    DECLARE positive_uuid BINARY(16);
                    DECLARE negative_uuid BINARY(16);

                    SELECT uuid INTO positive_uuid FROM community_post_review_types WHERE review_name = 'Positive';
                    SELECT uuid INTO negative_uuid FROM community_post_review_types WHERE review_name = 'Negative';

                    IF OLD.review_type_uuid = positive_uuid AND NEW.review_type_uuid = negative_uuid THEN
                        UPDATE community_posts
                        SET community_post_likes = community_post_likes - 1,
                            community_post_dislikes = community_post_dislikes + 1
                        WHERE uuid = NEW.community_post_uuid;
                    END IF;

                    IF OLD.review_type_uuid = negative_uuid AND NEW.review_type_uuid = positive_uuid THEN
                        UPDATE community_posts
                        SET community_post_dislikes = community_post_dislikes - 1,
                            community_post_likes = community_post_likes + 1
                        WHERE uuid = NEW.community_post_uuid;
                    END IF;
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_community_post_like_insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_community_post_like_delete;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_community_post_dislike_insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_community_post_dislike_delete;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_community_post_review_update;");
            
            migrationBuilder.DropColumn(
                name: "community_post_dislikes",
                table: "community_posts");

            migrationBuilder.DropColumn(
                name: "community_post_likes",
                table: "community_posts");
        }
    }
}
