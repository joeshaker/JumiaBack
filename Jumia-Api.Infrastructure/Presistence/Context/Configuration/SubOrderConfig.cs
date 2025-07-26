using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models.Configuration
{
    public class SubOrderConfig : IEntityTypeConfiguration<SubOrder>
    {
        public void Configure(EntityTypeBuilder<SubOrder> builder)
        {
            builder.HasMany(builder => builder.OrderItems)
                .WithOne(builder => builder.SubOrder)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
