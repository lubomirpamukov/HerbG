using Herbg.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.ViewModels.Order;

public class OrderDetailsViewModel
{
    public string OrderId { get; set; } = string.Empty;

    [Display(Name = "Order Date")]
    public DateTime Date { get; set; }

    [Display(Name = "Total Amount")]
    public decimal TotalAmount { get; set; }

    // Customer Information
    [Display(Name = "Customer Name")]
    public string CustomerName { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string CustomerEmail { get; set; } = string.Empty;

    [Display(Name = "Shipping Address")]
    public string Address { get; set; } = string.Empty;

    // Payment Information
    [Display(Name = "Payment Method")]
    public PaymentMethodEnum PaymentMethod { get; set; }

    // Ordered Products
    public List<OrderedProductViewModel> OrderedProducts { get; set; } = new();
}
