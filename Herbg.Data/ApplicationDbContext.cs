using Herbg.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection.Emit;

namespace Herbg.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            builder.Entity<Category>().HasData
                (
                   new Category 
                   {
                       Id = 1, Name = "Medicinal Herbs",
                       Description = "Explore our range of medicinal herbs.",
                       ImagePath = "/images/categories/medical-herb-category.jpg"
                   },
                   new Category 
                   { Id = 2,
                       Name = "Culinary Herbs",
                       Description = "Discover fresh and aromatic herbs.",
                       ImagePath = "/images/categories/culinary-category.jpg"
                   },
                   new Category
                   {
                       Id = 3,
                       Name = "Herbal Teas",
                       Description = "Savor the soothing flavors of our premium herbal teas. Perfect for relaxation and wellness.",
                       ImagePath = "/images/categories/herbal-teas.jpg"
                   },
                   new Category
                   {
                       Id = 4,
                       Name = "Aromatherapy Herbs",
                       Description = "Discover our selection of herbs for aromatherapy. Perfect for relaxation, focus, and mood enhancement.",
                       ImagePath = "/images/categories/aromatherapy-herbs.jpg"
                   }
                );

            builder.Entity<Manufactorer>().HasData
               (
                    new Manufactorer
                    {
                        Id = 1,
                        Name = "Bilkibg",
                        Address = "gk.strelbishte 124, Sofia"
                    },
                   new Manufactorer
                   {
                       Id = 2,
                       Name = "Herbas",
                       Address = "gk.lulin 24, Sofia"
                   },
                   new Manufactorer
                   {
                       Id = 3,
                       Name = "7Season",
                       Address = "gk.vrajdebna 12, Sofia"
                   }
               );

            builder.Entity<Product>().HasData
                (
                    new Product
                    {
                        Id = "6bc2d52c-dee4-474d-9918-ef1375e38a00",
                        Name = "Herbal Tea Mix",
                        Price = 14.99m,
                        ImagePath = "images/products/herbal-tea-mix.jpg",
                        Description = "Herbal Tea Mix: A soothing blend of natural herbs crafted to promote relaxation, improve digestion, and boost overall wellness. Perfect for a calming break anytime.",
                        CategoryId = 3,
                        ManufactorerId = 1

                    },
                    new Product
                    {
                        Id = "a59b9080-679b-474a-9d56-f5049850ea94",
                        Name = "Lavender Oil",
                        Price = 10.99m,
                        ImagePath = "images/products/lavender-oil.jpg",
                        Description = "Lavender Oil: A pure, aromatic essential oil known for its calming properties, skin nourishment, and stress relief. Ideal for relaxation and self-care rituals.",
                        CategoryId = 4,
                        ManufactorerId = 2
                    },
                    new Product
                    {
                        Id = "38661195-ce52-48af-905f-d9ab42834679",
                        Name = "Rosemary Pack",
                        Price = 7.99m,
                        ImagePath = "images/products/rosmery-pack.jpg",
                        Description = "Dried Rosemary: A fragrant herb with a robust flavor, perfect for enhancing your culinary dishes or brewing into a soothing herbal tea.",
                        CategoryId = 2,
                        ManufactorerId = 3
                    },
                    new Product
                    {
                        Id = "7549d117-2412-494e-965e-f9cea2c88fea",
                        Name = "Basil plant",
                        Price = 17.99m,
                        ImagePath = "images/products/basil-plant.jpg",
                        Description = "Basil Plant: A fresh and aromatic herb, ideal for home gardens, cooking, and adding a touch of greenery to your space.",
                        CategoryId = 2,
                        ManufactorerId = 1
                    }
                );

        }

        public DbSet<ApplicationUser> Clients { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<CompanyUser> CompanyUsers { get; set; } = null!;
        public DbSet<CreditCard> CreditCards { get; set; } = null!;
        public DbSet<Manufactorer> Manufactorers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductOrder> ProductOrders { get; set; } = null!;
        public DbSet<ProductSize> ProductSizes { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Size> Sizes { get; set; } = null!;
        public DbSet<Wishlist> Wishlists { get; set; } = null!;
    }
}
