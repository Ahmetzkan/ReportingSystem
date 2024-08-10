using Core.Entities;
using Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityConfigurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Property(r => r.Id).HasColumnName("Id").IsRequired();
            builder.Property(r => r.UserId).HasColumnName("UserId").IsRequired();
            builder.Property(r => r.Token).HasColumnName("Token").IsRequired();
            builder.Property(r => r.CreatedByIp).HasColumnName("CreatedByIp").IsRequired();

            builder.HasIndex(indexExpression: r => r.Id, name: "UK_Id").IsUnique();
            builder.HasQueryFilter(r => !r.DeletedDate.HasValue);
        }
    }
}
