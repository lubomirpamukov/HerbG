using Herbg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Data.Configurations;

public class ProductOrderConfiguration : IEntityTypeConfiguration<ProductOrder>
{
    public void Configure(EntityTypeBuilder<ProductOrder> builder)
    {
        //Primary key configuration
        builder
            .HasKey(po => new { po.OrderId, po.ProductId });

        //ProductOrder relation with Order
        builder
            .HasOne(po => po.Order)
            .WithMany(o => o.ProductOrders)
            .HasForeignKey(o => o.OrderId);

        //ProductOrder relation with Product
        builder
            .HasOne(po => po.Product)
            .WithMany(p => p.ProductOrders)
            .HasForeignKey(o => o.ProductId);
    }
}
