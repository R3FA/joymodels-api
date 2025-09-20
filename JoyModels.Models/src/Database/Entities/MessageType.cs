namespace JoyModels.Models.Database.Entities;

public partial class MessageType
{
    public Guid Uuid { get; set; }

    public string MessageName { get; set; } = null!;

    public virtual ICollection<CommunityPostQuestionSection> CommunityPostQuestionSections { get; set; } = new List<CommunityPostQuestionSection>();

    public virtual ICollection<ModelFaqSection> ModelFaqSections { get; set; } = new List<ModelFaqSection>();
}
