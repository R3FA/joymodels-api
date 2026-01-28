using JoyModels.Models.DataTransferObjects.RecommenderSettings;

namespace JoyModels.API.Setups;

public static class RecommenderSettingsSetup
{
    public static RecommenderSettingsDetails RegisterRecommenderSettingsDetails(IConfiguration configuration)
    {
        var recommenderSettings = configuration.GetSection("RecommenderSettings");

        var modelPath = recommenderSettings["ModelPath"];

        if (string.IsNullOrWhiteSpace(modelPath))
            throw new ApplicationException("Recommender settings are not configured!");

        return new RecommenderSettingsDetails
        {
            ModelPath = modelPath
        };
    }
}