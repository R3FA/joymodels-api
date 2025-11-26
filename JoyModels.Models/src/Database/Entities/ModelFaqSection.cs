namespace JoyModels.Models.Database.Entities;

public partial class ModelFaqSection
{
    public Guid Uuid { get; set; }

    public Guid? ParentMessageUuid { get; set; }

    public Guid UserUuid { get; set; }

    public Guid ModelUuid { get; set; }

    public string MessageText { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ModelFaqSection? ParentMessage { get; set; }

    public virtual ICollection<ModelFaqSection> Replies { get; set; } = new List<ModelFaqSection>();

    public virtual Model ModelUu { get; set; } = null!;

    public virtual User UserUu { get; set; } = null!;
}