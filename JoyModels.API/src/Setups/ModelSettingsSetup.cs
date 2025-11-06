using JoyModels.Models.DataTransferObjects.ModelSettings;

namespace JoyModels.API.Setups;

public static class ModelSettingsSetup
{
    public static ModelSettingsDetails RegisterModelSettingsDetails(IConfiguration configuration)
    {
        var modelSettingsDetails = configuration.GetSection("ModelSettings");

        var modelSettingsAllowedSize = modelSettingsDetails["AllowedSize"];
        var modelSettingsAllowedFormats = modelSettingsDetails["AllowedFormats"];
        var modelSettingsSavePath = modelSettingsDetails["SavePath"];

        if (string.IsNullOrWhiteSpace(modelSettingsAllowedSize)
            || string.IsNullOrWhiteSpace(modelSettingsAllowedFormats)
            || string.IsNullOrWhiteSpace(modelSettingsSavePath))
            throw new ApplicationException("Model settings details are not configured!");

        return new ModelSettingsDetails()
        {
            AllowedSize = int.Parse(modelSettingsAllowedSize),
            AllowedFormats = modelSettingsAllowedFormats.Split(',').ToList(),
            SavePath = modelSettingsSavePath
        };
    }
}