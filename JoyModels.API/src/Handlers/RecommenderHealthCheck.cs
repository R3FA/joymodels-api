using JoyModels.Services.Services.Recommender;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace JoyModels.API.Handlers;

public class RecommenderHealthCheck(IRecommenderService recommenderService) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(recommenderService.IsModelTrained
            ? HealthCheckResult.Healthy("Recommender model is trained.")
            : HealthCheckResult.Unhealthy("Recommender model is not yet trained."));
    }
}