using System;
using System.Collections.Generic;

namespace JoyModels.Models.src.Database.Entities;

public partial class CommunityPostReviewType
{
    public Guid Uuid { get; set; }

    public string ReviewType { get; set; } = null!;

    public virtual ICollection<CommunityPostUserReview> CommunityPostUserReviews { get; set; } = new List<CommunityPostUserReview>();
}
