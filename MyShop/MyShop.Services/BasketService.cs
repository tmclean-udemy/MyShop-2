using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService : IBasketService
    {
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext)
        {
            this.basketContext = BasketContext;
            this.productContext = ProductContext;
        }
        // Now we load the basket
        // Reading Customer's cookies in the Http context
        // Look for the basket Id
        // Attempt to read the basket Id in the database

        //Create a private methos
        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            // Try to read the cookie
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            // Create a new basket

            Basket basket = new Basket();


            // Checking to see if the cookie exists
            // If yes
            if (cookie != null)
            {
                string basketId = cookie.Value;

                // And if basket isn't empty...
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId)
                }
                // If basket is empty, create the basket
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            // If cookie is null
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }



        // Creat the CreateNewBasket method
        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();

            //Inset it into the db
            basketContext.Insert(basket);
            
            basketContext.Commit();


            //Then we need to write a cookie
            HttpCookie cookie = new HttpCookie(BasketSessionName);
            // Add a value to the cookie by assigning it to an id
            cookie.Value = basket.Id;

            // Set expiration
            cookie.Expires = DateTime.Now.AddDays(1);

            //Add Http cookie response back to the user
            httpContext.Response.Cookies.Add(cookie);

            // Return basket content to custy
            return basket;
        }



        // Add basket item
        //Using a lot of internal methods we just created
        public void AddToBasket(HttpContextBase httpContext, string productId)
        {
            // To get a basket, we need to know if it's created (true)
            Basket basket = GetBasket(httpContext, true);

            // Is there already an item. EntityFramework will load basket from db
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            //Does that item exist in the basket
            if (item == null)
            {
                //If not, create a new basket
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                // Add new item to basket
                basket.BasketItems.Add(item);
            }
            // If there is stuff in the basket...add new item
            else
            {
                // Loading item/items from db (EntityFramework)
                item.Quantity = item.Quantity + 1;
            }
            // Commit Changes
            basketContext.Commit();
        }



        // Add Remove basket item
        //Send basket item id vs product id
        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
        }

        //Now we have to create a view model to get information about the product, ie, price, description, color...
        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            //Get basket from db
            // If it doesn't exist, don't create one
            Basket basket = GetBasket(httpContext, false);

            if (basket != null)
            {

                //Linq query
                var results = (from b in basket.BasketItems
                               join p in productContext.Collection() on b.ProductId equals p.Id
                               select new BasketItemViewModel()
                               {
                                   // Basket table
                                   Id = b.Id,
                                   Quantity = b.Quantity,
                                   // Products table
                                   ProductName = p.Name,
                                   Image = p.Image,
                                   Price = p.Price
                               }
                              //Convert the result into a list
                              ).ToList();
                //Then return the results
                return results;
            }
            // If no basket, return new empty list of basket items
            else
            {
                return new List<BasketItemViewModel>();
            }
        }



        // Basket Summary or Total list and Total Quantity
        //Need to create anothe view model
        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            //Dont create basket if it's currently empty
            Basket basket = GetBasket(httpContext, false);

            //Use the view model and default with 0,0
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);
            if (basket != null)
            {
                //If we do have a basket, we have to find out how many items are in the basket
                // int? stores a null value and return a null value
                int? basketCount = (from item in basket.BasketItems
                                    select item.Quantity).Sum();

                decimal? basketTotal = (from item in basket.BasketItems
                                        join p in productContext.Collection() on item.ProductId equals p.Id
                                        select item.Quantity * p.Price).Sum();
                //If there is a basket count, return that value, otherwise null, return zero
                model.BasketCount = basketCount ?? 0;
                model.BasketTotal = basketTotal ?? decimal.Zero;

                return model;
            }
            else
            {
                return model;
            }

            //Now we need to create an interface for the basket service
        }

    }
}

