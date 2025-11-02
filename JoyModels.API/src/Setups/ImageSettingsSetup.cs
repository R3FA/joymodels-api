using JoyModels.Models.DataTransferObjects.ImageSettings;

namespace JoyModels.API.Setups;

public static class ImageSettingsSetup
{
    public static ImageSettingsDetails RegisterImageSettingsDetails(IConfiguration configuration)
    {
        var imageSettingsDetails = configuration.GetSection("ImageSettings");
        var imageSettingsAllowedSize = imageSettingsDetails["AllowedSize"];
        var imageSettingsAllowedExtensions = imageSettingsDetails["AllowedExtensions"];
        var imageSettingsSavePath = imageSettingsDetails["SavePath"];

        if (string.IsNullOrWhiteSpace(imageSettingsAllowedSize)
            || string.IsNullOrWhiteSpace(imageSettingsAllowedExtensions)
            || string.IsNullOrWhiteSpace(imageSettingsSavePath))
            throw new ApplicationException("Image settings details are not configured!");

        return new ImageSettingsDetails()
        {
            AllowedSize = int.Parse(imageSettingsAllowedSize),
            AllowedExtensions = imageSettingsAllowedExtensions.Split(","),
            SavePath = imageSettingsSavePath
        };
    }
}