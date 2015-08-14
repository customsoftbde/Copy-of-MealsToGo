using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using PayPal.Api;
using MealsToGo.Models;
using WebMatrix.WebData;
namespace MealsToGo.Controllers
{
    public class PaypalController : Controller
    {
        //
        // GET: /Payment/
        private ThreeSixtyTwoEntities dbmeals = new ThreeSixtyTwoEntities();

        //public static string GetResponse(RequestContext context, decimal price)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api-3t.sandbox.paypal.com/nvp");
        //    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api-3t.sandbox.paypal.com/nvp");

        //    request.Method = "POST";

        //    UrlHelper url = new UrlHelper(context);
        //    string urlBase = string.Format("{0}://{1}", context.HttpContext.Request.Url.Scheme, context.HttpContext.Request.Url.Authority);

        //    string formContent = "USER=" + System.Configuration.ConfigurationManager.AppSettings["paypalUser"] +
        //            "&PWD=" + System.Configuration.ConfigurationManager.AppSettings["paypalPassword"] +
        //            "&SIGNATURE=" + System.Configuration.ConfigurationManager.AppSettings["paypalSignature"] +
        //            "&VERSION=84.0" +
        //            "&PAYMENTREQUEST_0_PAYMENTACTION=Sale" +
        //            "&PAYMENTREQUEST_0_AMT=" + String.Format("{0:0.00}", price) +
        //            "&RETURNURL=" + urlBase + url.Action("Confirm", "Checkout") +
        //            "&CANCELURL=" + urlBase + url.Action("Canceled", "Checkout") +
        //            "&METHOD=SetExpressCheckout";

        //    byte[] byteArray = Encoding.UTF8.GetBytes(formContent);
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = byteArray.Length;
        //    Stream dataStream = request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();
        //    WebResponse response = request.GetResponse();
        //    dataStream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(dataStream);
        //    string responseFromServer = HttpUtility.UrlDecode(reader.ReadToEnd());

        //    reader.Close();
        //    dataStream.Close();
        //    response.Close();

        //    return responseFromServer;
        //}

        public ActionResult PostToPayPal(int userid)
        {
            string item = "test";
            string amount = "00.55";
            string BusinessAccountKey = "123456789";
            string CurrencyCode = "USD";

            MealsToGo.Models.Paypal paypal = new Models.Paypal();
            paypal.cmd = "_xclick";
            paypal.business = "kanjasaha@yahoo.com";

            string useSandbox = ConfigurationManager.AppSettings["PayPalUseSandbox"];
            if (useSandbox == "1")
                ViewBag.actionURl = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            else
                ViewBag.actionURl = "https://www.paypal.com/cgi-bin/webscr";

            paypal.cancel_return = System.Configuration.ConfigurationManager.AppSettings["CancelURL"];
            paypal.@return = ConfigurationManager.AppSettings["ReturnURL"]; //+"&PaymentId=1"; you can append your order Id here
            paypal.notify_url = ConfigurationManager.AppSettings["ReturnURL"];// +"?PaymentId=1"; to maintain database logic 

            paypal.currency_code = CurrencyCode;

            //paypal.item_name = item;
            //paypal.amount = amount;
            return View(paypal);
        }

        public ActionResult RedirectFromPaypal()
        {
            return View();
        }
        //?orderId=80guid=22288&token=EC-7FY2945157687815B
        public ActionResult FailureView(int orderId = 0, int guid = 0, string token = "")
        {
            try
            {
                Models.FunOrder order = dbmeals.FunOrders.FirstOrDefault(x => x.OrderID == orderId);
                if (order != null)
                {
                    order.Status = 0;
                }
                foreach (TempOrderList tempData in dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId))
                {
                    dbmeals.TempOrderLists.Remove(tempData);
                }

                dbmeals.SaveChanges();

            }
            catch
            {
            }
            try
            {
                EmailModel emailmodel = new EmailModel();
                emailmodel.Subject = "Your Order Cancelled Successfully";
                emailmodel.To = "kanjasaha@gmail.com"; //"kanjasaha@gmail.com";
                emailmodel.EmailBody = "<div>Your order has been cancelled successfully<div>";
                Common.sendeMail(emailmodel, true);
            }
            catch
            {
            }
            return View();
        }

        //Home/success?orderId=74&guid=19321&paymentId=PAY-7PC28293G8946184CKU73ARQ&token=EC-7WF99309NE796111C&PayerID=QYTJF7KMFTSZW
        public ActionResult Success(int orderId = 0, int guid = 0, string paymentId = "", string token = "", string PayerID = "")
        {
            if (String.IsNullOrEmpty(paymentId) || String.IsNullOrEmpty(PayerID))
            {
                return RedirectToAction("FailureView", new { orderId = orderId, guid = guid, token = token });
            }
            UpdateOrderDeteailsAfterPayment(orderId);
            return View();
        }
        public ActionResult SuccessCash(int orderId = 0)
        {
            UpdateOrderDeteailsAfterPayment(orderId);
            return View("Success");
        }
        private void UpdateOrderDeteailsAfterPayment(int orderId)
        {
            try
            {
                Models.FunOrder order = dbmeals.FunOrders.FirstOrDefault(x => x.OrderID == orderId);
                if (order != null)
                {
                    order.Status = 1;
                }
                foreach (TempOrderList tempData in dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId))
                {
                    dbmeals.TempOrderLists.Remove(tempData);
                }

                dbmeals.SaveChanges();

            }
            catch
            {
            }
            try
            {
                EmailModel emailmodel = new EmailModel();
                emailmodel.Subject = "Your Order Created Successfully";
                emailmodel.To = "kanjasaha@gmail.com"; //"kanjasaha@gmail.com";
                emailmodel.EmailBody = "<div>Your order has been created successfully and your order number is " + orderId + "<div>";
                Common.sendeMail(emailmodel, true);
            }
            catch
            {
            }
        }

        public ActionResult PaymentWithPaypal(int orderId)
        {
            //getting the apiContext as earlier
            APIContext apiContext = Configurations.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist

                    //it is returned by the create function call of the payment class

                    // Creating a payment

                    // baseURL is the url on which paypal sendsback the data.

                    // So we have provided URL of this controller only

                    string baseURI = ConfigurationManager.AppSettings["NotifyURL"] + "?orderId=" + orderId+"&";//Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session

                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url

                    //on which payer is redirected for paypal acccount payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, orderId);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        //return View("FailureView");
                        return RedirectToAction("FailureView");
                    }

                }
            }
            catch (Exception ex)
            {
                //Logger.Log("Error" + ex.Message);
                //return View("FailureView");
                throw ex;
                return RedirectToAction("FailureView");
            }

            //return View("SuccessView");
            return RedirectToAction("Success", new { orderId = orderId });
        }

        private PayPal.Api.Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl, int orderId)
        {

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };
            foreach (FunOrderDetail od in dbmeals.FunOrderDetails.Where(x => x.OrderID == orderId))
            {
                itemList.items.Add(new Item()
                {
                    name = od.Description,
                    currency = "USD",
                    price = String.Format("{0:0}", od.Price),//.ToString(),
                    quantity = (od.Quantity).ToString(),
                    sku = od.ID.ToString()
                });
            }

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = String.Format("{0:0}", dbmeals.FunOrderDetails.Where(x => x.OrderID == orderId).Sum(x => (x.Price * x.Quantity)))
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = details.subtotal, // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = orderId.ToString(),
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);

        }





    }
}
