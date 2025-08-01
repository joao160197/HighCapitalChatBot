using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;

namespace HighCapitalBot.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public virtual ICollection<Bot> Bots { get; set; } = new List<Bot>();
    }
}
