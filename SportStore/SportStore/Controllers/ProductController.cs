using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportStore.Models;
using SportStore.Models.ViewModels;

namespace SportStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _repository;
        public int PageSize = 4;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }
        
        public ViewResult List(string category, int productPage = 1)
        {
            return View(new ProductListViewModel
            {
                Products = _repository.Products
                    .Where(x => category == null || x.Category == category)
                    .OrderBy(x => x.ProductId)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null 
                        ? _repository.Products.Count() 
                        : _repository.Products.Count(x => x.Category == category)
                },
                CurrentCategory = category
            });
        }
    }
}