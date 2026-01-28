using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JoyModels.API.Setups.DatabaseSeed.Seeders;

public static class OrderAndLibrarySeeder
{
    public static async Task SeedOrdersAndLibrary(JoyModelsDbContext context, ILogger logger)
    {
        var existingOrdersCount = await context.Orders.CountAsync();
        if (existingOrdersCount > 0)
        {
            logger.LogInformation("Database already contains {Count} orders. Skipping orders seeding.",
                existingOrdersCount);
            return;
        }

        logger.LogInformation("Starting Orders and Library seeding...");

        var regularUsers = await context.Users
            .Include(u => u.UserRoleUu)
            .Where(u => u.UserRoleUu.RoleName == nameof(UserRoleEnum.User))
            .OrderBy(u => u.CreatedAt)
            .ToListAsync();
        var models = await context.Models.OrderBy(m => m.CreatedAt).ToListAsync();

        var orders = new List<Order>();
        var libraryEntries = new List<Library>();
        var purchasedPairs = new HashSet<(Guid UserUuid, Guid ModelUuid)>();

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

                var modelGroup = modelIndex / modelsPerGroup;
                if (modelGroup >= groupCount) modelGroup = groupCount - 1;

                var groupDistance = Math.Abs(userGroup - modelGroup);

                double purchaseChance = groupDistance switch
                {
                    0 => 0.25,
                    1 => 0.10,
                    _ => 0.02
                };

                if (SeedDataConstants.Random.NextDouble() < purchaseChance)
                {
                    if (purchasedPairs.Add((user.Uuid, model.Uuid)))
                    {
                        var orderUuid = Guid.NewGuid();
                        var purchaseDate = DateTime.UtcNow.AddDays(-SeedDataConstants.Random.Next(1, 180));

                        orders.Add(new Order
                        {
                            Uuid = orderUuid,
                            UserUuid = user.Uuid,
                            ModelUuid = model.Uuid,
                            Amount = model.Price,
                            Status = nameof(OrderStatus.Completed),
                            StripePaymentIntentId = $"seed_pi_{Guid.NewGuid()}",
                            CreatedAt = purchaseDate,
                            UpdatedAt = purchaseDate
                        });

                        libraryEntries.Add(new Library
                        {
                            Uuid = Guid.NewGuid(),
                            UserUuid = user.Uuid,
                            ModelUuid = model.Uuid,
                            OrderUuid = orderUuid,
                            AcquiredAt = purchaseDate
                        });
                    }
                }
            }
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();

            await context.Libraries.AddRangeAsync(libraryEntries);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            logger.LogInformation(
                "Orders and Library seeding completed. Created {Orders} orders and {Libraries} library entries.",
                orders.Count, libraryEntries.Count);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "Orders and Library seeding failed. Rolling back transaction.");
            throw;
        }
    }
}