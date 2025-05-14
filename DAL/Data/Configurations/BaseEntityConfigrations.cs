using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data.Configurations
{
    public class BaseEntityConfigrations<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(D => D.CreatedAt).HasDefaultValueSql("getdate()");
            builder.Property(D => D.UpdatedAt).HasComputedColumnSql("getdate()");
        }
    }
}
