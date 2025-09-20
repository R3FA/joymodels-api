namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoTokenResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}