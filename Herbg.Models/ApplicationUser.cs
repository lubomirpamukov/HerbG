using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Herbg.Common.ValidationConstants.ApplicationUser;

namespace Herbg.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(UserAddressMaxLength)]
    public string? Address { get; set; } = null!;

    [Required]
    public bool IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

    public virtual ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Cart Cart { get; set; } = new Cart();
}
