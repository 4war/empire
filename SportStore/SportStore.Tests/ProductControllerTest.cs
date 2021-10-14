using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Moq;
using SportStore.Controllers;
using SportStore.Models;
using SportStore.Models.ViewModels;
using Xunit;

namespace SportStore.Tests
{
    public class ProductControllerTest
    {
        [Fact]
        public void Can_Paginate()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products)
                .Returns((new Product[]
                {
                    new Product { ProductId = 1, Name = "P1" },
                    new Product { ProductId = 2, Name = "P2" },
                    new Product { ProductId = 3, Name = "P3" },
                    new Product { ProductId = 4, Name = "P4" },
                    new Product { ProductId = 5, Name = "P5" },
                }).AsQueryable());

            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            var result = (controller.List(null, 2).Model as ProductListViewModel).Products.ToList();

            Assert.True(result.Count == 2);

            Assert.Equal("P4", result[0].Name);
            Assert.Equal("P5", result[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products)
                .Returns(new List<Product>()
                {
                    new Product { ProductId = 1, Name = "P1" },
                    new Product { ProductId = 2, Name = "P2" },
                    new Product { ProductId = 3, Name = "P3" },
                    new Product { ProductId = 4, Name = "P4" },
                    new Product { ProductId = 5, Name = "P5" },
                }.AsQueryable());

            var controller = new ProductController(mock.Object) { PageSize = 3 };
            var result = controller.List(null, 2).ViewData.Model as ProductListViewModel;

            var pagingInfo = result.PagingInfo;

            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products)
                .Returns(new List<Product>()
                {
                    new Product { ProductId = 1, Name = "P1", Category = "Tennis" },
                    new Product { ProductId = 2, Name = "P2", Category = "Music" },
                    new Product { ProductId = 3, Name = "P3", Category = "Tennis" },
                    new Product { ProductId = 4, Name = "P4", Category = "Tennis" },
                    new Product { ProductId = 5, Name = "P5", Category = "Music" },
                }.AsQueryable());

            var controller = new ProductController(mock.Object) { PageSize = 3 };
            var result = controller.List("Music", 1).ViewData.Model as ProductListViewModel;

            var pagingInfo = result.PagingInfo;
            var list = result.Products.ToList();

            Assert.Equal("P2", list[0].Name);
            Assert.Equal("P5", list[1].Name);
            Assert.Equal(1, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(2, pagingInfo.TotalItems);
            Assert.Equal(1, pagingInfo.TotalPages);
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(x => x.Products)
                .Returns(new List<Product>()
                {
                    new Product { ProductId = 1, Name = "P1", Category = "Tennis" },
                    new Product { ProductId = 2, Name = "P2", Category = "Music" },
                    new Product { ProductId = 3, Name = "P3", Category = "Tennis" },
                    new Product { ProductId = 4, Name = "P4", Category = "Tennis" },
                    new Product { ProductId = 5, Name = "P5", Category = "Music" },
                }.AsQueryable());

            var target = new ProductController(mock.Object);
            target.PageSize = 3;
            
            Func<ViewResult, ProductListViewModel> GetModel = result => 
                result?.ViewData?.Model as ProductListViewModel;

            var res1 = GetModel(target.List("Tennis"))?.PagingInfo.TotalItems;
            var res2 = GetModel(target.List("Music"))?.PagingInfo.TotalItems;
            var resAll = GetModel(target.List(null))?.PagingInfo.TotalItems;
            
            Assert.Equal(3, res1.Value);
            Assert.Equal(2, res2.Value);
            Assert.Equal(5, resAll.Value);
        }
    }
}