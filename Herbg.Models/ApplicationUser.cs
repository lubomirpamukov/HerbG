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
    [Required]
    [MaxLength(UserAddressMaxLength)]
    public string Address { get; set; } = null!;

    [Required]
    public bool IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
