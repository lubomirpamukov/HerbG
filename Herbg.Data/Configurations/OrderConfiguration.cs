using Herbg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Primary Key
        builder
            .HasKey(o => o.Id);

        //Order relation with user
        builder
            .HasOne(o => o.Client)
            .WithMany(o => o.Orders)
            .HasForeignKey(o => o.ClientId);

    }
}
