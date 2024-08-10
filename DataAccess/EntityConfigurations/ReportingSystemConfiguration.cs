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
    public class ReportingSystemConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Reports").HasKey(r => r.Id);

            builder.Property(r => r.Id).HasColumnName("Id").IsRequired();
            builder.Property(r => r.Title).HasColumnName("Title").IsRequired();
            builder.Property(r => r.Content).HasColumnName("Content").IsRequired();

            builder.HasOne(r => r.Project)
             .WithMany(r => r.Reports)
             .HasForeignKey(r => r.ProjectId);

            builder.HasOne(r => r.Task)
             .WithMany(r => r.Reports)
             .HasForeignKey(r => r.TaskId);

            builder.HasIndex(indexExpression: r => r.Id, name: "UK_Id").IsUnique();
            builder.HasQueryFilter(r => !r.DeletedDate.HasValue);
        }
    }
}
