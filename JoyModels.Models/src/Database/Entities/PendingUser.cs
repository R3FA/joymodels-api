using System;
using System.Collections.Generic;

namespace JoyModels.Models.src.Database.Entities;

public partial class PendingUser
{
    public Guid Uuid { get; set; }

    public Guid UserUuid { get; set; }

    public string OtpCode { get; set; } = null!;

    public DateTime OtpCreatedAt { get; set; }

    public DateTime OtpExpirationDate { get; set; }

    public virtual User UserUu { get; set; } = null!;
}