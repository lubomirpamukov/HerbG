using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.ViewModels.Order;

public class OrderSummaryViewModel
{
    public string OrderId { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public int TotalItems { get; set; }
}
