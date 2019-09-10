using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService
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
    }
}

//Now we have to create a view model to get information about the product, ie, price, description, color...
