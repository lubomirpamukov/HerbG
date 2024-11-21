using Herbg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Data.Configurations;

public class ProductSizeConfiguration : IEntityTypeConfiguration<ProductSize>
{
    public void Configure(EntityTypeBuilder<ProductSize> builder)
    {
        //Primary key configuration
        builder
            .HasKey(x => new { x.ProductId, x.SizeId });

        //ProductSize relation with Size
        builder
            .HasOne(ps => ps.Size)
            .WithMany(p => p.ProductSizes)
            .HasForeignKey(p => p.SizeId);

        //ProducSize realtin with Product
        builder
            .HasOne(ps => ps.Product)
            .WithMany(p => p.ProductSizes)
            .HasForeignKey(ps => ps.ProductId);
    }
}
