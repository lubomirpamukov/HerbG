using Herbg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Data.Configurations;

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        // Primary key configuration
        builder
            .HasKey(x => x.Id);

        //Wishlist relations with User
        builder
            .HasOne(w => w.Client)
            .WithMany(c => c.Wishlists)
            .HasForeignKey(w => w.ClientId);

        //Wishlist relations with product
        builder
            .HasOne(w => w.Product)
            .WithMany(p => p.Wishlists)
            .HasForeignKey(w => w.ProductId);
    }
}
