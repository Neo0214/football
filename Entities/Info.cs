using System;
using System.Collections.Generic;

namespace football.Entities;

public partial class Info
{
    public int UserId { get; set; }

    public DateOnly? StartYear { get; set; }

    public DateOnly? EndYear { get; set; }

    public string? Status { get; set; }

    public string? Addition { get; set; }

    public int InfoId { get; set; }

    public virtual User User { get; set; } = null!;
}
