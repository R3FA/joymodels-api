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
        await TrainModelAsync();

        using var timer = new PeriodicTimer(_trainingInterval);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            await TrainModelAsync();
        }
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