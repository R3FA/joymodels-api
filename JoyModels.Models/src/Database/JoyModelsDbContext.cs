using JoyModels.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Models.Database;

public partial class JoyModelsDbContext : DbContext
{
    public JoyModelsDbContext()
    {
    }

    public JoyModelsDbContext(DbContextOptions<JoyModelsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CommunityPost> CommunityPosts { get; set; }

    public virtual DbSet<CommunityPostPicture> CommunityPostPictures { get; set; }

    public virtual DbSet<CommunityPostQuestionSection> CommunityPostQuestionSections { get; set; }

    public virtual DbSet<CommunityPostReviewType> CommunityPostReviewTypes { get; set; }

    public virtual DbSet<CommunityPostType> CommunityPostTypes { get; set; }

    public virtual DbSet<CommunityPostUserReview> CommunityPostUserReviews { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<ModelAvailability> ModelAvailabilities { get; set; }

    public virtual DbSet<ModelCategory> ModelCategories { get; set; }

    public virtual DbSet<ModelFaqSection> ModelFaqSections { get; set; }

    public virtual DbSet<ModelPicture> ModelPictures { get; set; }

    public virtual DbSet<ModelReview> ModelReviews { get; set; }

    public virtual DbSet<ModelReviewType> ModelReviewTypes { get; set; }

    public virtual DbSet<PendingUser> PendingUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserFollower> UserFollowers { get; set; }

    public virtual DbSet<UserModelLike> UserModelLikes { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_uca1400_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.HasIndex(e => e.CategoryName, "category_name").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("category_name");

            entity.HasData(
                new Category
                {
                    Uuid = new Guid("d4363787-30b2-4cf9-b7ce-26dd2dce81ad"),
                    CategoryName = "Characters"
                },
                new Category
                {
                    Uuid = new Guid("c72086e6-fe8c-480a-a918-f02cf632865e"),
                    CategoryName = "Humans"
                },
                new Category
                {
                    Uuid = new Guid("8c76c6ad-d49d-4620-835b-333bf8996517"),
                    CategoryName = "Robots & Mechanics"
                },
                new Category
                {
                    Uuid = new Guid("5457c9f7-a077-41c8-82cf-57cfd024b940"),
                    CategoryName = "Animals"
                },
                new Category
                {
                    Uuid = new Guid("b4806536-be2d-4528-8548-1e7969efd599"),
                    CategoryName = "Plants & Vegetation"
                },
                new Category
                {
                    Uuid = new Guid("e6a53e54-39af-4345-8cb1-3ba3650e85e8"),
                    CategoryName = "Rocks & Minerals"
                },
                new Category
                {
                    Uuid = new Guid("5c76a1da-3881-4b95-8497-abea4c565a86"),
                    CategoryName = "Terrain & Landscapes"
                },
                new Category
                {
                    Uuid = new Guid("228c1335-eef0-4c0c-b518-a36cfd237b00"),
                    CategoryName = "Buildings"
                },
                new Category
                {
                    Uuid = new Guid("af71e3b2-7628-4492-a83d-36983526bfc3"),
                    CategoryName = "Interiors"
                },
                new Category
                {
                    Uuid = new Guid("6f916b5b-2ca7-4174-a37f-04e4d203dfe8"),
                    CategoryName = "Props"
                },
                new Category
                {
                    Uuid = new Guid("56aa06ec-c42b-4fb7-ac5e-322cf5dd5925"),
                    CategoryName = "Industrial & Factory"
                },
                new Category
                {
                    Uuid = new Guid("0c09b371-eee6-4c1d-91e9-c0888f62545e"),
                    CategoryName = "Tools & Hardware"
                },
                new Category
                {
                    Uuid = new Guid("6f8f81ea-232b-4fd7-8f90-fe5e61f2c87f"),
                    CategoryName = "Electronics & Gadgets"
                },
                new Category
                {
                    Uuid = new Guid("81463c5f-aac3-4f99-bf8c-2b909b8b5c47"),
                    CategoryName = "Clothing & Accessories"
                },
                new Category
                {
                    Uuid = new Guid("08edf884-994b-4f1c-9344-189a18904e9a"),
                    CategoryName = "Jewelry"
                },
                new Category
                {
                    Uuid = new Guid("09798ef5-3756-4b3b-afbb-1d539efe0f35"),
                    CategoryName = "Sports & Fitness"
                },
                new Category
                {
                    Uuid = new Guid("f4d84098-e750-4ca5-b71c-0c46b0b060f4"),
                    CategoryName = "Medical & Anatomy"
                },
                new Category
                {
                    Uuid = new Guid("18ab38b1-7dee-4e4d-84ba-e3c6eb69d453"),
                    CategoryName = "Military"
                },
                new Category
                {
                    Uuid = new Guid("b3d581f7-d746-4978-9c17-efe9002623d4"),
                    CategoryName = "Sci‑Fi"
                },
                new Category
                {
                    Uuid = new Guid("3f838fcc-d267-4b46-999c-0f3e1bedc5d9"),
                    CategoryName = "Fantasy"
                },
                new Category
                {
                    Uuid = new Guid("6362a527-9322-4820-91e7-36b1798913ea"),
                    CategoryName = "Horror"
                },
                new Category
                {
                    Uuid = new Guid("056ae18c-6e0f-420b-9583-077cd7a483ff"),
                    CategoryName = "Musical Instruments"
                },
                new Category
                {
                    Uuid = new Guid("b29ec84d-3951-4f1c-b881-7deba7c2f87d"),
                    CategoryName = "Office & Education"
                },
                new Category
                {
                    Uuid = new Guid("e1d11f5a-f802-42ca-9dc3-981b8eafc4ff"),
                    CategoryName = "Boats & Ships"
                },
                new Category
                {
                    Uuid = new Guid("98bfb9ab-cc57-4c13-9e45-9ef265e2092b"),
                    CategoryName = "Aircraft & Drones"
                },
                new Category
                {
                    Uuid = new Guid("f34a3545-733c-42fa-8fad-82b3277d486f"),
                    CategoryName = "Spacecraft"
                },
                new Category
                {
                    Uuid = new Guid("6aaeb356-5adc-4ceb-82b7-cf6f35e19df2"),
                    CategoryName = "Trains & Rail"
                },
                new Category
                {
                    Uuid = new Guid("68737f63-995c-4ee0-8b01-4558e7fc2dd2"),
                    CategoryName = "Nature Elements"
                },
                new Category
                {
                    Uuid = new Guid("eedc9940-378f-416b-9127-29f26985d6ed"),
                    CategoryName = "History"
                }
            );
        });

        modelBuilder.Entity<CommunityPost>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("community_posts");

            entity.HasIndex(e => e.PostTypeUuid, "post_type_uuid");

            entity.HasIndex(e => new { e.UserUuid, e.Title }, "uq_user_title").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.PostTypeUuid).HasColumnName("post_type_uuid");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.UserUuid).HasColumnName("user_uuid");
            entity.Property(e => e.YoutubeVideoLink)
                .HasMaxLength(2048)
                .HasColumnName("youtube_video_link");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CommunityPostLikes)
                .HasColumnName("community_post_likes")
                .HasDefaultValue(0)
                .IsRequired();
            entity.Property(e => e.CommunityPostDislikes)
                .HasColumnName("community_post_dislikes")
                .HasDefaultValue(0)
                .IsRequired();

            entity.HasOne(d => d.PostTypeUu).WithMany(p => p.CommunityPosts)
                .HasForeignKey(d => d.PostTypeUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("community_posts_ibfk_2");

            entity.HasOne(d => d.UserUu).WithMany(p => p.CommunityPosts)
                .HasForeignKey(d => d.UserUuid)
                .HasConstraintName("community_posts_ibfk_1");
        });

        modelBuilder.Entity<CommunityPostPicture>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("community_post_pictures");

            entity.HasIndex(e => new { e.CommunityPostUuid, e.PictureLocation }, "uq_community_post_picture")
                .IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.CommunityPostUuid).HasColumnName("community_post_uuid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PictureLocation)
                .HasMaxLength(254)
                .HasColumnName("picture_location");

            entity.HasOne(d => d.CommunityPostUu).WithMany(p => p.CommunityPostPictures)
                .HasForeignKey(d => d.CommunityPostUuid)
                .HasConstraintName("community_post_pictures_ibfk_1");
        });

        modelBuilder.Entity<CommunityPostQuestionSection>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("community_post_question_section");

            entity.HasIndex(e => e.CommunityPostUuid, "community_post_uuid");
            entity.HasIndex(e => e.UserUuid, "user_uuid");
            entity.HasIndex(e => e.ParentMessageUuid, "parent_message_uuid");


            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.ParentMessageUuid).HasColumnName("parent_message_uuid");
            entity.Property(e => e.CommunityPostUuid).HasColumnName("community_post_uuid").IsRequired();
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at")
                .IsRequired();
            entity.Property(e => e.MessageText)
                .HasColumnType("text")
                .HasColumnName("message_text")
                .IsRequired();
            entity.Property(e => e.UserUuid).HasColumnName("user_uuid").IsRequired();

            entity.HasOne(d => d.CommunityPostUu).WithMany(p => p.CommunityPostQuestionSections)
                .HasForeignKey(d => d.CommunityPostUuid)
                .HasConstraintName("community_post_question_section_ibfk_3");

            entity.HasOne(d => d.UserUu).WithMany(p => p.CommunityPostQuestionSectionUserUus)
                .HasForeignKey(d => d.UserUuid)
                .HasConstraintName("community_post_question_section_ibfk_1");

            entity.HasOne(d => d.ParentMessage)
                .WithMany(p => p.Replies)
                .HasForeignKey(d => d.ParentMessageUuid)
                .HasConstraintName("community_post_question_section_ibfk_parent")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommunityPostReviewType>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("community_post_review_types");

            entity.HasIndex(e => e.ReviewName, "review_name").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.ReviewName)
                .HasMaxLength(50)
                .HasColumnName("review_name");

            entity.HasData(
                new
                {
                    Uuid = new Guid("86e4b752-2f94-4034-b22f-6dbf806b0fde"), ReviewName = "Negative"
                },
                new
                {
                    Uuid = new Guid("2e5c75b5-532f-4f5c-b86c-a9bdceb69e80"), ReviewName = "Positive"
                }
            );
        });

        modelBuilder.Entity<CommunityPostType>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("community_post_types");

            entity.HasIndex(e => e.CommunityPostName, "community_post_name").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.CommunityPostName)
                .HasMaxLength(50)
                .HasColumnName("community_post_name");

            entity.HasData(
                new CommunityPostType
                {
                    Uuid = new Guid("458c69e7-3d86-44c2-a9c1-336354d81643"),
                    CommunityPostName = "Guide"
                },
                new CommunityPostType
                {
                    Uuid = new Guid("662b1c39-8e30-4567-a874-d1188a88a8fb"),
                    CommunityPostName = "Post"
                }
            );
        });

        modelBuilder.Entity<CommunityPostUserReview>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("community_post_user_reviews");

            entity.HasIndex(e => e.CommunityPostUuid, "community_post_uuid");

            entity.HasIndex(e => e.ReviewTypeUuid, "review_type_uuid");

            entity.HasIndex(e => new { e.UserUuid, e.CommunityPostUuid }, "uq_user_community_post").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.CommunityPostUuid).HasColumnName("community_post_uuid");
            entity.Property(e => e.ReviewTypeUuid).HasColumnName("review_type_uuid");
            entity.Property(e => e.UserUuid).HasColumnName("user_uuid");

            entity.HasOne(d => d.CommunityPostUu).WithMany(p => p.CommunityPostUserReviews)
                .HasForeignKey(d => d.CommunityPostUuid)
                .HasConstraintName("community_post_user_reviews_ibfk_2");

            entity.HasOne(d => d.ReviewTypeUu).WithMany(p => p.CommunityPostUserReviews)
                .HasForeignKey(d => d.ReviewTypeUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("community_post_user_reviews_ibfk_3");

            entity.HasOne(d => d.UserUu).WithMany(p => p.CommunityPostUserReviews)
                .HasForeignKey(d => d.UserUuid)
                .HasConstraintName("community_post_user_reviews_ibfk_1");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("models");

            entity.HasIndex(e => e.ModelAvailabilityUuid, "model_availability_uuid");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.HasIndex(e => e.UserUuid, "user_uuid");

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.ModelAvailabilityUuid).HasColumnName("model_availability_uuid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.LocationPath)
                .HasMaxLength(255)
                .HasColumnName("location_path");
            entity.HasIndex(e => e.LocationPath, "location_path").IsUnique();
            entity.Property(e => e.UserUuid).HasColumnName("user_uuid");

            entity.HasOne(d => d.ModelAvailabilityUu).WithMany(p => p.Models)
                .HasForeignKey(d => d.ModelAvailabilityUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("models_ibfk_2");

            entity.HasOne(d => d.UserUu).WithMany(p => p.Models)
                .HasForeignKey(d => d.UserUuid)
                .HasConstraintName("models_ibfk_1");
        });

        modelBuilder.Entity<ModelAvailability>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("model_availability");

            entity.HasIndex(e => e.AvailabilityName, "availability_name").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.AvailabilityName)
                .HasMaxLength(50)
                .HasColumnName("availability_name");

            entity.HasData(
                new ModelAvailability
                {
                    Uuid = new Guid("65dcc8bf-4a87-4cc3-83e9-ee9dfdc937d9"),
                    AvailabilityName = "Hidden"
                },
                new ModelAvailability
                {
                    Uuid = new Guid("e1a7390c-ca8c-4914-83f2-15b0b3b96391"),
                    AvailabilityName = "Public"
                }
            );
        });

        modelBuilder.Entity<ModelCategory>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("model_categories");

            entity.HasIndex(e => e.CategoryUuid, "category_uuid");

            entity.HasIndex(e => new { e.ModelUuid, e.CategoryUuid }, "uq_model_category").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.CategoryUuid).HasColumnName("category_uuid");
            entity.Property(e => e.ModelUuid).HasColumnName("model_uuid");

            entity.HasOne(d => d.CategoryUu).WithMany(p => p.ModelCategories)
                .HasForeignKey(d => d.CategoryUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("model_categories_ibfk_2");

            entity.HasOne(d => d.ModelUu).WithMany(p => p.ModelCategories)
                .HasForeignKey(d => d.ModelUuid)
                .HasConstraintName("model_categories_ibfk_1");
        });

        modelBuilder.Entity<ModelFaqSection>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("model_faq_section");

            entity.HasIndex(e => e.ModelUuid, "model_uuid");

            entity.HasIndex(e => e.UserUuid, "user_uuid");

            entity.HasIndex(e => e.ParentMessageUuid, "parent_message_uuid");

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at")
                .IsRequired();
            entity.Property(e => e.MessageText)
                .HasColumnType("text")
                .HasColumnName("message_text")
                .IsRequired();
            entity.Property(e => e.ModelUuid).HasColumnName("model_uuid").IsRequired();
            entity.Property(e => e.UserUuid).HasColumnName("user_uuid").IsRequired();
            entity.Property(e => e.ParentMessageUuid).HasColumnName("parent_message_uuid");

            entity.HasOne(d => d.ModelUu).WithMany(p => p.ModelFaqSections)
                .HasForeignKey(d => d.ModelUuid)
                .HasConstraintName("model_faq_section_ibfk_3");

            entity.HasOne(d => d.UserUu).WithMany(p => p.ModelFaqSectionUserUus)
                .HasForeignKey(d => d.UserUuid)
                .HasConstraintName("model_faq_section_ibfk_1");

            entity.HasOne(d => d.ParentMessage)
                .WithMany(p => p.Replies)
                .HasForeignKey(d => d.ParentMessageUuid)
                .HasConstraintName("model_faq_section_ibfk_parent")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ModelPicture>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("model_pictures");

            entity.HasIndex(e => new { e.ModelUuid, e.PictureLocation }, "uq_model_picture").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ModelUuid).HasColumnName("model_uuid");
            entity.Property(e => e.PictureLocation)
                .HasMaxLength(254)
                .HasColumnName("picture_location");

            entity.HasOne(d => d.ModelUu).WithMany(p => p.ModelPictures)
                .HasForeignKey(d => d.ModelUuid)
                .HasConstraintName("model_pictures_ibfk_1");
        });

        modelBuilder.Entity<ModelReview>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("model_reviews");

            entity.HasIndex(e => e.ModelUuid, "model_uuid");

            entity.HasIndex(e => e.ReviewTypeUuid, "review_type_uuid");

            entity.HasIndex(e => new { e.UserUuid, e.ModelUuid }, "uq_user_model_review").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ModelUuid).HasColumnName("model_uuid");
            entity.Property(e => e.ReviewText)
                .HasColumnType("text")
                .HasColumnName("review_text");
            entity.Property(e => e.ReviewTypeUuid).HasColumnName("review_type_uuid");
            entity.Property(e => e.UserUuid).HasColumnName("user_uuid");

            entity.HasOne(d => d.ModelUu).WithMany(p => p.ModelReviews)
                .HasForeignKey(d => d.ModelUuid)
                .HasConstraintName("model_reviews_ibfk_1");

            entity.HasOne(d => d.ReviewTypeUu).WithMany(p => p.ModelReviews)
                .HasForeignKey(d => d.ReviewTypeUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("model_reviews_ibfk_3");

            entity.HasOne(d => d.UserUu).WithMany(p => p.ModelReviews)
                .HasForeignKey(d => d.UserUuid)
                .HasConstraintName("model_reviews_ibfk_2");
        });

        modelBuilder.Entity<ModelReviewType>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("model_review_types");

            entity.HasIndex(e => e.ReviewName, "review_name").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.ReviewName)
                .HasMaxLength(50)
                .HasColumnName("review_name");

            entity.HasData(
                new
                {
                    Uuid = new Guid("410b9090-04da-4d0e-a641-719da1962d94"), ReviewName = "Negative"
                },
                new
                {
                    Uuid = new Guid("2e3b8189-4df1-42a0-8590-d3e6ec8b91d6"), ReviewName = "Positive"
                }
            );
        });

        modelBuilder.Entity<PendingUser>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("pending_users");

            entity.HasIndex(e => e.OtpCode, "otp_code").IsUnique();

            entity.HasIndex(e => e.UserUuid, "user_uuid");

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.OtpCode)
                .HasMaxLength(12)
                .IsFixedLength()
                .HasColumnName("otp_code");
            entity.Property(e => e.OtpCreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("otp_created_at");
            entity.Property(e => e.OtpExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("otp_expiration_date");
            entity.Property(e => e.UserUuid).HasColumnName("user_uuid");

            entity.HasOne(d => d.UserUu).WithMany(p => p.PendingUsers)
                .HasForeignKey(d => d.UserUuid)
                .HasConstraintName("pending_users_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.NickName, "nick_name").IsUnique();

            entity.HasIndex(e => e.UserRoleUuid, "user_role_uuid");

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.NickName)
                .HasMaxLength(50)
                .HasColumnName("nick_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.UserRoleUuid).HasColumnName("user_role_uuid");
            entity.Property(e => e.UserPictureLocation)
                .HasMaxLength(255)
                .HasColumnName("user_picture_location");
            entity.Property(e => e.UserFollowerCount)
                .HasColumnName("user_follower_count")
                .HasDefaultValue(0)
                .IsRequired();
            entity.Property(e => e.UserFollowingCount)
                .HasColumnName("user_following_count")
                .HasDefaultValue(0)
                .IsRequired();
            entity.Property(e => e.UserLikedModelsCount)
                .HasColumnName("user_liked_models_count")
                .HasDefaultValue(0)
                .IsRequired();

            entity.HasOne(d => d.UserRoleUu).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_ibfk_1");
        });

        modelBuilder.Entity<UserFollower>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("user_followers");

            entity.HasIndex(e => new { e.UserOriginUuid, e.UserTargetUuid }, "uq_unique_follow").IsUnique();

            entity.HasIndex(e => e.UserTargetUuid, "user_target_uuid");

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.FollowedAt)
                .HasColumnType("datetime")
                .HasColumnName("followed_at");
            entity.Property(e => e.UserOriginUuid).HasColumnName("user_origin_uuid");
            entity.Property(e => e.UserTargetUuid).HasColumnName("user_target_uuid");

            entity.HasOne(d => d.UserOriginUu).WithMany(p => p.UserFollowerUserOriginUus)
                .HasForeignKey(d => d.UserOriginUuid)
                .HasConstraintName("user_followers_ibfk_1");

            entity.HasOne(d => d.UserTargetUu).WithMany(p => p.UserFollowerUserTargetUus)
                .HasForeignKey(d => d.UserTargetUuid)
                .HasConstraintName("user_followers_ibfk_2");
        });

        modelBuilder.Entity<UserModelLike>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("user_model_likes");

            entity.HasIndex(e => e.ModelUuid, "model_uuid");

            entity.HasIndex(e => new { e.UserUuid, e.ModelUuid }, "uq_user_model_like").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.ModelUuid).HasColumnName("model_uuid");
            entity.Property(e => e.UserUuid).HasColumnName("user_uuid");

            entity.HasOne(d => d.ModelUu).WithMany(p => p.UserModelLikes)
                .HasForeignKey(d => d.ModelUuid)
                .HasConstraintName("user_model_likes_ibfk_2");

            entity.HasOne(d => d.UserUu).WithMany(p => p.UserModelLikes)
                .HasForeignKey(d => d.UserUuid)
                .HasConstraintName("user_model_likes_ibfk_1");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("user_roles");

            entity.HasIndex(e => e.RoleName, "role_name").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");

            entity.HasData(
                new UserRole { Uuid = new Guid("2107d612-ac38-4390-aeb6-276cf55b42bb"), RoleName = "Unverified" },
                new UserRole { Uuid = new Guid("efd09239-1461-4740-86ae-94ed49137a36"), RoleName = "User" },
                new UserRole { Uuid = new Guid("90051a72-89ea-48ca-8543-43f5843148c8"), RoleName = "Helper" },
                new UserRole { Uuid = new Guid("44b4be39-2884-462a-98c0-cb3b5eb9c3dd"), RoleName = "Admin" },
                new UserRole { Uuid = new Guid("c5c8ab92-5933-4122-a804-6533516aeb5d"), RoleName = "Root" }
            );
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("user_tokens");

            entity.HasIndex(e => new { e.UserUuid, e.RefreshToken }, "uq_user_refresh_token").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");
            entity.Property(e => e.TokenExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("token_expiration_date");
            entity.Property(e => e.UserUuid).HasColumnName("user_uuid");

            entity.HasOne(d => d.UserUu).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserUuid)
                .HasConstraintName("user_tokens_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}