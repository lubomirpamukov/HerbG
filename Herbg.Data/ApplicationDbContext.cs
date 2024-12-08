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
                       ImagePath = "https://media.istockphoto.com/id/598931180/photo/basil-sage-dill-and-thyme-herbs.jpg?s=1024x1024&w=is&k=20&c=QgnLxS6TDDPoh_bbVAVqaXTe5TbyjIFge9sxSGSA__s="
                   },
                   new Category 
                   { Id = 2,
                       Name = "Culinary Herbs",
                       Description = "Discover fresh and aromatic herbs.",
                       ImagePath = "https://media.istockphoto.com/id/504069254/photo/fresh-herbs-on-wooden-background.jpg?s=1024x1024&w=is&k=20&c=TdPOhT3xUMRt03AzvSfo8NKgzKusHbXMJv9Omw7zenw="
                   },
                   new Category
                   {
                       Id = 3,
                       Name = "Herbal Teas",
                       Description = "Savor the soothing flavors of our premium herbal teas. Perfect for relaxation and wellness.",
                       ImagePath = "https://media.istockphoto.com/id/545799832/photo/two-cups-of-healthy-herbal-tea-with-mint-cinnamon-dried.jpg?s=1024x1024&w=is&k=20&c=kRpRimF1ufgUaXyl-wcFkQKvnU4YbMFwtpqKlFhG9oM="
                   },
                   new Category
                   {
                       Id = 4,
                       Name = "Aromatherapy Herbs",
                       Description = "Discover our selection of herbs for aromatherapy. Perfect for relaxation, focus, and mood enhancement.",
                       ImagePath = "https://media.istockphoto.com/id/546775666/photo/dried-herbs-and-essential-oils.jpg?s=1024x1024&w=is&k=20&c=4AK88NpTGMeqCViwoizSxc0B4Wr4nIsxia9hkboaA3M="
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
                        Id= 1,
                        Name = "Herbal Tea Mix",
                        Price = 14.99m,
                        ImagePath = "https://media.istockphoto.com/id/622039222/photo/assortment-of-dry-tea-in-glass-bowls-on-wooden-surface.jpg?s=1024x1024&w=is&k=20&c=4ggjpaDqyMDatq_O6q59BsCFs7VFmk9YDysYr7KDcRY=",
                        Description = "Herbal Tea Mix: A soothing blend of natural herbs crafted to promote relaxation, improve digestion, and boost overall wellness. Perfect for a calming break anytime.",
                        CategoryId = 3,
                        ManufactorerId = 1

                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Lavender Oil",
                        Price = 10.99m,
                        ImagePath = "https://media.istockphoto.com/id/585048326/photo/herbal-oil-and-lavender-flowers.jpg?s=1024x1024&w=is&k=20&c=jNZn3EevtNDr53UcPFilreSd1LOPyK0h5q784h9J8Ns=",
                        Description = "Lavender Oil: A pure, aromatic essential oil known for its calming properties, skin nourishment, and stress relief. Ideal for relaxation and self-care rituals.",
                        CategoryId = 4,
                        ManufactorerId = 2
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Rosemary Pack",
                        Price = 7.99m,
                        ImagePath = "https://media.istockphoto.com/id/1309541100/photo/fresh-rosemary-herb-on-a-wooden-background-top-view-rosemary-with-copy-space-cooking-concept.jpg?s=1024x1024&w=is&k=20&c=rw7K_nYTr-64KBmWlFZ6UrWuGrvV2-88kccf-SG8JwE=m",
                        Description = "Dried Rosemary: A fragrant herb with a robust flavor, perfect for enhancing your culinary dishes or brewing into a soothing herbal tea.",
                        CategoryId = 2,
                        ManufactorerId = 3
                    },
                    new Product
                    {
                        Id=4,
                        Name = "Basil plant",
                        Price = 17.99m,
                        ImagePath = "https://media.istockphoto.com/id/535913985/photo/basil-in-a-clay-pot.jpg?s=1024x1024&w=is&k=20&c=GnxsfcEjKTGdOUEfHvq1E2Pr8ZBcYiu-n0GABgGSyNA=",
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
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;
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
