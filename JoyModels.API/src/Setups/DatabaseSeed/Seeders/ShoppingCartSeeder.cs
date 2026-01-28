using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JoyModels.API.Setups.DatabaseSeed.Seeders;

public static class ShoppingCartSeeder
{
    public static async Task SeedShoppingCart(JoyModelsDbContext context, ILogger logger)
    {
        var existingCartCount = await context.ShoppingCartItems.CountAsync();
        if (existingCartCount > 0)
        {
            logger.LogInformation("Database already contains {Count} cart items. Skipping cart seeding.",
                existingCartCount);
            return;
        }

        logger.LogInformation("Starting ShoppingCart seeding...");

        var regularUsers = await context.Users
            .Include(u => u.UserRoleUu)
            .Where(u => u.UserRoleUu.RoleName == nameof(UserRoleEnum.User))
            .OrderBy(u => u.CreatedAt)
            .ToListAsync();
        var models = await context.Models.OrderBy(m => m.CreatedAt).ToListAsync();

        var ownedModels = await context.Libraries
            .Select(l => new { l.UserUuid, l.ModelUuid })
            .ToListAsync();
        var ownedSet = ownedModels.ToHashSet();

        var cartItems = new List<ShoppingCart>();
        var cartPairs = new HashSet<(Guid UserUuid, Guid ModelUuid)>();

        const int groupCount = 5;
        var modelsPerGroup = models.Count / groupCount;
        var usersPerGroup = regularUsers.Count / groupCount;

        for (var userIndex = 0; userIndex < regularUsers.Count; userIndex++)
        {
            var user = regularUsers[userIndex];
            var userGroup = userIndex / usersPerGroup;
            if (userGroup >= groupCount) userGroup = groupCount - 1;

            for (var modelIndex = 0; modelIndex < models.Count; modelIndex++)
            {
                var model = models[modelIndex];
                if (model.UserUuid == user.Uuid)
                    continue;

                if (ownedSet.Contains(new { UserUuid = user.Uuid, ModelUuid = model.Uuid }))
                    continue;

                var modelGroup = modelIndex / modelsPerGroup;
                if (modelGroup >= groupCount) modelGroup = groupCount - 1;

                var groupDistance = Math.Abs(userGroup - modelGroup);

                double cartChance = groupDistance switch
                {
                    0 => 0.08,
                    1 => 0.03,
                    _ => 0.005
                };

                if (SeedDataConstants.Random.NextDouble() < cartChance)
                {
                    if (cartPairs.Add((user.Uuid, model.Uuid)))
                    {
                        cartItems.Add(new ShoppingCart
                        {
                            Uuid = Guid.NewGuid(),
                            UserUuid = user.Uuid,
                            ModelUuid = model.Uuid,
                            CreatedAt = DateTime.UtcNow.AddDays(-SeedDataConstants.Random.Next(1, 30))
                        });
                    }
                }
            }
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.ShoppingCartItems.AddRangeAsync(cartItems);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            logger.LogInformation("ShoppingCart seeding completed. Created {Count} cart items.", cartItems.Count);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "ShoppingCart seeding failed. Rolling back transaction.");
            throw;
        }
    }
}