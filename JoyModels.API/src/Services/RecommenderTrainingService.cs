using JoyModels.Services.Services.Recommender;

namespace JoyModels.API.Services;

public class RecommenderTrainingService(
    IRecommenderService recommenderService,
    ILogger<RecommenderTrainingService> logger)
    : BackgroundService
{
    private readonly TimeSpan _trainingInterval = TimeSpan.FromHours(6);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!TryLoadModel())
        {
            await TrainModelAsync();
        }

        using var timer = new PeriodicTimer(_trainingInterval);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            await TrainModelAsync();
        }
    }

    private bool TryLoadModel()
    {
        logger.LogInformation("Attempting to load recommender model from disk...");

        var loaded = recommenderService.LoadModel();

        if (loaded)
        {
            logger.LogInformation("Recommender model loaded successfully from disk.");
        }
        else
        {
            logger.LogInformation("No saved model found. Will train a new model.");
        }

        return loaded;
    }

    private async Task TrainModelAsync()
    {
        try
        {
            logger.LogInformation("Starting recommender model training...");
            await recommenderService.TrainModel();

            if (recommenderService.IsModelTrained)
            {
                logger.LogInformation("Recommender model training completed successfully.");
            }
            else
            {
                logger.LogWarning(
                    "Recommender model training skipped - insufficient data (need at least 10 interactions).");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during recommender model training.");
        }
    }
}