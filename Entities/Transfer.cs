using System;
using System.Collections.Generic;

namespace football.Entities;

public partial class Transfer
{
    public int Id { get; set; }

    public int From { get; set; }

    public int To { get; set; }

    public int? Cost { get; set; }

    public virtual Club FromNavigation { get; set; } = null!;

    public virtual Club ToNavigation { get; set; } = null!;
}
