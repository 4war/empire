using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportStore.Components;
using SportStore.Controllers;
using SportStore.Models;
using SportStore.Models.ViewModels;
using Xunit;

namespace SportStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products)
                .Returns(new List<Product>()
                {
                    new Product {ProductId = 1, Name = "P1", Category = "Apples"},
                    new Product {ProductId = 2, Name = "P2", Category = "Apples"},
                    new Product {ProductId = 3, Name = "P3", Category = "Plums"},
                    new Product {ProductId = 4, Name = "P4", Category = "Oranges"},
                }.AsQueryable());

            var controller = new ProductController(mock.Object);
            var result = controller.List("Music",1).ViewData.Model as ProductListViewModel;
            var list = result.Products.ToList();
            
            Assert.True(Enumerable.SequenceEqual(new string [] { "Apples",
                "Oranges", "Plums" }, list.Select(x => x.Category)));
        }
        
        [Fact]
        public void Indicates_Selected_Category()
        {
            var categoryToSelect = "Apples";
            var mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products)
                .Returns(new List<Product>()
                {
                    new Product {ProductId = 1, Name = "P1", Category = "Apples"},
                    new Product {ProductId = 4, Name = "P2", Category = "Oranges"},
                }.AsQueryable());

            var target = new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext()
                {
                    RouteData = new RouteData()
                }
            };

            target.RouteData.Values["category"] = categoryToSelect;

            var result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];
            
            Assert.Equal(categoryToSelect, result);
        }
    }
}