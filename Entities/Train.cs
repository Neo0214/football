using System;
using System.Collections.Generic;

namespace football.Entities;

public partial class Train
{
    public int TrainId { get; set; }

    public DateOnly? TrainDate { get; set; }

    public int? Score { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
