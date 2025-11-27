using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.ModelFaqSection;

public class ModelFaqSectionResponse
{
    public Guid Uuid { get; set; }
    public string MessageText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public UsersResponse User { get; set; } = null!;
    public ModelResponse Model { get; set; } = null!;
    public ModelFaqSectionParentDto? ParentMessage { get; set; }
    public List<ModelFaqSectionReplyDto>? Replies { get; set; }
}

public class ModelFaqSectionParentDto
{
    public Guid Uuid { get; set; }
    public string MessageText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public UsersResponse User { get; set; } = null!;
}

public class ModelFaqSectionReplyDto
{
    public Guid Uuid { get; set; }
    public string MessageText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public UsersResponse User { get; set; } = null!;
}