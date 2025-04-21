using System;
using System.Collections.Generic;

namespace BuzzTalk.Data.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Username { get; set; } = null!;

    public DateTime? JoinedOn { get; set; }

    public virtual ICollection<Message> MessageFroms { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageTos { get; set; } = new List<Message>();
}
