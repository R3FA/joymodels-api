using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Recommender;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Recommender;
using JoyModels.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace JoyModels.Services.Services.Recommender;

public class RecommenderService(IDbContextFactory<JoyModelsDbContext> contextFactory) : IRecommenderService
{
    private readonly MLContext _mlContext = new(seed: 42);
    private ITransformer? _trainedModel;
    private PredictionEngine<ModelInteractionRequest, ModelInteractionResponse>? _predictionEngine;
    private Dictionary<Guid, uint> _userIdMap = new();
    private Dictionary<Guid, uint> _modelIdMap = new();
    private readonly object _lock = new();

    public bool IsModelTrained => _trainedModel != null;

    public async Task TrainModel()
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        var newUserIdMap = new Dictionary<Guid, uint>();
        var newModelIdMap = new Dictionary<Guid, uint>();

        var trainingData = await GetTrainingDataAsync(context, newUserIdMap, newModelIdMap);

        if (trainingData.Count < 10)
        {
            return;
        }

        var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

        var options = new MatrixFactorizationTrainer.Options
        {
            MatrixColumnIndexColumnName = nameof(ModelInteractionRequest.UserId),
            MatrixRowIndexColumnName = nameof(ModelInteractionRequest.ModelId),
            LabelColumnName = nameof(ModelInteractionRequest.Score),
            NumberOfIterations = 20,
            ApproximationRank = 32,
            LearningRate = 0.1
        };

        var trainer = _mlContext.Recommendation().Trainers.MatrixFactorization(options);
        var trainedModel = trainer.Fit(dataView);
        var predictionEngine =
            _mlContext.Model.CreatePredictionEngine<ModelInteractionRequest, ModelInteractionResponse>(trainedModel);

        lock (_lock)
        {
            _trainedModel = trainedModel;
            _predictionEngine = predictionEngine;
            _userIdMap = newUserIdMap;
            _modelIdMap = newModelIdMap;
        }
    }

    public async Task<List<Guid>> GetRecommendations(Guid userUuid, int count)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        uint userId = 0;
        Dictionary<Guid, uint>? modelIdMapSnapshot = null;
        bool useFallback;

        lock (_lock)
        {
            if (_trainedModel == null || _predictionEngine == null)
            {
                return [];
            }

            if (!_userIdMap.TryGetValue(userUuid, out userId))
            {
                useFallback = true;
            }
            else
            {
                useFallback = false;
                modelIdMapSnapshot = new Dictionary<Guid, uint>(_modelIdMap);
            }
        }

        if (useFallback)
        {
            return await GetFallbackRecommendationsAsync(context, userUuid, count);
        }

        var userInteractedModelUuids = await GetUserInteractedModelsAsync(context, userUuid);

        var publicModelUuids = await context.Models
            .AsNoTracking()
            .Where(m => m.ModelAvailabilityUu.AvailabilityName == nameof(ModelAvailabilityEnum.Public))
            .Select(m => m.Uuid)
            .ToListAsync();

        var candidateModels = publicModelUuids
            .Except(userInteractedModelUuids)
            .ToList();

        var predictions = new List<(Guid ModelUuid, float Score)>();

        lock (_lock)
        {
            if (_predictionEngine == null)
            {
                return new List<Guid>();
            }

            foreach (var modelUuid in candidateModels)
            {
                if (!modelIdMapSnapshot!.TryGetValue(modelUuid, out var modelId))
                    continue;

                var prediction = _predictionEngine.Predict(new ModelInteractionRequest
                {
                    UserId = userId,
                    ModelId = modelId
                });

                if (!float.IsNaN(prediction.Score))
                {
                    predictions.Add((modelUuid, prediction.Score));
                }
            }
        }

        var result = predictions
            .OrderByDescending(p => p.Score)
            .Take(count)
            .Select(p => p.ModelUuid)
            .ToList();

        if (result.Count == 0)
        {
            return await GetFallbackRecommendationsAsync(context, userUuid, count);
        }

        return result;
    }

    private async Task<List<ModelInteractionRequest>> GetTrainingDataAsync(
        JoyModelsDbContext context,
        Dictionary<Guid, uint> userIdMap,
        Dictionary<Guid, uint> modelIdMap)
    {
        var interactions = new List<ModelInteractionRequest>();

        uint userCounter = 1;
        uint modelCounter = 1;

        var purchases = await context.Orders
            .Where(o => o.Status == nameof(OrderStatus.Completed))
            .Select(o => new { o.UserUuid, o.ModelUuid })
            .ToListAsync();

        foreach (var purchase in purchases)
        {
            var userId = GetOrCreateId(userIdMap, purchase.UserUuid, ref userCounter);
            var modelId = GetOrCreateId(modelIdMap, purchase.ModelUuid, ref modelCounter);

            interactions.Add(new ModelInteractionRequest
            {
                UserId = userId,
                ModelId = modelId,
                Score = 1.0f
            });
        }

        var likes = await context.UserModelLikes
            .Select(l => new { l.UserUuid, l.ModelUuid })
            .ToListAsync();

        foreach (var like in likes)
        {
            var userId = GetOrCreateId(userIdMap, like.UserUuid, ref userCounter);
            var modelId = GetOrCreateId(modelIdMap, like.ModelUuid, ref modelCounter);

            var exists = interactions.Any(i => i.UserId == userId && i.ModelId == modelId);
            if (!exists)
            {
                interactions.Add(new ModelInteractionRequest
                {
                    UserId = userId,
                    ModelId = modelId,
                    Score = 0.6f
                });
            }
        }

        var cartItems = await context.ShoppingCartItems
            .Select(c => new { c.UserUuid, c.ModelUuid })
            .ToListAsync();

        foreach (var cartItem in cartItems)
        {
            var userId = GetOrCreateId(userIdMap, cartItem.UserUuid, ref userCounter);
            var modelId = GetOrCreateId(modelIdMap, cartItem.ModelUuid, ref modelCounter);

            var exists = interactions.Any(i => i.UserId == userId && i.ModelId == modelId);
            if (!exists)
            {
                interactions.Add(new ModelInteractionRequest
                {
                    UserId = userId,
                    ModelId = modelId,
                    Score = 0.3f
                });
            }
        }

        return interactions;
    }

    private static uint GetOrCreateId(Dictionary<Guid, uint> map, Guid uuid, ref uint counter)
    {
        if (!map.TryGetValue(uuid, out var id))
        {
            id = counter++;
            map[uuid] = id;
        }

        return id;
    }

    private async Task<HashSet<Guid>> GetUserInteractedModelsAsync(JoyModelsDbContext context, Guid userUuid)
    {
        var purchased = await context.Orders
            .Where(o => o.UserUuid == userUuid && o.Status == nameof(OrderStatus.Completed))
            .Select(o => o.ModelUuid)
            .ToListAsync();

        var liked = await context.UserModelLikes
            .Where(l => l.UserUuid == userUuid)
            .Select(l => l.ModelUuid)
            .ToListAsync();

        var inCart = await context.ShoppingCartItems
            .Where(c => c.UserUuid == userUuid)
            .Select(c => c.ModelUuid)
            .ToListAsync();

        return purchased.Union(liked).Union(inCart).ToHashSet();
    }

    private async Task<List<Guid>> GetFallbackRecommendationsAsync(JoyModelsDbContext context, Guid userUuid, int count)
    {
        var userInteracted = await GetUserInteractedModelsAsync(context, userUuid);

        return await context.Models
            .AsNoTracking()
            .Where(m => m.ModelAvailabilityUu.AvailabilityName == nameof(ModelAvailabilityEnum.Public))
            .Where(m => !userInteracted.Contains(m.Uuid))
            .OrderByDescending(m =>
                context.Orders.Count(o => o.ModelUuid == m.Uuid && o.Status == nameof(OrderStatus.Completed)))
            .Take(count)
            .Select(m => m.Uuid)
            .ToListAsync();
    }
}