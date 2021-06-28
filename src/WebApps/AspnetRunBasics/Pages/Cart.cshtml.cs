using System;
using System.Linq;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class CartModel : PageModel
    {
        private readonly IBasketService _basketService;

        public BasketModel Cart { get; set; } = new BasketModel();

        public CartModel(IBasketService basketService)
        {
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userName = "admin";

            Cart = await _basketService.GetBasket(userName);

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string productId)
        {
            var userName = "admin";

            var basket = await _basketService.GetBasket(userName);
            var item = basket.Items.Single(x => x.ProductId == productId);

            basket.Items.Remove(item);

            await _basketService.UpdateBasket(basket);

            return RedirectToPage();
        }
    }
}