using Herbg.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Data.Configurations;

public class CompanyUserConfiguration : IEntityTypeConfiguration<CompanyUser>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CompanyUser> builder)
    {
        //Primary key configuration
        builder
            .HasKey(cu => new { cu.CompanyId, cu.ClientId });

        //CompanyUser relations with Company
        builder
            .HasOne(c => c.Company)
            .WithMany(c => c.CompanyUsers)
            .HasForeignKey(c => c.CompanyId);

        //CompanyUser relations with User
        builder
            .HasOne(c => c.Client)
            .WithMany(c => c.CompanyUsers)
            .HasForeignKey(c => c.ClientId);
    }
}
