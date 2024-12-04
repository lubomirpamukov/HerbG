using Herbg.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Services.Interfaces;

public interface IOrderService
{
    public Task<CheckoutViewModel> GetCheckout(string clientId,string cartId);

    public Task<string> GetOrderConfirmed(string clientId, CheckoutViewModel model);

    public Task<OrderDetailsViewModel> GetOrderDetailsAsync(string orderId);
}
