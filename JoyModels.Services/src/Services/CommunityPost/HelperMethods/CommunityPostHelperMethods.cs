using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.CommunityPost.HelperMethods;

public static class CommunityPostHelperMethods
{
    public static async Task<JoyModels.Models.Database.Entities.CommunityPost> GetCommunityPostEntity(
        JoyModelsDbContext context,
        Guid communityPostUuid)
    {
        var communityPostEntity = await context.CommunityPosts
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.PostTypeUu)
            .Include(x => x.CommunityPostPictures)
            .FirstOrDefaultAsync(x => x.Uuid == communityPostUuid);

        return communityPostEntity ?? throw new KeyNotFoundException("Community post with sent values is not found.");
    }

    public static async Task<List<string>> SaveCommunityPostPictures(this List<IFormFile> communityPostPictures,
        ModelImageSettingsDetails modelImageSettingsDetails, Guid communityPostUuid)
    {
        var communityPostPicturePaths = new List<string>(communityPostPictures.Count);

        try
        {
            foreach (var communityPostPicture in communityPostPictures)
            {
                var communityPostPictureName =
                    $"community-post-picture-{Guid.NewGuid()}{Path.GetExtension(communityPostPicture.FileName)}";

                var basePath =
                    Directory.CreateDirectory(Path.Combine(modelImageSettingsDetails.SavePath, "community-posts",
                        communityPostUuid.ToString()));
                var communityPostPicturePath = Path.Combine(basePath.FullName, communityPostPictureName);

                await using var stream = new FileStream(communityPostPicturePath, FileMode.Create);
                await communityPostPicture.CopyToAsync(stream);

                communityPostPicturePaths.Add(communityPostPicturePath);
            }
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Failed to save community post picture: {e.Message}");
        }

        return communityPostPicturePaths;
    }

    public static List<CommunityPostPicture> CreateCommunityPostPictureEntities(Guid communityPostUuid,
        List<string> communityPostPicturePaths)
    {
        var communityPostPictureEntities = new List<CommunityPostPicture>(communityPostPicturePaths.Count);

        foreach (var communityPostPicturePath in communityPostPicturePaths)
        {
            communityPostPictureEntities.Add(new CommunityPostPicture()
            {
                Uuid = Guid.NewGuid(),
                CommunityPostUuid = communityPostUuid,
                PictureLocation = communityPostPicturePath,
                CreatedAt = DateTime.Now
            });
        }

        return communityPostPictureEntities;
    }

    public static async Task CreateCommunityPostEntity(this JoyModels.Models.Database.Entities.CommunityPost entity,
        JoyModelsDbContext context)
    {
        await context.CommunityPosts.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public static async Task CreateCommunityPostPictureEntities(
        this List<CommunityPostPicture>? entities,
        JoyModelsDbContext context)
    {
        if (entities != null && entities.Count > 0)
        {
            foreach (var entity in entities)
            {
                await context.CommunityPostPictures.AddAsync(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}