using System;
using System.Collections.Generic;

namespace football.Entities;

public partial class Nation
{
    public int NationId { get; set; }

    public string? NationName { get; set; }

    public int? Score { get; set; }

    public string? AvatarId { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
