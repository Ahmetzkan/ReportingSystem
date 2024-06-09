using Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Entities.Concretes.Task;

namespace DataAccess.EntityConfigurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            {
                builder.ToTable("Tasks").HasKey(t => t.Id);

                builder.Property(t => t.Id).HasColumnName("Id").IsRequired();
                builder.Property(t => t.Description).HasColumnName("Description").IsRequired();
                builder.Property(t => t.Status).HasColumnName("Status").IsRequired();

                builder.HasMany(t => t.Reports)
                 .WithOne(t => t.Task)
                 .HasForeignKey(t => t.TaskId);

                builder.HasIndex(indexExpression: t => t.Id, name: "UK_Id").IsUnique();
                builder.HasQueryFilter(t => !t.DeletedDate.HasValue);
            }
        }
    }
}