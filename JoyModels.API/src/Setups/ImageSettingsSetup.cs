using JoyModels.Models.DataTransferObjects.ImageSettings;

namespace JoyModels.API.Setups;

public static class ImageSettingsSetup
{
    public static ImageSettingsDetails RegisterModelImageSettingsDetails(IConfiguration configuration)
    {
        var imageSettingsDetails = configuration.GetSection("ImageSettings");
        var imageSettingsResolutionSize = configuration.GetSection("ImageSettings:ImageSettingsModel");

        var imageSettingsAllowedSize = imageSettingsDetails["AllowedSize"];
        var imageSettingsSavePath = imageSettingsDetails["SavePath"];
        var imageSettingsResolutionMinimumWidthSize = imageSettingsResolutionSize["MinimumWidth"];
        var imageSettingsResolutionMinimumHeightSize = imageSettingsResolutionSize["MinimumHeight"];
        var imageSettingsResolutionMaximumWidthSize = imageSettingsResolutionSize["MaximumWidth"];
        var imageSettingsResolutionMaximumHeightSize = imageSettingsResolutionSize["MaximumHeight"];

        if (string.IsNullOrWhiteSpace(imageSettingsAllowedSize)
            || string.IsNullOrWhiteSpace(imageSettingsSavePath)
            || string.IsNullOrWhiteSpace(imageSettingsResolutionMinimumWidthSize)
            || string.IsNullOrWhiteSpace(imageSettingsResolutionMinimumHeightSize)
            || string.IsNullOrWhiteSpace(imageSettingsResolutionMaximumWidthSize)
            || string.IsNullOrWhiteSpace(imageSettingsResolutionMaximumHeightSize))
            throw new ApplicationException("Image settings details are not configured!");

        return new ImageSettingsDetails()
        {
            ImageSettingsResolutionDetails = new ImageSettingsResolutionDetails
            {
                MinimumWidth = int.Parse(imageSettingsResolutionMinimumWidthSize),
                MinimumHeight = int.Parse(imageSettingsResolutionMinimumHeightSize),
                MaximumWidth = int.Parse(imageSettingsResolutionMaximumWidthSize),
                MaximumHeight = int.Parse(imageSettingsResolutionMaximumHeightSize)
            },
            AllowedSize = int.Parse(imageSettingsAllowedSize),
            SavePath = imageSettingsSavePath
        };
    }
}