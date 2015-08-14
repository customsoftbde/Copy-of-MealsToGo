using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text;
using System.IO;

namespace MealsToGo.Controllers
{
    public class IPNController : Controller
    {
        //
        // GET: /IPN/

  

public ActionResult IPN()
{
// Receive IPN request from PayPal and parse all the variables returned
var formVals = new Dictionary<string, string>();
formVals.Add("cmd", "_notify-validate");
 
// if you want to use the PayPal sandbox change this from false to true
string response = GetPayPalResponse(formVals, true);
 
if (response == "VERIFIED")
{
string transactionID = Request["txn_id"];
string sAmountPaid = Request["mc_gross"];
string deviceID = Request["custom"];
 
//validate the order
Decimal amountPaid = 0;
Decimal.TryParse(sAmountPaid, out amountPaid);
 
if (sAmountPaid == "2.95")
{
// take the information returned and store this into a subscription table
// this is where you would update your database with the details of the tran
 
return View();
 
}
else
{
// let fail - this is the IPN so there is no viewer
// you may want to log something here
}
}
 
return View();
}

string GetPayPalResponse(Dictionary<string, string> formVals, bool useSandbox)
{
 
// Parse the variables
// Choose whether to use sandbox or live environment
string paypalUrl = useSandbox ? "https://www.sandbox.paypal.com/cgi-bin/webscr"
: "https://www.paypal.com/cgi-bin/webscr";
 
HttpWebRequest req = (HttpWebRequest)WebRequest.Create(paypalUrl);
 
// Set values for the request back
req.Method = "POST";
req.ContentType = "application/x-www-form-urlencoded";
 
byte[] param = Request.BinaryRead(Request.ContentLength);
string strRequest = Encoding.ASCII.GetString(param);
 
StringBuilder sb = new StringBuilder();
sb.Append(strRequest);
 
foreach (string key in formVals.Keys)
{
sb.AppendFormat("&{0}={1}", key, formVals[key]);
}
strRequest += sb.ToString();
req.ContentLength = strRequest.Length;
 
//for proxy
//WebProxy proxy = new WebProxy(new Uri("http://urlort#");
//req.Proxy = proxy;
//Send the request to PayPal and get the response
string response = "";
using (StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII))
{
 
streamOut.Write(strRequest);
streamOut.Close();
using (StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream()))
{
response = streamIn.ReadToEnd();
}
}
 
return response;
}

           
    }
}
