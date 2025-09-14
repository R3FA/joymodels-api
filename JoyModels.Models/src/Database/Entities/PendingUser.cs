using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyModels.Models.src.Database.Entities;

public partial class PendingUser
{
    public Guid Uuid { get; set; }

    public Guid UserUuid { get; set; }

    [Column(TypeName = "char(12)")]
    [MaxLength(12)]
    public string OtpCode { get; set; } = null!;

    public DateTime OtpCreatedAt { get; set; }

    public DateTime OtpExpirationDate { get; set; }

    public virtual User? UserUu { get; set; }
}