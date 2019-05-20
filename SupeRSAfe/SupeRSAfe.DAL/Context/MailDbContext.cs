using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SupeRSAfe.DAL.Entities;

namespace SupeRSAfe.DAL.Context
{
    public class MailDbContext: IdentityDbContext<User>
    {
        public DbSet<Email> Emails { get; set; }

        public DbSet<Key> Keys { get; set; }

        public MailDbContext() { }

        public MailDbContext(DbContextOptions<MailDbContext> dbContextOptions): base(dbContextOptions)
        {
            Database.EnsureCreated();
        }
    }
}
