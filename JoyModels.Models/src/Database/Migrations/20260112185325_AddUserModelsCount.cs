using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyModels.Models.src.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUserModelsCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_models_count",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.Sql(@"
               CREATE TRIGGER trg_model_insert
                AFTER INSERT ON models
                FOR EACH ROW
                BEGIN
                    UPDATE users
                    SET user_models_count = user_models_count + 1
                    WHERE uuid = NEW.user_uuid;
                END;");
            
            migrationBuilder.Sql(@"
               CREATE TRIGGER trg_model_delete
                AFTER DELETE ON models
                FOR EACH ROW
                BEGIN
                    UPDATE users
                    SET user_models_count = user_models_count - 1
                    WHERE uuid = OLD.user_uuid
                      AND user_models_count > 0;
                END;");
            
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_model_delete_likes
                BEFORE DELETE ON models
                FOR EACH ROW
                BEGIN
                    DELETE FROM user_model_likes 
                    WHERE model_uuid = OLD.uuid;
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_model_insert;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_model_delete;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_model_delete_likes;");
            
            migrationBuilder.DropColumn(
                name: "user_models_count",
                table: "users");
        }
    }
}
