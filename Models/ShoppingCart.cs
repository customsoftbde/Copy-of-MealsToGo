using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MealsToGo.Models
{
    public partial class ShoppingCart
    {
        ThreeSixtyTwoEntities storeDB = new ThreeSixtyTwoEntities();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        
        public static ShoppingCart GetCart( int UserID)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = UserID.ToString();
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller, int UserID)
        {
            return GetCart(UserID);
        }
        //public ActionResult AddToCart(int id, int userID)
        //{
        //    // Get the matching cart and mealad instances
        //    var cartItem = storeDB.Carts.SingleOrDefault(
        //        c => c.CartID == ShoppingCartId
        //        && c.MealAdID == id);

        //    if (cartItem == null)
        //    {
        //        // Create a new cart item if no cart item exists
        //        cartItem = new Cart
        //        {
        //            MealAdID = id,
        //            CartID = ShoppingCartId,
        //            Count = 1,
        //            DateCreated = DateTime.Now
        //        };
        //        storeDB.Carts.Add(cartItem);
        //    }
        //    else
        //    {
        //        // If the item does exist in the cart, 
        //        // then add one to the quantity
        //        cartItem.Count++;
        //    }
        //    // Save changes
        //    storeDB.SaveChanges();
        //    return
        //    //return RedirectResult("home/cart");
        //}
        public void AddToCart(MealAd mealad)
        {
            // Get the matching cart and mealad instances
            var cartItem = storeDB.Carts.SingleOrDefault(
                c => c.CartID == ShoppingCartId
                && c.MealAdID == mealad.MealAdID);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    MealAdID = mealad.MealAdID,
                    CartID = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                storeDB.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            storeDB.SaveChanges();
        }
        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = storeDB.Carts.Single(
                cart => cart.CartID == ShoppingCartId
                && cart.RecordID == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    storeDB.Carts.Remove(cartItem);
                }
                // Save changes
                storeDB.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = storeDB.Carts.Where(
                cart => cart.CartID == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                storeDB.Carts.Remove(cartItem);
            }
            // Save changes
            storeDB.SaveChanges();
        }
        public List<Cart> GetCartItems()
        {
            return storeDB.Carts.Where(
                cart => cart.CartID == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in storeDB.Carts
                          where cartItems.CartID == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply mealad price by count of that mealad to get 
            // the current price for each of those mealads in the cart
            // sum all mealad price totals to get the cart total
            decimal? total = (from cartItems in storeDB.Carts
                              where cartItems.CartID == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.MealAd.MealItem.Price).Sum();

            return total ?? decimal.Zero;
        }
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    MealAdID = item.MealAdID,
                    OrderID = order.OrderID,
                  
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.MealAd.MealItem.Price);

                storeDB.OrderDetails.Add(orderDetail);

            }
            // Set the order's total to the orderTotal count
           order.Total = orderTotal;


            // Save the order
            storeDB.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderID;
        }
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = storeDB.Carts.Where(c => c.CartID == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartID = userName;
            }
            storeDB.SaveChanges();
        }
    }
}