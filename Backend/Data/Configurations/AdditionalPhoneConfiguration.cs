using ContactsAppApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsAppApi.Data.Configurations
{
    public class AdditionalPhoneConfiguration : IEntityTypeConfiguration<AdditionalPhone>
    {
        public void Configure(EntityTypeBuilder<AdditionalPhone> builder)
        {
            builder
                .HasKey(x => x.Phone);

            builder
                .Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(x => x.ContactId)
                .IsRequired();
        }
    }
}
