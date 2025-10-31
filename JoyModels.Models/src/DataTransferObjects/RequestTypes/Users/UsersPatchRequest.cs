namespace JoyModels.Models.DataTransferObjects.RequestTypes.Users;

public class UsersPatchRequest
{
    public Guid UserUuid { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Nickname { get; set; }
}