using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations
{
    class ProjectConfigrations :IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(P => P.Id)
                .UseIdentityColumn(10, 10);

            builder.HasMany(P => P.Tasks)
                .WithOne(T => T.Project)
                .HasForeignKey(T => T.ProjectId);

            builder.HasOne(P => P.User)
                .WithMany(U => U.Projects)
                .HasForeignKey(P => P.UserId);

        }
    }
}
