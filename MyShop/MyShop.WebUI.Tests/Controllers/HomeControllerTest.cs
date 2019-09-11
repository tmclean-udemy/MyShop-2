using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.View_Models;
using MyShop.WebUI;
using MyShop.WebUI.Controllers;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IndexPageDoesReturnProducts()
        {
            //Need to create Mock version of the repositories
            // Then, below, create the controller
            IRepository<Product> productContext = new Mocks.MockContext<Product>();
            IRepository<ProductCategory> productCategoryContext = new Mocks.MockContext<ProductCategory>();

            //This will make the test successful because it insert a product
            productContext.Insert(new Product());

            HomeController controller = new HomeController(productContext, productCategoryContext);



            // Then call the index method on the controller
            var result = controller.Index() as ViewResult;
            // Cast the ProductListViewModel
            var viewModel = (ProductListViewModel)result.ViewData.Model;

            //Now Access the areas of the view model
            //This test will look to see if there is at least one product
            Assert.AreEqual(1, viewModel.Products.Count());
        }
    }
}
