using Herbg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Data.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        //Primary key
        builder
            .HasKey(k => new { k.ProductId, k.CartId });

        //Relation with Cart
        builder.HasOne(c => c.Cart)
            .WithMany(ci => ci.CartItems)
            .HasForeignKey(c => c.CartId);

        //Relation with Product
        builder.HasOne(p => p.Product)
            .WithMany(pi => pi.CartItems)
            .HasForeignKey(p => p.ProductId);
    }
}
