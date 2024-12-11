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
                       Id = 1,
                       Name = "Medicinal Herbs",
                       Description = "Explore our range of medicinal herbs.",
                       ImagePath = "https://media.istockphoto.com/id/598931180/photo/basil-sage-dill-and-thyme-herbs.jpg?s=1024x1024&w=is&k=20&c=QgnLxS6TDDPoh_bbVAVqaXTe5TbyjIFge9sxSGSA__s="
                   },
                   new Category
                   {
                       Id = 2,
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
                        Id = 1,
                        Name = "Herbal Tea Mix",
                        Price = 14.99m,
                        ImagePath = "https://media.istockphoto.com/id/955162416/photo/various-leaves-of-tea-and-spices-on-wooden-background.jpg?s=1024x1024&w=is&k=20&c=nGcLCxUl-mWMwpQWnngE9fC4AgzcQYho8FcjMsu0Ta8=",
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
                        ImagePath = "https://media.istockphoto.com/id/610584404/photo/eco-craft-christmas-gift-boxes.jpg?s=1024x1024&w=is&k=20&c=HmXz1r7xDQBg83Mvcqn9roHR-3G5LzqHz5OfE_QqDB0=",
                        Description = "Dried Rosemary: A fragrant herb with a robust flavor, perfect for enhancing your culinary dishes or brewing into a soothing herbal tea.",
                        CategoryId = 2,
                        ManufactorerId = 3
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "Basil Plant",
                        Price = 17.99m,
                        ImagePath = "https://media.istockphoto.com/id/535913985/photo/basil-in-a-clay-pot.jpg?s=1024x1024&w=is&k=20&c=GnxsfcEjKTGdOUEfHvq1E2Pr8ZBcYiu-n0GABgGSyNA=",
                        Description = "Basil Plant: A fresh and aromatic herb, ideal for home gardens, cooking, and adding a touch of greenery to your space.",
                        CategoryId = 2,
                        ManufactorerId = 1
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "Eucalyptus Essential Oil",
                        Price = 15.99m,
                        ImagePath = "https://media.istockphoto.com/id/1053121442/photo/eucalyptus-essential-oil.jpg?s=1024x1024&w=is&k=20&c=94_16e0iLljAy_9fFWKUQa_6DhCM5MYCv1FmtrSnKIU=",
                        Description = "Eucalyptus Essential Oil: Known for its refreshing scent and healing properties, it aids in relaxation, focus, and relieving respiratory issues.",
                        CategoryId = 4,
                        ManufactorerId = 2
                    },
                    new Product
                    {
                        Id = 6,
                        Name = "Peppermint Tea",
                        Price = 11.49m,
                        ImagePath = "https://media.istockphoto.com/id/1355250220/photo/dry-herbal-green-tea-in-a-plate-with-cup-of-tea-and-eucalyptus-leaves-top-view-bright.jpg?s=1024x1024&w=is&k=20&c=06yP4sIe09ceFdG9nzGqCtEa54QqJAt1QogPZwED3X4=",
                        Description = "Peppermint Tea: A refreshing herbal tea known for its soothing properties that help with digestion and reduce stress.",
                        CategoryId = 3,
                        ManufactorerId = 2
                    },
                    new Product
                    {
                        Id = 7,
                        Name = "Chamomile Tea",
                        Price = 12.50m,
                        ImagePath = "https://media.istockphoto.com/id/1134246421/photo/cup-of-chamomile-tea-with-tea-bag.jpg?s=1024x1024&w=is&k=20&c=sHiQcQ0RND7zkmcXsbcAgnUIXBRG1ax54f0zzwJQ8ZM=",
                        Description = "Chamomile Tea: A calming herbal tea made from chamomile flowers, perfect for winding down before bed and promoting better sleep.",
                        CategoryId = 3,
                        ManufactorerId = 3
                    },
                    new Product
                    {
                        Id = 8,
                        Name = "Aloe Vera Gel",
                        Price = 19.99m,
                        ImagePath = "https://media.istockphoto.com/id/1215011574/photo/aloe-vera-gel-close-up-sliced-aloe-vera-plants-leaf-and-gel-with-wooden-spoon.jpg?s=1024x1024&w=is&k=20&c=fm8eVWQIF53Q0-zRZTMgXB3Ve3D2_vA1JpbYwlU5-hk=",
                        Description = "Aloe Vera Gel: Known for its soothing and healing properties, ideal for skincare, sunburns, and hydration.",
                        CategoryId = 4,
                        ManufactorerId = 1
                    },
                    new Product
                    {
                        Id = 9,
                        Name = "Turmeric Powder",
                        Price = 8.99m,
                        ImagePath = "https://media.istockphoto.com/id/965503302/photo/turmeric-powder-and-roots.jpg?s=1024x1024&w=is&k=20&c=U3hFkU4b8ODTK_9o4kMYEQbfW_JGC5-FbFd4uZQaHSE=",
                        Description = "Turmeric Powder: A powerful antioxidant and anti-inflammatory spice, perfect for adding flavor to dishes or making turmeric tea.",
                        CategoryId = 2,
                        ManufactorerId = 3
                    },
                    new Product
                    {
                        Id = 10,
                        Name = "Rosemary Essential Oil",
                        Price = 13.99m,
                        ImagePath = "https://media.istockphoto.com/id/589137972/photo/small-bottle-of-burdock-extract.jpg?s=1024x1024&w=is&k=20&c=vH22VbCqRIPBstTDj6n5vuKjIICEiIxRf7fgFZZ9lmw=",
                        Description = "Rosemary Essential Oil: A versatile oil for aromatherapy, known for its energizing, memory-boosting, and stress-relieving benefits.",
                        CategoryId = 4,
                        ManufactorerId = 3
                    },
                    new Product
                    {
                        Id = 11,
                        Name = "Sage Bundle",
                        Price = 9.99m,
                        ImagePath = "https://media.istockphoto.com/id/1460937551/photo/smoldering-white-sage-smudge-stick.jpg?s=1024x1024&w=is&k=20&c=agi9O7Z28qVwfViQBdyJA20okfFw62_30RszMAv-tHA=",
                        Description = "Sage Bundle: Dried sage leaves for smudging and cleansing, offering spiritual and emotional benefits for relaxation and focus.",
                        CategoryId = 1,
                        ManufactorerId = 2
                    },
                    new Product
                    {
                        Id = 12,
                        Name = "Lemongrass Tea",
                        Price = 11.25m,
                        ImagePath = "https://media.istockphoto.com/id/546792864/photo/thai-herbal-drinks-lemon-grass.jpg?s=1024x1024&w=is&k=20&c=Mz-14vRpNlWawyOF3BtwS-gDrpgvF-Y1id9-wcigFRc=",
                        Description = "Lemongrass Tea: A refreshing, citrusy tea made from lemongrass, known for its ability to promote digestion and reduce anxiety.",
                        CategoryId = 3,
                        ManufactorerId = 1
                    },
                    new Product
                    {
                        Id = 13,
                        Name = "Cinnamon Stick Pack",
                        Price = 6.50m,
                        ImagePath = "https://media.istockphoto.com/id/1214648653/photo/cinnamon-sticks-with-rope-isolated-on-white-background.jpg?s=1024x1024&w=is&k=20&c=et2CT61Hat3vgIhQ1ScDtiRszza7khjNXkACwLmCPcg=",
                        Description = "Cinnamon Stick Pack: A bundle of dried cinnamon sticks, perfect for brewing in tea, adding to cooking, or for aromatic purposes.",
                        CategoryId = 2,
                        ManufactorerId = 1
                    },
                    new Product
                    {
                        Id = 14,
                        Name = "Aromatherapy Diffuser",
                        Price = 20.99m,
                        ImagePath = "https://media.istockphoto.com/id/1366419259/photo/humidifier-with-steam-moisturizing-air-at-home.jpg?s=1024x1024&w=is&k=20&c=0N59shgTzPOYmnLtHcdw8MbSMh2ta792-mi6TOPKYv8=",
                        Description = "Aromatherapy Diffuser: Enhance your space with the calming effects of essential oils. This ultrasonic diffuser disperses oils for a soothing atmosphere, perfect for relaxation, focus, and stress relief.",
                        CategoryId = 4,
                        ManufactorerId = 2
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
