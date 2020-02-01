using ContactsAppApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ContactsAppApi.Data.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(u => u.Id)
                .HasDefaultValueSql("NEWID()");

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
                 .Property(x => x.Surname)
                .HasMaxLength(100);

            builder
                .Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .HasIndex(x => x.Phone)
                .IsUnique();

            builder
                .Property(x => x.Email)
                .HasMaxLength(100);

            builder.HasData(GetInitialContacts());
        }

        private IEnumerable<Contact> GetInitialContacts()
        {
            var json = File.ReadAllText("contacts.json");
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            var contacts = JsonSerializer.Deserialize<List<Contact>>(json, options);
            return contacts;
        }
    }
}
