using System;
using System.Collections.Generic;

namespace BuzzTalk.Data.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Username { get; set; } = null!;

    public DateTime? JoinedOn { get; set; }

    public string? Fcm { get; set; }

    public virtual ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Message> MessageFroms { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageTos { get; set; } = new List<Message>();
}
