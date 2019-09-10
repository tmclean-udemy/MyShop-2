using MyShop.Core.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Core.Contracts
{
    public interface IBasketService
    {
        // Add interface
        void AddToBasket(HttpContextBase httpContext, string productId);
        // Remove interface
        void RemoveFromBasket(HttpContextBase httpContext, string itemId);
        // Get interface
        List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext);
        // Summary interface
        BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext);
    }
}
