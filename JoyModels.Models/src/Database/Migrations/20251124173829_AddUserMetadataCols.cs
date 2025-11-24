using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMetadataCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_follower_count",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "user_following_count",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "user_liked_models_count",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            // 1. INSERT FOLLOWER & FOLLOWING
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_user_followers_insert
                AFTER INSERT ON user_followers
                FOR EACH ROW
                BEGIN
                    UPDATE users 
                    SET user_follower_count = user_follower_count + 1 
                    WHERE uuid = NEW.user_target_uuid;

                    UPDATE users 
                    SET user_following_count = user_following_count + 1 
                    WHERE uuid = NEW.user_origin_uuid;
                    END");

            // 2. DELETE FOLLOWER & FOLLOWING
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_user_followers_delete
                AFTER DELETE ON user_followers
                FOR EACH ROW
                BEGIN
                    UPDATE users 
                    SET user_follower_count = user_follower_count - 1 
                    WHERE uuid = OLD.user_target_uuid;

                    UPDATE users 
                    SET user_following_count = user_following_count - 1 
                    WHERE uuid = OLD.user_origin_uuid;
                    END");

            // 3. MODEL LIKES INSERT
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_user_model_likes_insert
                AFTER INSERT ON user_model_likes
                FOR EACH ROW
                BEGIN
                    UPDATE users
                    SET user_liked_models_count = user_liked_models_count + 1
                    WHERE uuid = NEW.user_uuid;
                    END");

            // 4. MODEL LIKES DELETE
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_user_model_likes_delete
                AFTER DELETE ON user_model_likes
                FOR EACH ROW
                BEGIN
                    UPDATE users
                    SET user_liked_models_count = user_liked_models_count - 1
                    WHERE uuid = OLD.user_uuid;
                    END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_user_model_likes_delete;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_user_model_likes_insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_user_followers_delete;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_user_followers_insert;");
            
            migrationBuilder.DropColumn(
                name: "user_follower_count",
                table: "users");

            migrationBuilder.DropColumn(
                name: "user_following_count",
                table: "users");

            migrationBuilder.DropColumn(
                name: "user_liked_models_count",
                table: "users");
        }
    }
}
