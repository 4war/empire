using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using SportStore.Infrastructure;
using SportStore.Models;
using SportStore.Models.ViewModels;

namespace SportStore.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository _repository;
        private Cart _cart;

        public CartController(IProductRepository repository, Cart cartService)
        {
            _repository = repository;
            _cart = cartService;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel()
            {
                Cart = _cart,
                ReturnUrl = returnUrl
            });
        }
        
        public RedirectToActionResult AddToCard(int productId, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(x => x.ProductId == productId);

            if (product != null)
            {
                _cart.AddItem(product, 1);
            }

            return RedirectToAction("index", new { returnUrl });
        }
        
        
        public RedirectToActionResult RemoveFromCard(int productId, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(x => x.ProductId == productId);

            if (product != null)
            {
                _cart.RemoveLine(product);
            }

            return RedirectToAction("index", new { returnUrl });
        }
    }
}