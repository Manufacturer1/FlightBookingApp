﻿using Microsoft.AspNetCore.Identity;

namespace BaseEntity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
