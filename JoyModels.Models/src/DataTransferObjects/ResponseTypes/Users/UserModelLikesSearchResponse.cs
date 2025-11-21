using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

public class UserModelLikesSearchResponse
{
    public Guid Uuid { get; set; }
    public ModelResponse ModelResponse { get; set; } = null!;
}