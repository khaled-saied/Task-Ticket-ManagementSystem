using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Enums;

namespace DAL.Data.Configurations
{
    class TaskConfigrations : IEntityTypeConfiguration<TaskK>
    {
        public void Configure(EntityTypeBuilder<TaskK> builder)
        {
            builder.HasOne(t => t.Project)
                   .WithMany(p => p.Tasks)
                   .HasForeignKey(t => t.ProjectId);

            builder.Property(T => T.Status).HasConversion(
                (type) => type.ToString(),
                (type) => (TaskStatusEnum)Enum.Parse(typeof(TaskStatusEnum), type) // Convert string to enum
                );
        }
    }
}
