using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class DataContext : DbContext
    {
        //Contructor to captire and pass in the connection string that base classes expecting
        public DataContext()
            //Looks for "DefailtConnection" in the Web.config
            //Can actually pass in a connection string
            : base("DefaultConnection")
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        //2nd Add Customer Model to DB
        public DbSet<Customer> Customers { get; set; }
        // 3rd We add migration


    }
}
