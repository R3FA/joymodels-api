namespace JoyModels.Services.Services.Recommender;

public interface IRecommenderService
{
    Task TrainModel();
    Task<List<Guid>> GetRecommendations(Guid userUuid, int count);
    bool IsModelTrained { get; }
}