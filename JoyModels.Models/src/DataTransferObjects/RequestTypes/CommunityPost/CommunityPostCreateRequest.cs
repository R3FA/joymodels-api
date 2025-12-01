using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;

public class CommunityPostCreateRequest
{
    [Required, MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(5000, ErrorMessage = "Description cannot exceed 5000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required] public Guid PostTypeUuid { get; set; }

    [MaxLength(2048, ErrorMessage = "YoutubeVideoLink cannot exceed 2048 characters.")]
    public string? YoutubeVideoLink { get; set; }

    [MaxLength(4, ErrorMessage = "You can only add up to 4 pictures.")]
    public List<IFormFile>? Pictures { get; set; }
}