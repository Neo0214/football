using System;
using System.Collections.Generic;

namespace football.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? TeleNumber { get; set; }

    public string? Pwd { get; set; }

    public string? Address { get; set; }

    public string? AvatarId { get; set; }

    public virtual ICollection<Chat> Chat_1s { get; set; } = new List<Chat>();

    public virtual ICollection<Chat> Chat_2s { get; set; } = new List<Chat>();

    public virtual ICollection<Info> Infos { get; set; } = new List<Info>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();
}
