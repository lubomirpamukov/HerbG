using Herbg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Data.Configurations;

public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
{
    public void Configure(EntityTypeBuilder<CreditCard> builder)
    {
        //Primary key
        builder
            .HasKey(x => x.Id);

        //CreditCard realtion with user
        builder
            .HasOne(c => c.Client)
            .WithMany(c => c.CreditCards)
            .HasForeignKey(c => c.ClientId);

        //CreditCard Relation with order
        builder
            .HasOne(c => c.Order)
            .WithOne(o => o.Card)
            .HasForeignKey<Order>(o => o.Id)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
