using ContactsAppApi.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ContactsAppApi.Data
{
    public class ContactsAppDbContext : IdentityDbContext<IdentityUser>
    {
        public ContactsAppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddUpdatedDateTime();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddUpdatedDateTime()
        { 
           ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && x.State == EntityState.Modified).ToList()
                .ForEach(x => (x.Entity as BaseEntity).UpdatedAt = DateTime.Now);
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<AdditionalPhone> AdditionalPhones { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
