using System;
using System.Collections.Generic;

namespace JoyModels.Models.src.Database.Entities;

public partial class ModelReviewType
{
    public Guid Uuid { get; set; }

    public string ReviewType { get; set; } = null!;

    public virtual ICollection<ModelReview> ModelReviews { get; set; } = new List<ModelReview>();
}
