using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.ModelFaqSection;

public class ModelFaqSectionResponse
{
    public ModelFaqSectionResponse? ParentMessage { get; set; }
    public Guid Uuid { get; set; }
    public UsersResponse User { get; set; } = null!;
    public ModelResponse Model { get; set; } = null!;
    public string MessageText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<ModelFaqSectionResponse>? Replies { get; set; }
}