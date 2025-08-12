using System;
using System.Collections.Generic;

namespace JoyModels.Models.src.Database.Entities;

public partial class ModelReview
{
    public Guid Uuid { get; set; }

    public Guid ModelUuid { get; set; }

    public Guid UserUuid { get; set; }

    public Guid ReviewTypeUuid { get; set; }

    public string ReviewText { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Model ModelUu { get; set; } = null!;

    public virtual ModelReviewType ReviewTypeUu { get; set; } = null!;

    public virtual User UserUu { get; set; } = null!;
}
