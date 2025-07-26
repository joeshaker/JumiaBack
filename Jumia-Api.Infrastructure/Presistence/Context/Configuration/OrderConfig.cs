using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models.Configuration
{
    public class OrderConfig: IEntityTypeConfiguration<Order>
    {
        

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasMany(o => o.SubOrders)
                .WithOne(s => s.Order)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    
    
}
