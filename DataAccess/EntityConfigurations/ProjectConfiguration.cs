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
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects").HasKey(p => p.Id);

            builder.Property(p => p.Id).HasColumnName("Id").IsRequired();
            builder.Property(p => p.Name).HasColumnName("Name").IsRequired();
            builder.Property(p => p.StartDate).HasColumnName("StartDate").IsRequired();
            builder.Property(p => p.EndDate).HasColumnName("EndDate").IsRequired();
            builder.Property(p => p.Status).HasColumnName("Status").IsRequired();

            builder.HasIndex(indexExpression: t => t.Id, name: "UK_Id").IsUnique();
            builder.HasQueryFilter(p=> !p.DeletedDate.HasValue);
        }
    }
}
