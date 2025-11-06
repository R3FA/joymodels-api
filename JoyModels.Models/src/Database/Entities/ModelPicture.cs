namespace JoyModels.Models.Database.Entities;

public partial class ModelPicture
{
    public Guid Uuid { get; set; }

    public Guid ModelUuid { get; set; }

    public string PictureLocation { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Model ModelUu { get; set; } = null!;
}