using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GCheckout.Util;
using GCheckout.Checkout;

namespace MealsToGo.Views.GCheckout
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GCheckoutButton1_Click(object sender, ImageClickEventArgs e)
        {
            CheckoutShoppingCartRequest Req = GCheckoutButton1.CreateRequest();
            Req.AddItem("Snickers", "Packed with peanuts.", 0.75m, 2);
            Req.AddItem("Gallon of Milk", "Milk goes great with candy bars!", 2.99m, 1);
            Req.AddStateTaxRule("CA", 0.0825, true);
            Req.AddStateTaxRule("IL", 0.0625, false);
            ShippingRestrictions Only48Lower = new ShippingRestrictions();
           // Only48Lower.AddAllowedCountryArea(GCheckout.AutoGen.USAreas.CONTINENTAL_48);
            Req.AddFlatRateShippingMethod("UPS Ground", 7.05m, Only48Lower);
            ShippingRestrictions OnlyCA_NV = new ShippingRestrictions();
            OnlyCA_NV.AddAllowedStateCode("CA");
            OnlyCA_NV.AddAllowedStateCode("NV");
            Req.AddFlatRateShippingMethod("California Express", 6.35m, OnlyCA_NV);
            Req.AddFlatRateShippingMethod("USPS", 3.08m);
            Req.AddPickupShippingMethod("Pick up in store", 0);
            Req.ContinueShoppingUrl = "http://www.example.com/continueshopping";
            Req.EditCartUrl = "http://www.example.com/editcart";
            GCheckoutResponse Resp = Req.Send();
            Response.Redirect(Resp.RedirectUrl, true);
        }
    }
}