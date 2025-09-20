namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoLoginResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}