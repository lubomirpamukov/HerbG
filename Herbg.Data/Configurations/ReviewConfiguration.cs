using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Herbg.Models;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herbg.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        //Primary key configuration
        builder
            .HasKey(x => x.Id);

        //Review realtion with product
        builder
            .HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId);

        //Review relation with Client
        builder
            .HasOne(r => r.Client)
            .WithMany(c => c.Reviews)
            .HasForeignKey(r => r.ClientId);
    }
}
