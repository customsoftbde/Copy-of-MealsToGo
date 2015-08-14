using System.Collections.Generic;
using MealsToGo.Models;

namespace MealsToGo.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}