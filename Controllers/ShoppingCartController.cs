using System.Linq;
using System.Web.Mvc;
using MealsToGo.Models;
using MealsToGo.ViewModels;


namespace MealsToGo.Controllers
{
    public class ShoppingCartController : Controller
    {
        ThreeSixtyTwoEntities storeDB = new ThreeSixtyTwoEntities();
        //
        // GET: /ShoppingCart/
        public ActionResult Index(int userid)
        {
            var cart = ShoppingCart.GetCart(userid);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id, int userid)
        {
            // Retrieve the mealad from the database
            var addedmealad = storeDB.MealAds.FirstOrDefault(mealad => mealad.MealItemID == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(userid);

            cart.AddToCart(addedmealad);

            // Go back to the main store page for more shopping
            //return RedirectToAction("Index","Home");
            return RedirectToAction("cart", "home", routeValues: new { userid = userid });
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id,int userid)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(userid);

            // Get the name of the mealad to display confirmation
           
                
                int MealItemID= storeDB.Carts.Single(item => item.RecordID == id).MealAd.MealItemID;
                string mealadName = storeDB.MealItems.Single(item => item.MealItemId == id).MealItemName;
            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(mealadName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary(int userid)
        {
            var cart = ShoppingCart.GetCart(userid);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}