using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace JoyModels.Models.src.Database.Entities;

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

    public virtual DbSet<MessageType> MessageTypes { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<ModelAvailability> ModelAvailabilities { get; set; }

    public virtual DbSet<ModelCategory> ModelCategories { get; set; }

    public virtual DbSet<ModelFaqSection> ModelFaqSections { get; set; }

    public virtual DbSet<ModelPicture> ModelPictures { get; set; }

    public virtual DbSet<ModelReview> ModelReviews { get; set; }

    public virtual DbSet<ModelReviewType> ModelReviewTypes { get; set; }

    public virtual DbSet<PendingUser> PendingUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDevice> UserDevices { get; set; }

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
                .HasMaxLength(255)
                .HasColumnName("youtube_video_link");

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
            entity.Property(e => e.PictureHeight)
                .HasColumnType("int(11)")
                .HasColumnName("picture_height");
            entity.Property(e => e.PictureLocation)
                .HasMaxLength(254)
                .HasColumnName("picture_location");
            entity.Property(e => e.PictureWidth)
                .HasColumnType("int(11)")
                .HasColumnName("picture_width");

            entity.HasOne(d => d.CommunityPostUu).WithMany(p => p.CommunityPostPictures)
                .HasForeignKey(d => d.CommunityPostUuid)
                .HasConstraintName("community_post_pictures_ibfk_1");
        });

        modelBuilder.Entity<CommunityPostQuestionSection>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("community_post_question_section");

            entity.HasIndex(e => e.CommunityPostUuid, "community_post_uuid");

            entity.HasIndex(e => e.MessageTypeUuid, "message_type_uuid");

            entity.HasIndex(e => e.UserOriginUuid, "user_origin_uuid");

            entity.HasIndex(e => e.UserTargetUuid, "user_target_uuid");

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.CommunityPostUuid).HasColumnName("community_post_uuid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.MessageText)
                .HasColumnType("text")
                .HasColumnName("message_text");
            entity.Property(e => e.MessageTypeUuid).HasColumnName("message_type_uuid");
            entity.Property(e => e.UserOriginUuid).HasColumnName("user_origin_uuid");
            entity.Property(e => e.UserTargetUuid).HasColumnName("user_target_uuid");

            entity.HasOne(d => d.CommunityPostUu).WithMany(p => p.CommunityPostQuestionSections)
                .HasForeignKey(d => d.CommunityPostUuid)
                .HasConstraintName("community_post_question_section_ibfk_3");

            entity.HasOne(d => d.MessageTypeUu).WithMany(p => p.CommunityPostQuestionSections)
                .HasForeignKey(d => d.MessageTypeUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("community_post_question_section_ibfk_4");

            entity.HasOne(d => d.UserOriginUu).WithMany(p => p.CommunityPostQuestionSectionUserOriginUus)
                .HasForeignKey(d => d.UserOriginUuid)
                .HasConstraintName("community_post_question_section_ibfk_1");

            entity.HasOne(d => d.UserTargetUu).WithMany(p => p.CommunityPostQuestionSectionUserTargetUus)
                .HasForeignKey(d => d.UserTargetUuid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("community_post_question_section_ibfk_2");
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

        modelBuilder.Entity<MessageType>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("message_types");

            entity.HasIndex(e => e.MessageName, "message_name").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.MessageName)
                .HasMaxLength(50)
                .HasColumnName("message_name");
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

            entity.HasIndex(e => e.MessageTypeUuid, "message_type_uuid");

            entity.HasIndex(e => e.ModelUuid, "model_uuid");

            entity.HasIndex(e => e.UserOriginUuid, "user_origin_uuid");

            entity.HasIndex(e => e.UserTargetUuid, "user_target_uuid");

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.MessageText)
                .HasColumnType("text")
                .HasColumnName("message_text");
            entity.Property(e => e.MessageTypeUuid).HasColumnName("message_type_uuid");
            entity.Property(e => e.ModelUuid).HasColumnName("model_uuid");
            entity.Property(e => e.UserOriginUuid).HasColumnName("user_origin_uuid");
            entity.Property(e => e.UserTargetUuid).HasColumnName("user_target_uuid");

            entity.HasOne(d => d.MessageTypeUu).WithMany(p => p.ModelFaqSections)
                .HasForeignKey(d => d.MessageTypeUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("model_faq_section_ibfk_4");

            entity.HasOne(d => d.ModelUu).WithMany(p => p.ModelFaqSections)
                .HasForeignKey(d => d.ModelUuid)
                .HasConstraintName("model_faq_section_ibfk_3");

            entity.HasOne(d => d.UserOriginUu).WithMany(p => p.ModelFaqSectionUserOriginUus)
                .HasForeignKey(d => d.UserOriginUuid)
                .HasConstraintName("model_faq_section_ibfk_1");

            entity.HasOne(d => d.UserTargetUu).WithMany(p => p.ModelFaqSectionUserTargetUus)
                .HasForeignKey(d => d.UserTargetUuid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("model_faq_section_ibfk_2");
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
            entity.Property(e => e.PictureHeight)
                .HasColumnType("int(11)")
                .HasColumnName("picture_height");
            entity.Property(e => e.PictureLocation)
                .HasMaxLength(254)
                .HasColumnName("picture_location");
            entity.Property(e => e.PictureWidth)
                .HasColumnType("int(11)")
                .HasColumnName("picture_width");

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
        });

        modelBuilder.Entity<PendingUser>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("pending_users");

            entity.HasIndex(e => e.OtpCode, "otp_code").IsUnique();

            entity.HasIndex(e => e.UserUuid, "user_uuid");

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.OtpCode)
                .HasMaxLength(8)
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
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(255)
                .HasColumnName("password_salt");
            entity.Property(e => e.UserRoleUuid).HasColumnName("user_role_uuid");

            entity.HasOne(d => d.UserRoleUu).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_ibfk_1");
        });

        modelBuilder.Entity<UserDevice>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("user_devices");

            entity.HasIndex(e => e.DeviceName, "device_name").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.DeviceName)
                .HasMaxLength(50)
                .HasColumnName("device_name");
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
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("user_tokens");

            entity.HasIndex(e => new { e.UserUuid, e.RefreshToken }, "uq_user_refresh_token").IsUnique();

            entity.HasIndex(e => e.UserDeviceUuid, "user_device_uuid");

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");
            entity.Property(e => e.TokenCreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("token_created_at");
            entity.Property(e => e.TokenExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("token_expiration_date");
            entity.Property(e => e.UserDeviceUuid).HasColumnName("user_device_uuid");
            entity.Property(e => e.UserUuid).HasColumnName("user_uuid");

            entity.HasOne(d => d.UserDeviceUu).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserDeviceUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_tokens_ibfk_2");

            entity.HasOne(d => d.UserUu).WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserUuid)
                .HasConstraintName("user_tokens_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}