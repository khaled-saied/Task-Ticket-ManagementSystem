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
    class TicketConfigrations : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasOne(T => T.Task)
                .WithMany(Ts => Ts.Tickets)
                .HasForeignKey(T => T.TaskId);

            builder.Property(T => T.Type).HasConversion(
                (type) => type.ToString(),
                (type) => (TicketTypeEnum)Enum.Parse(typeof(TicketTypeEnum), type));

            builder.Property(T => T.Status).HasConversion(
                (type) => type.ToString(),
                (type) => (TicketStatusEnum)Enum.Parse(typeof(TicketStatusEnum), type));
        }
    }
}
