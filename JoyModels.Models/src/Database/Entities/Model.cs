namespace JoyModels.Models.Database.Entities;

public partial class Model
{
    public Guid Uuid { get; set; }

    public string Name { get; set; } = null!;

    public Guid UserUuid { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public Guid ModelAvailabilityUuid { get; set; }

    public virtual ModelAvailability ModelAvailabilityUu { get; set; } = null!;

    public virtual ICollection<ModelCategory> ModelCategories { get; set; } = new List<ModelCategory>();

    public virtual ICollection<ModelFaqSection> ModelFaqSections { get; set; } = new List<ModelFaqSection>();

    public virtual ICollection<ModelPicture> ModelPictures { get; set; } = new List<ModelPicture>();

    public virtual ICollection<ModelReview> ModelReviews { get; set; } = new List<ModelReview>();

    public virtual ICollection<UserModelLike> UserModelLikes { get; set; } = new List<UserModelLike>();

    public virtual User UserUu { get; set; } = null!;
}