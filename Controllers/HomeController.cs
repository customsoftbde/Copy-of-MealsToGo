#region license
// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MealsToGo.Models;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.DSL;
using SolrNet.Exceptions;
using MealsToGo.Helpers;
using WebMatrix.WebData;
using System.Net;
using System.Web.Security;
using MealsToGo.ViewModels;
using JQGridMVCExamples.Models;
using Trirand.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.Routing;
using System.Configuration;
using System.Data;
using MealsToGo.Service;



namespace MealsToGo.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly ISolrReadOnlyOperations<SolrResultSet> solr;
        private ThreeSixtyTwoEntities dbmeals = new ThreeSixtyTwoEntities();
        private readonly IMealItemService _service;

        public HomeController(ISolrReadOnlyOperations<SolrResultSet> solr, IMealItemService service)
        {
            this.solr = solr;
            _service = service;
        }



        /// <summary>
        /// Builds the Solr query from the search parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ISolrQuery BuildQuery(SearchParam parameters)
        {
            if (!string.IsNullOrEmpty(parameters.FreeSearch))
                return new SolrQuery("allcolumnsearch:" + parameters.FreeSearch);
            return SolrQuery.All;
        }

        public ICollection<ISolrQuery> BuildFilterQueries(SearchParam parameters)
        {
            var queriesFromFacets = from p in parameters.Facets
                                    select (ISolrQuery)Query.Field(p.Key).Is(p.Value);




            List<ISolrQuery> filter = new List<ISolrQuery>();
            filter = queriesFromFacets.ToList();
        //    parameters.PickUpDateSearch = Convert.ToDateTime("2015-01-10 04:33:41.040"); //to be taken out later
            var Day1 = new SolrQueryByRange<DateTime>("PickUpTime", Common.AbsoluteStart(parameters.PickUpDateSearch), Common.AbsoluteEnd(parameters.PickUpDateSearch.AddDays(90)));
            filter.Add((ISolrQuery)Day1);

            return filter;

        }

        public ActionResult ShowMap(string chefLatlng, string Name, string ChefAddress)
        {


            string[] words = chefLatlng.Split(',');
            ViewBag.latitude = words[0];
            ViewBag.longitude = words[1];
            ViewBag.Name = Name;
            ViewBag.ChefAddress = ChefAddress;

            return View();
        }
        /// <summary>
        /// All selectable facet fields
        /// </summary>
        private static readonly string[] AllFacetFields = new[] { "Cuisine", "Provider", "Diet", "PriceRange", "Meal" };


        /// <summary>
        /// Gets the selected facet fields
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<string> SelectedFacetFields(SearchParam parameters)
        {
            return parameters.Facets.Select(f => f.Key);
        }


        public SortOrder[] GetSelectedSort(SearchParam parameters)
        {
            if (parameters.Sort == "Distance asc")

                parameters.Sort = "geodist() asc";

            else if (parameters.Sort == "Distance desc")

                parameters.Sort = "geodist() desc";


            return new[] { SortOrder.Parse(parameters.Sort) }.Where(o => o != null).ToArray();
        }

        public ActionResult Index2(SearchParam parameters)
        {

            return View();

        }

        public ActionResult Index(SearchParam parameters)
        {
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            else
            {
                ViewBag.ErrorMessage = "";
            }
            var ChangeLocation = HttpContext.Request.QueryString["ChangeLocation"];

            if ((ChangeLocation != null) && (ChangeLocation != ""))
            {


                GLatLong loc = new GLatLong();
                loc = GeoCodingHelper.GetLatLong(ChangeLocation.ToString());


                if (loc != null)
                {
                    Session["UserLocLat"] = loc.Latitude;
                    Session["UserLocLong"] = loc.Longitude;
                }
                Session["UserLoc"] = ChangeLocation;
            }
            else
            {
                var LocationsSearched = dbmeals.LocationsSearcheds.Where(x => x.UserID == WebSecurity.CurrentUserId).OrderByDescending(x => x.DateCreated).ToList().FirstOrDefault();
                if (LocationsSearched != null)
                {
                    Session["UserLoc"] = LocationsSearched.Location;
                    parameters.FreeSearch = LocationsSearched.Keywords;
                    Session["UserLocLat"] = LocationsSearched.Latitude;
                    Session["UserLocLong"] = LocationsSearched.Longitude;
                    parameters.DistanceSearch = LocationsSearched.Distance.HasValue ? LocationsSearched.Distance.Value.ToString() : string.Empty;
                    parameters.PickUpDateSearch = Common.AbsoluteEnd(LocationsSearched.DateRangeStart.HasValue ? LocationsSearched.DateRangeStart.Value : DateTime.Now);// LocationsSearched.DateRangeStart;
                    //parameters.DateRangeEnd = LocationsSearched.DateRangeEnd;
                }
            }


            if (String.IsNullOrEmpty(parameters.DistanceSearch))
                parameters.DistanceSearch = "100";

            if (parameters.PickUpDateSearch == null)
                parameters.PickUpDateSearch = DateTime.Now;



            string pointofreference = Session["UserLocLat"] + "," + Session["UserLocLong"];

            LocationsSearched ls = new LocationsSearched();
            ls.Location = Convert.ToString(Session["UserLoc"]);//.ToString();
            ls.Keywords = parameters.FreeSearch;
            ls.Latitude = Convert.ToDecimal(Session["UserLocLat"]);
            ls.Longitude = Convert.ToDecimal(Session["UserLocLong"]);
            ls.DateRangeStart = Common.AbsoluteStart(parameters.PickUpDateSearch);
            ls.DateRangeEnd = Common.AbsoluteEnd(parameters.PickUpDateSearch);
            ls.Distance = Convert.ToInt32(parameters.DistanceSearch);
            ls.DistanceUnit = "km";
            ls.DateCreated = DateTime.Now;
            ls.UserID = WebSecurity.CurrentUserId;
            try
            {
                dbmeals.LocationsSearcheds.Add(ls);
                dbmeals.SaveChanges();
            }
            catch (Exception e)
            {

                string message = e.Message;
            }
            try
            {


                ICollection<ISolrFacetQuery> list =
                AllFacetFields.Except(SelectedFacetFields(parameters)).
        Select(f => new SolrFacetFieldQuery(f) { MinCount = 1 })
        .Cast<ISolrFacetQuery>().ToList();



                var start = (parameters.PageIndex - 1) * parameters.PageSize;

                var matchingProducts = solr.Query(BuildQuery(parameters), new QueryOptions
                {
                    FilterQueries = BuildFilterQueries(parameters),
                    Rows = parameters.PageSize,
                    Start = start,
                    OrderBy = GetSelectedSort(parameters),
                    SpellCheck = new SpellCheckingParameters(),
                    Facet = new FacetParameters
                    {
                        Queries = list


                    },


                    ExtraParams = new Dictionary<string, string>
                                          {
                                              // uncomment for filtering by distance
                                              {"fq", "{!geofilt}"},
                                              {"d", parameters.DistanceSearch},// distance.ToString(CultureInfo.InvariantCulture)} replace distance with your radius filter
                                              {"sfield", "latlng"}, // replace lat_long with your field in solr that stores the lat long values
                                              {"pt", pointofreference},// this is the point of reference
                                             // {"sort","geodist() asc"},
                                               {"fl","*,Distance:geodist()"},
                                     
                                          }

                });

                var distancemodel = new DistanceViewModel();


                // var matchingProducts

                var ProductModel = new ProductView
                {
                    WholeSet = matchingProducts,
                    Search = parameters,
                    TotalCount = matchingProducts.NumFound,
                    Facets = matchingProducts.FacetFields,
                    //  PickUpTimeFacet = GetPickUpTimeFacet(matchingProducts.FacetQueries),
                    // DistanceFacet = GetDistanceFacet(matchingProducts.FacetQueries),
                    DidYouMean = GetSpellCheckingResult(matchingProducts),
                    //DistanceLimitList.SelectedDistance = parameters.DistanceLimit


                };


                // Preselect the option with Value = "US"
                // Make sure you have such option in the Countries list


                List<SolrResultSet> SolrResultSetList = new List<SolrResultSet>();
                SolrResultSetList = (from n in ProductModel.WholeSet
                                     select n).ToList();

                var ProductViewModel = new ResultSetViewModel();
                ProductViewModel.Search = ProductModel.Search;
                ProductViewModel.TotalCount = ProductModel.TotalCount;
                ProductViewModel.Facets = ProductModel.Facets;
                ProductViewModel.DidYouMean = ProductModel.DidYouMean;
                ProductViewModel.ProviderList = (from n in SolrResultSetList
                                                 group n by new
                                                 {
                                                     n.ProviderName,
                                                     n.ProviderType
                                                     ,
                                                     n.Distance,
                                                     n.latlng,
                                                     n.PhoneNumber,
                                                     n.FullAddress,
                                                     n.Cuisine
                                                 } into g
                                                 select new Provider()
                                                 {
                                                     ProviderName = g.Key.ProviderName,
                                                     ProviderType = g.Key.ProviderType,
                                                     Distance = g.Key.Distance,
                                                     latlng = g.Key.latlng,
                                                     PhoneNumber = g.Key.PhoneNumber,
                                                     FullAddress = g.Key.FullAddress,
                                                     Cuisine = g.Key.Cuisine,
                                                 }).ToList();

                foreach (Provider p in ProductViewModel.ProviderList)
                {



                    p.Products = (from n in SolrResultSetList
                                  where n.ProviderName == p.ProviderName
                                  select new Product()

                                      {
                                          MealAdID = n.MealAdID,
                                          MealItemName = n.MealItemName,
                                          FoodType = n.FoodType,
                                          MealType = n.MealType,
                                          Price = n.Price,
                                          Ingredients = n.Ingredients,
                                          AllergenicIngredients = n.AllergenicIngredients,
                                          Timestamp = n.Timestamp,
                                          Description = n.Description



                                      }).ToList();
                }
                

                distancemodel.SelectedDistanceLimit = parameters.DistanceSearch;
                ProductViewModel.DistanceDD = distancemodel;

                return View(ProductViewModel);

            }
            catch (SolrConnectionException e)
            {

                string msg = e.Message;
                return View(new ResultSetViewModel
                {
                    QueryError = true,
                });

            }

        }



        public ActionResult LocationToSearch()
        {

            // SearchParam searchparam = new SearchParam();

            //searchparam.UserID = userid;

            return View();


        }

        [HttpPost]
        public ActionResult LocationToSearch(string address)
        {


            if (!string.IsNullOrEmpty(address))
            {



                GLatLong loc = new GLatLong();
                //    loc = GeoCodingHelper.GetLatLong(address);//uncomment this

                loc.Latitude = 41.330215; //comment this
                loc.Longitude = -73.859004;//comment this

                Session["UserLocLat"] = loc.Latitude;
                Session["UserLocLong"] = loc.Longitude;
                Session["UserLoc"] = address;



                LocationsSearched ls = new LocationsSearched();
                ls.Location = address;
                ls.Latitude = Convert.ToDecimal(loc.Latitude);
                ls.Longitude = Convert.ToDecimal(loc.Longitude);
                ls.DateCreated = DateTime.Now;
                ls.UserID = WebSecurity.CurrentUserId;
                dbmeals.LocationsSearcheds.Add(ls);
                dbmeals.SaveChanges();


                SearchParam searchparam = new SearchParam();
                return RedirectToAction("Index", "Home", new RouteValueDictionary(searchparam));

            }

            return View();
        }


        public ContentResult GetEmployeeDetails(int employeeID)
        {
            //  var northWindModel = new NorthwindDataContext();
            string result = @"
                        ";

            var employee = (from e in dbmeals.ActiveMealAds
                            where e.MealAdID == employeeID
                            select e).First();

            ContentResult cr = new ContentResult();
            cr.Content = String.Format(result,
                                        employee.FirstName,
                                        employee.LastName,
                //employee.Title,
                //employee.TitleOfCourtesy,
                //employee.BirthDate.ToString(),
                //employee.HireDate.ToString(),
                //employee.Address,
                                        employee.City,
                // employee.PostalCode,
                                        Url.Content("~/content/images/hierarchycustomrowdetails/" + employee.MealAdID.ToString() + ".jpg")
                                       );

            return cr;
        }


        public ActionResult CheckUser()
        {            
            if (Session["FirstName"] != null)
            {
                return RedirectToAction("PrivacyRules", "Settings");
               // return View("/Settings/PrivacyRules");
            }
            else
            {
                ModelState.AddModelError("", "Please login to go for this menu");
                //ViewBag.ErrorMessage = "Please login to go for this menu";
               // er.PrError = "Please login to go for this menu";
               // Session["ErrorMessage"] = "Please login to go for this menu";
                TempData["ErrorMessage"] = "To access this menu, please login into the site";
                return RedirectToAction("Index", "Home");
                
                //return View("/Home/Index");
            }
        }


        private IDictionary<string, int> GetPickUpTimeFacet(IDictionary<string, int> fc)
        {

            return fc.Where(p => p.Key.Contains("PickUpTime")).ToDictionary(p => p.Key, p => p.Value);



        }
        private IDictionary<string, int> GetDistanceFacet(IDictionary<string, int> fc)
        {

            return fc.Where(p => p.Key.Contains("Distance")).ToDictionary(p => p.Key, p => p.Value);



        }

        private string GetSpellCheckingResult(SolrQueryResults<SolrResultSet> Provider)
        {
            return string.Join(" ", Provider.SpellChecking
                                        .Select(c => c.Suggestions.FirstOrDefault())
                                        .Where(c => !string.IsNullOrEmpty(c))
                                        .ToArray());


        }
        #region Add to cart and paypal integration

        public JsonResult GetCount()
        {
            List<TempOrderList> lstTempOrderList = dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId).ToList();
            int count = lstTempOrderList.Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Cart()
        {
            List<TempOrderList> lstTempOrderList = dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId).ToList();
            return View(lstTempOrderList);
        }
        public JsonResult CartJsonData()
        {
            List<TempOrderList> lstTempOrderList = dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId).ToList();
            //lstTempOrderList = dbmeals.TempOrderLists.Where(x => x.id != 0).ToList();
            //if (lstTempOrderList == null)
            //{
            //    lstTempOrderList.Add(new TempOrderList());
            //    lstTempOrderList.Add(new TempOrderList());
            //    lstTempOrderList.Add(new TempOrderList());
            //}
            return Json(lstTempOrderList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FinalOrder()
        {
            List<TempOrderList> lstTempOrderList = dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId).ToList();
            return View(lstTempOrderList);
        }
        public JsonResult GetShippingInformation()
        {
            //dbmeals.Configuration.LazyLoadingEnabled = false;
            AddressViewModel objAddressList = dbmeals.AddressLists
                .Where(x => x.UserId == WebSecurity.CurrentUserId).OrderByDescending(y => y.DateUpdated)
                .Select(z => new AddressViewModel
                {
                    Address1 = z.Address1
                ,
                    Address2 = z.Address2
                ,
                    City = z.City
                ,
                    CountryID = z.CountryID
                ,
                    Province = z.Province
                ,
                    Telephone = z.Telephone
                ,
                    Zip = z.Zip
                })
                .FirstOrDefault();
            if (objAddressList == null)
                objAddressList = new AddressViewModel();
            var countryList = dbmeals.countries.ToList();
            var data = new { Country = countryList, ShippingAddress = objAddressList };
            //dbmeals.Configuration.LazyLoadingEnabled = true;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddtoCartMealItem(string mealItemId, int qty, string itemName, decimal price)
        {
            AddtoCart(mealItemId, qty, itemName, price);
            return RedirectToAction("Cart");
        }
        public JsonResult AddtoCart(string mealItemId, int qty, string itemName, decimal price)
        {

            TempOrderList tempOrder = new TempOrderList();
            tempOrder.MealItemId = mealItemId;
            tempOrder.qty = qty;
            tempOrder.userid = WebSecurity.CurrentUserId;
            tempOrder.sessionId = Session.SessionID;

            tempOrder.lineitemcost = price;
            tempOrder.TotalCost = qty * price;
            //dbmeals.Entry(dbmeals.TempOrderLists).State = EntityState.Added;
            dbmeals.TempOrderLists.Add(tempOrder);
            int id = Convert.ToInt32(mealItemId);
            var mealItem = dbmeals.MealAds.Where(x => x.MealAdID == id).FirstOrDefault();
            if (mealItem != null)
            {
                tempOrder.itemName = mealItem.MealItem.MealItemName;
                mealItem.MealItem.Quantity = mealItem.MealItem.Quantity - qty;
            }
            dbmeals.SaveChanges();
            int count = dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId).Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getOrderProdcutList()
        {
            var productOrderList = dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId);
            return Json(productOrderList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveAllFromCart(int id)
        {
            Models.TempOrderList TempOrderList = dbmeals.TempOrderLists.FirstOrDefault(x => x.id == id);
            if (TempOrderList != null)
            {
                dbmeals.TempOrderLists.Remove(TempOrderList);
                dbmeals.SaveChanges();
            }
            return RedirectToAction("Cart");
        }
        public JsonResult UpdatetoCart(int id, int qty)
        {
            Models.TempOrderList tempOrder = dbmeals.TempOrderLists.FirstOrDefault(x => x.id == id);
            if (tempOrder != null)
            {
                tempOrder.MealItemId = id.ToString();
                tempOrder.qty = qty;
                tempOrder.sessionId = Session.SessionID;
                tempOrder.TotalCost = tempOrder.lineitemcost * qty;
                dbmeals.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProceedToCheckOutFinalOrder(int DeliveryMode, DateTime DeliveryDateTime, int PaymentOptionID, string Address1, string Address2, string City, int CountryID, string Province, Int64? Telephone, string Zip)
        {
            if (DeliveryMode == 3)
            {
                var userDetail = dbmeals.UserDetails.Where(x => x.UserId == WebSecurity.CurrentUserId).FirstOrDefault();
                if (userDetail != null)
                {
                    AddressList objAddressList = userDetail.AddressList;// dbmeals.AddressLists.Where(x => x.UserId == WebSecurity.CurrentUserId).OrderByDescending(y => y.DateUpdated).FirstOrDefault();
                    bool isnew = false;
                    if (objAddressList == null)
                    {
                        objAddressList = new AddressList();
                        isnew = true;
                    }
                    objAddressList.UserId = WebSecurity.CurrentUserId;
                    objAddressList.Address1 = Address1;
                    objAddressList.Address2 = Address2;
                    objAddressList.City = City;
                    objAddressList.CountryID = CountryID;
                    objAddressList.Province = Province;
                    objAddressList.Telephone = (Telephone.HasValue ? Telephone.ToString() : string.Empty);
                    objAddressList.IsBillingAddress = 1;
                    objAddressList.DateUpdated = DateTime.Now;
                    if (isnew)
                    {
                        dbmeals.AddressLists.Add(objAddressList);
                        dbmeals.SaveChanges();
                    }
                }
            }
            Models.FunOrder order = new Models.FunOrder();
            order.Status = (PaymentOptionID == 1) ? 1 : 0;
            order.UserId = WebSecurity.CurrentUserId;
            order.DateCreated = DateTime.Now;
            order.DateUpdated = DateTime.Now;
            order.ActualPickUpTime = DeliveryDateTime;

            //delivery=1,pickup 2
            order.DeliveryMethodID = DeliveryMode;
            order.PaymentOptionID = PaymentOptionID;
            order.EstimatedPickupTime = DeliveryDateTime;
            List<TempOrderList> lstTempOrderList = dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId).ToList();
            foreach (TempOrderList TempOrderList in lstTempOrderList)
            {
                FunOrderDetail orderDetail = new FunOrderDetail();
                orderDetail.Quantity = TempOrderList.qty;
                orderDetail.Description = TempOrderList.itemName;
                orderDetail.Price = TempOrderList.lineitemcost;
                order.Total = TempOrderList.TotalCost;
                order.FunOrderDetails.Add(orderDetail);
                int mealid = Convert.ToInt32(TempOrderList.MealItemId);
                var activeMealAd = dbmeals.ActiveMealAds.Where(x => x.MealAdID == mealid).FirstOrDefault();
                if (activeMealAd != null)
                {
                    activeMealAd.Quantity = activeMealAd.Quantity - TempOrderList.qty;
                    dbmeals.SaveChanges();
                }
            }
            if (dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId) != null)
            {
                order.Total = dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId).Sum(y => (y == null) ? 0 : y.TotalCost);
            }
            dbmeals.FunOrders.Add(order);
            if (PaymentOptionID == 1)
            {
                foreach (TempOrderList tempData in dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId))
                {
                   // dbmeals.TempOrderLists.Remove(tempData);
                }
            }
            foreach (TempOrderList TempOrderList in lstTempOrderList)
            {
                int MealAdID = Convert.ToInt32(TempOrderList.MealItemId);
                var activeMealAd = dbmeals.ActiveMealAds.Where(x => x.MealAdID == MealAdID).FirstOrDefault();
                if (activeMealAd != null)
                {
                    activeMealAd.Quantity = activeMealAd.Quantity - TempOrderList.qty;
                    dbmeals.SaveChanges();
                }
            }
            dbmeals.SaveChanges();
            int orderid = order.OrderID;
            decimal totalAmount = lstTempOrderList.Sum(x => x.TotalCost);
            var data = new { OrderId = orderid, amount = totalAmount, isCashPayment = PaymentOptionID == 1 };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProceedToCheckOut()
        {
            //Models.FunOrder order = new Models.FunOrder();
            //order.Status = 0;
            //order.UserId = WebSecurity.CurrentUserId;
            //order.DateCreated = DateTime.Now;
            //order.DateUpdated = DateTime.Now;
            //order.ActualPickUpTime = DeliveryDateTime;

            ////delivery=1,pickup 2
            //order.DeliveryMethodID = DeliveryMode;
            //order.PaymentOptionID = PaymentOptionID;
            //order.EstimatedPickupTime = DeliveryDateTime;
            //List<TempOrderList> lstTempOrderList = dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId).ToList();
            //foreach (TempOrderList TempOrderList in lstTempOrderList)
            //{
            //    FunOrderDetail orderDetail = new FunOrderDetail();
            //    orderDetail.Quantity = TempOrderList.qty;
            //    orderDetail.Description = TempOrderList.itemName;
            //    orderDetail.Price = TempOrderList.lineitemcost;
            //    order.Total = TempOrderList.TotalCost;
            //    order.FunOrderDetails.Add(orderDetail);
            //}
            //order.Total = dbmeals.TempOrderLists.Where(x => x.sessionId == Session.SessionID && x.userid == WebSecurity.CurrentUserId).Sum(y => y.TotalCost);
            //dbmeals.FunOrders.Add(order);
            //int orderid = order.OrderID;
            //dbmeals.SaveChanges();

            // decimal totalAmount = lstTempOrderList.Sum(x => x.TotalCost);

            // return RedirectToAction("PostToPaypal", new { orderId = orderid, amount = totalAmount.ToString() });
            return View();
        }
        public ActionResult PostToPaypal(int orderId, string amount)
        {
            Models.Paypal paypal = new Models.Paypal();
            paypal.cmd = "_xclick";
            paypal.business = ConfigurationManager.AppSettings["BusinessAccountKey"];
            //if (useSandbox == "1")
            //    ViewBag.actionURl = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            //else
            //    ViewBag.actionURl = "https://www.paypal.com/cgi-bin/webscr";

            bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSandbox"]);
            if (useSandbox)
            {
                //sandbox url
                ViewBag.actionURl = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            }
            else
            {
                ViewBag.actionURl = "https://www.paypal.com/cgi-bin/webscr";
            }
            paypal.cancel_return = ConfigurationManager.AppSettings["CancelURL"] + "?orderId=" + orderId+"&";
            paypal.@return = ConfigurationManager.AppSettings["ReturnURL"] + "?orderId=" + orderId + "&";
            paypal.notify_url = ConfigurationManager.AppSettings["NotifyURL"] + "?orderId=" + orderId + "&";
            paypal.currency_code = ConfigurationManager.AppSettings["CurrencyCode"];

            paypal.PaypalItems = new List<PaypalItem>();
            foreach (FunOrderDetail od in dbmeals.FunOrderDetails.Where(x => x.OrderID == orderId))
            {
                PaypalItem oPaypalItem = new PaypalItem();
                oPaypalItem.item_name = od.Description;
                oPaypalItem.Quantity = (od.Quantity).ToString();
                oPaypalItem.amount = od.Price.ToString();
                paypal.PaypalItems.Add(oPaypalItem);
            }
            return View(paypal);
        }
        public ActionResult Notify(int orderId)
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
            return View();
        }
        public ActionResult Success(int orderId)
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
            return View();
        }
        public ActionResult Cancel(int orderId)
        {
            Models.FunOrder order = dbmeals.
                FunOrders.FirstOrDefault(x => x.OrderID == orderId);
            if (order != null)
            {
                order.Status = 2;
            }
            dbmeals.SaveChanges();
            return View();
        }

        #endregion
    }
}