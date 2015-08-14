using System;
using System.Linq;
using System.Web.Mvc;
using MealsToGo.Models;
using WebMatrix.WebData;

namespace MealsToGo.Controllers
{
    [Authorize]
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Default)]
    public class CheckoutController : Controller
    {
        ThreeSixtyTwoEntities storeDB = new ThreeSixtyTwoEntities();
        const string PromoCode = "FREE";

        //
        // GET: /Checkout/ShippingAndPayment

        public ActionResult ShippingAndPayment()
        {
            //return View();


            return RedirectToAction("PostToPayPal", "PayPal", routeValues: new { userid = WebSecurity.CurrentUserId });
        }

           

        //
        // POST: /Checkout/ShippingAndPayment

        [HttpPost]
        public ActionResult ShippingAndPayment(FormCollection values,int userid)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    
                    
                    order.UserId = userid;//;User.Identity.Name;
                    order.DateCreated = DateTime.Now;
                  

                    //Save Order
                    storeDB.Orders.Add(order);
                    storeDB.SaveChanges();

                    //Process the order
                    var cart = ShoppingCart.GetCart(userid);
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete",
                        new { id = order.OrderID });
                }

            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }

        //
        // GET: /Checkout/Complete

        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(o => o.OrderID == id &&
                o.UserId == 1);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
