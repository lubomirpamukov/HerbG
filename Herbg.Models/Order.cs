using Herbg.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Herbg.Common.ValidationConstants.Order;

namespace Herbg.Models;

public class Order
{
    [Required]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [ForeignKey(nameof(ApplicationUser))]
    public string ClientId { get; set; } = null!;

    [Required]
    public virtual ApplicationUser Client { get; set; } = null!;


    [Required]
    public DateTime Date { get; set; } = DateTime.Now;

    [Required]
    [MaxLength(OrderAddressMaxLength)]
    public string Address { get; set; } = null!;

    [Required]
    public PaymentMethodEnum PaymentMethod { get; set; }

    [Required]
    public decimal TotalAmount { get; set; }
    public string? CardId { get; set; }
    

    public virtual ICollection<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();

}
