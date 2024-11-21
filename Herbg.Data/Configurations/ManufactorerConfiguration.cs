using Herbg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Data.Configurations;

public class ManufactorerConfiguration : IEntityTypeConfiguration<Manufactorer>
{
    public void Configure(EntityTypeBuilder<Manufactorer> builder)
    {
        //Primary key configuration
        builder
            .HasKey(x => x.Id);

        //Manufactorer relation with product
        builder
            .HasMany(m => m.Products)
            .WithOne(p => p.Manufactorer)
            .HasForeignKey(p => p.ManufactorerId);
    }
}
