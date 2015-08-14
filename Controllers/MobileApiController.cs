using AutoMapper;
using MealsToGo.Helpers;
using MealsToGo.Models;
using MealsToGo.Service;
using MealsToGo.ViewModels;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.DSL;
using SolrNet.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace MealsToGo.Controllers
{
    public class MobileApiController : Controller
    {
        // private readonly IMealItemService _service;


        // public MobileApiController()
        //{
        //    _service = new MealItemService();
        //}
        /// <summary>
        /// All selectable facet fields
        /// </summary>
        private static readonly string[] AllFacetFields = new[] { "Cuisine", "Provider", "Diet", "PriceRange", "Meal" };
        private readonly ISolrReadOnlyOperations<SolrResultSet> solr;
        private ThreeSixtyTwoEntities dbmeals = new ThreeSixtyTwoEntities();

        public MobileApiController(ISolrReadOnlyOperations<SolrResultSet> solr)
        {
            this.solr = solr;

        }

        [HttpGet]
        // [AllowAnonymous]
        public JsonResult test()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost()]
        // [AllowAnonymous]
       // [ValidateAntiForgeryToken]
        public JsonResult Register(LoginRegisterViewModel viewmodel)
        {
            bool res = true;
            //return Json(viewmodel, JsonRequestBehavior.AllowGet);
            try
            {
            LoginModel model = new LoginModel(); 
            model = Mapper.Map<LoginRegisterViewModel, LoginModel>(viewmodel);
            //return Json(viewmodel, JsonRequestBehavior.AllowGet);
            
            
                // Attempt to register the user
               
                    string confirmationToken =
                        WebSecurity.CreateUserAndAccount(viewmodel.UserName, viewmodel.Password,propertyValues: new
                        {
                            FirstName = viewmodel.FirstName
                        }, requireConfirmationToken: true); //new {Email=model.Email}

                   
                    EmailModel emailmodel = new EmailModel();
                    emailmodel.To = viewmodel.UserName;
                    emailmodel.Subject = "Welcome to Fun Fooding";


                    StringBuilder sb = new StringBuilder();
                    sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                    sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                    sb.Append("<head>");
                    sb.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                    sb.Append("</head>");
                    sb.Append("<body>");
                    sb.Append("<div style=\"padding:20px; font:normal 14px Arial, Helvetica, sans-serif; color:#333333;\">");
                    sb.Append("Hi " + viewmodel.FirstName + ",<br />");
                    sb.Append("Thank you for signing up!<br />");
                    sb.Append("<br />");
                    sb.Append("To complete the sign up process click ");
                    sb.Append("<a href=" + ConfigurationManager.AppSettings["funfoodingUrl"] + "/Account/EmailConfirmation/" + confirmationToken + " style=\"color:#0066CC\"> here</a>.<br />");
                    sb.Append("<br />");
                    sb.Append("If you have any problem completing the process, please contact <a href=\"#\" style=\"color:#0066CC\">support@gmail.com</a>.<br />");
                    sb.Append("<br /> ");
                    sb.Append("Best regards,<br />");
                    sb.Append("Support Team<br />");
                    sb.Append("<a href=\"http://funfooding.com/\" style=\"color:#0066CC\">www.funfooding.com</a></div>");
                    sb.Append("</body>");
                    sb.Append("</html>");
                    emailmodel.EmailBody = sb.ToString();
                    Common.sendeMail(emailmodel, true);
                    return Json(new { success = res}, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    res = false;
                    //ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    return Json(new { success=res, msg = e }, JsonRequestBehavior.AllowGet);
                }
            

            // If we got this far, something failed, redisplay form
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost()]
        // [AllowAnonymous]
       // [ValidateAntiForgeryToken]
        public JsonResult Login(LoginRegisterViewModel viewmodel)
        {
            UsersContext db = new UsersContext();
            bool res = true;
            
            LoginModel model = new LoginModel();
            model = Mapper.Map<LoginRegisterViewModel, LoginModel>(viewmodel);
           // return Json(viewmodel, JsonRequestBehavior.AllowGet);

            if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {

                var user = db.UserProfiles.Where(x => x.UserName.Equals(model.UserName)).First();
                int UserID = user.UserId;

                return Json(new { success = res, userData = user }, JsonRequestBehavior.AllowGet);
            }
            bool UserExists = db.UserProfiles.Any(x => x.UserName.Equals(model.UserName));
            if (UserExists)

                // If we got this far, something failed, redisplay form
                // ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return Json(new { success = res, msg = "The user name or password provided is incorrect." }, JsonRequestBehavior.AllowGet);
            else
                //ModelState.AddModelError("", "There is no account with that UserName. Please sign up");
                return Json(new { success = res, msg = "There is no account with that UserName. Please sign up" }, JsonRequestBehavior.AllowGet);

            return Json(res, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SearchMealItem(string mealItemName)
        {
            if (!string.IsNullOrEmpty(mealItemName))
            {
                IMealItemService _service = new MealItemService();
                IEnumerable<MealItem> mt = _service.GetAll().Where(x => x.MealItemName.Contains(mealItemName.Trim()));
                IEnumerable<MealItemViewModel> mtvm = Mapper.Map<IEnumerable<MealItem>, IEnumerable<MealItemViewModel>>(mt);
                return Json(mtvm, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, msg = "Meal item name cannnot be empty" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult MyMealItem(int userID)
        {
            IMealItemService _service = new MealItemService();
            IEnumerable<MealItem> mt = _service.FindByUser(userID);
            IEnumerable<MealItemViewModel> mtvm = Mapper.Map<IEnumerable<MealItem>, IEnumerable<MealItemViewModel>>(mt);
            //mtvm = PopulateDropDown(mtvm);

            return Json(mtvm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateMealItemDD(int userID)
        {

            MealItem mt = new MealItem();
            mt.UserId = userID;

            MealItemViewModel mtvm = Mapper.Map<MealItem, MealItemViewModel>(mt);

            mtvm = PopulateDropDown(mtvm, mt);

            return Json(mtvm, JsonRequestBehavior.AllowGet);
        }

        private MealItemViewModel PopulateDropDown(MealItemViewModel mtvm, MealItem mealitem)
        {
            IMealItemService _service = new MealItemService();
            if (mtvm == null)
                mtvm = new MealItemViewModel();
            mtvm.ServingUnitDDList = _service.GetServingUnitDDList().Select(x => new SelectListItem
            {
                Value = x.ServingUnitID.ToString(),
                Text = x.ServingUnit,
                Selected = (mealitem != null && mealitem.ServingUnit == x.ServingUnitID)
            });

            mtvm.MealTypeDD.MealTypeDDList = _service.MealTypeDDList().Select(x => new SelectListItem
            {
                Value = x.MealTypeID.ToString(),
                Text = x.Name,
                Selected = (mealitem != null && mealitem.MealTypeID == x.MealTypeID)
            });
            mtvm.CusineTypeDD.CuisineDDList = _service.CuisineTypeDDList().Select(x => new SelectListItem
            {
                Value = x.CuisineTypeID.ToString(),
                Text = x.Name,
                Selected = (mealitem != null && mealitem.CusineTypeID == x.CuisineTypeID)
            });

            mtvm.DietTypeDD.DietTypeDDList = _service.DietTypeDDList().Select(x => new SelectListItem
            {
                Value = x.DietTypeID.ToString(),
                Text = x.Name,
                Selected = (mealitem != null && mealitem.DietTypeID == x.DietTypeID)
            });
            mtvm.AllergenDD = _service.AllergenicFoodsDDList().Select(x => new Allergen
            {
                AllergenName = x.AllergenicFood,
                AllergenID = x.AllergenicFoodID,
                Selected = (mealitem != null && mealitem.MealItems_AllergenicFoods.Where(y => y.AllergenicFoodID == x.AllergenicFoodID).Count() > 0)
            }).ToList();

            return mtvm;
        }
        public JsonResult CreateMealItem1()
        {
            MealItemViewModel MealAdvm = new MealItemViewModel();
            MealAdvm.AllergenDD = new List<Allergen>();
            MealAdvm.AllergenDD.Add(new Allergen());
            //MealAdvm.AvailabilityTypeDD = new AvailabilityTypeViewModel();
            return Json(MealAdvm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateMealItem(MealItemViewModel mtvms)
        {
            IMealItemService _service = new MealItemService();
            MealItem mealitem = Mapper.Map<MealItemViewModel, MealItem>(mtvms);
            foreach (var mealaller in mtvms.AllergenDD)
            {
                if (mealaller.Selected)
                {
                    MealItems_AllergenicFoods aller = new MealItems_AllergenicFoods();
                    aller.AllergenicFoodID = mealaller.AllergenID;
                    mealitem.MealItems_AllergenicFoods.Add(aller);
                }
            }

            mealitem.MealItemId = _service.AddAndReturnID(mealitem);

            return Json(new { success = true, id = mealitem.MealItemId }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult MyMealAd(int userid)
        {
            ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();
            IMealAdService _service = new MealAdService();
            IEnumerable<MealAd> mealad = _service.FindByUser(userid);
            ViewBag.AddressPresent = false;
            ViewBag.PrivacySettingConfirmed = false;
            ViewBag.SellerTermsandSettingAccepted = false;

            //if (mealad.Count() == 0)
            //{
            //    ViewBag.MealItemCount = db.MealItems.Where(x => x.UserId == userid).Count();
            //    ViewBag.AddressPresent = db.UserDetails.Any(x => x.UserId == userid && x.AddressID != null);
            //    ViewBag.PrivacySettingConfirmed = db.UserSettings.Where(x => x.UserID == userid).All(x => x.Verified == 1);
            //    var useragreed = db.UserAgreementsAcceptanceDetails.Where(x => x.UserID == userid).FirstOrDefault();
            //    if (useragreed != null)

            //        ViewBag.SellerTermsandSettingAccepted = true;
            //    if (!((ViewBag.MealItemCount != 0) && (ViewBag.AddressPresent) && (ViewBag.PrivacySettingConfirmed) && (ViewBag.SellerTermsandSettingAccepted)))
            //    {

            //        Session["ReadyToAdvertise"] = "0";
            //    }


            //}

            IEnumerable<MealAdViewModel> mealadvm = Mapper.Map<IEnumerable<MealAd>, IEnumerable<MealAdViewModel>>(mealad);

            foreach (var item in mealad)
            {
                if (item.MealItem != null)
                    mealadvm.Where(x => x.MealAdID == item.MealAdID).FirstOrDefault().MealItemName = item.MealItem.MealItemName;
            }

            return Json(mealadvm, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateMealAdDD(int userid)
        {
            MealAd mealad = new MealAd();
            MealAdViewModel mealadvm = Mapper.Map<MealAd, MealAdViewModel>(mealad);
            mealadvm = PopulateDropDown(mealadvm, mealad, userid);
            return Json(mealadvm, JsonRequestBehavior.AllowGet);

        }
        public JsonResult CreateMealAd1()
        {
            MealAdViewModel MealAdvm = new MealAdViewModel();
            MealAdvm.AvailabilityTypeDD = new AvailabilityTypeViewModel();
            return Json(MealAdvm, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateMealAd(MealAdViewModel MealAdvm)
        {
            IMealAdService _service = new MealAdService();
            ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();
            MealAd mealad = Mapper.Map<MealAdViewModel, MealAd>(MealAdvm);

            foreach (var payment in MealAdvm.PaymentMethods)
            {
                if (payment.Selected)
                {

                    MealAds_PaymentOptions paymentoptions = new MealAds_PaymentOptions();
                    paymentoptions.PaymentOptionID = payment.PaymentMethodID;
                    mealad.MealAds_PaymentOptions.Add(paymentoptions);
                }

            }

            foreach (var delivery in MealAdvm.DeliveryMethods)
            {
                if (delivery.Selected)
                {

                    MealAds_DeliveryMethods deliverymthd = new MealAds_DeliveryMethods();
                    deliverymthd.DeliveryMethodID = delivery.DeliveryMethodID;
                    mealad.MealAds_DeliveryMethods.Add(deliverymthd);
                }

            }
            int orderingoptionnum = 0;
            var availabilityType = db.AvailabilityTypes.Where(x => x.AvaiilabilityTypeID == mealad.AvailabilityTypeID).FirstOrDefault();
            if (availabilityType != null)
            {

                string orderingoption = availabilityType.AvailabilityType1;
                orderingoptionnum = Convert.ToInt32(orderingoption);
            }
            foreach (var schedules in MealAdvm.MealAdSchedules)
            {

                MealAd_Schedules meadadschedule = new MealAd_Schedules();
                meadadschedule.PickUpStartDateTime = schedules.PickUpStartDateTime;
                meadadschedule.PickUpEndDateTime = schedules.PickUpEndDateTime;
                meadadschedule.LastOrderDateTime = schedules.PickUpEndDateTime.AddHours(-orderingoptionnum);
                mealad.MealAd_Schedules.Add(meadadschedule);


            }

            mealad.MealAdID = _service.AddAndReturnID(mealad);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private MealAdViewModel PopulateDropDown(MealAdViewModel mealadvm, MealAd mealad, int userid)
        {
            ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();
            IMealAdService _service = new MealAdService();
            if (mealadvm == null)
                mealadvm = new MealAdViewModel();

            var MealItems = from b in db.MealItems
                            where b.UserId == userid
                            select b;

            IEnumerable<MealItem> MealItemsList = MealItems.ToList();
            mealadvm.MealItemsDD.MealItemsDDList =
                MealItemsList.Select(x => new SelectListItem
                {
                    Value = x.MealItemId.ToString(),
                    Text = x.MealItemName,
                    Selected = (mealad != null && mealad.MealItemID == x.MealItemId)
                });

            var AvailTypeMethod = from b in db.AvailabilityTypes
                                  select b;

            IEnumerable<AvailabilityType> AvailTypeList = AvailTypeMethod.ToList();
            mealadvm.AvailabilityTypeDD.AvailabilityTypeDDList =
                AvailTypeList.Select(x => new SelectListItem
                {
                    Value = x.AvaiilabilityTypeID.ToString(),
                    Text = x.Descriptions,
                    Selected = (mealad != null && mealad.AvailabilityTypeID == x.AvaiilabilityTypeID)

                });

            mealadvm.DeliveryMethods = _service.GetDeliveryMethodDDList().Select(x => new DeliveryMethodViewModel
            {
                DeliveryMethod = x.DeliveryMethod,
                DeliveryMethodID = x.DeliveryMethodID,
                Selected = (mealad != null && mealad.MealAds_DeliveryMethods.Where(y => y.DeliveryMethodID == x.DeliveryMethodID).Count() > 0)
            }).ToList();


            mealadvm.PaymentMethods = _service.PaymentMethodDDList().Select(x => new PaymentMethodViewModel
            {
                PaymentMethodID = x.PaymentOptionID,
                PaymentMethod = x.PaymentOption1,
                Selected = (mealad != null && mealad.MealAds_PaymentOptions.Where(y => y.PaymentOptionID == x.PaymentOptionID).Count() > 0)
            }).ToList();

            MealAdSchedule mealads = new MealAdSchedule();
            mealadvm.MealAdSchedules = new List<MealAdSchedule>();
            if (mealad != null && mealad.MealAd_Schedules != null && mealad.MealAd_Schedules.Count > 0)
            {
                foreach (var data in mealad.MealAd_Schedules)
                {
                    mealadvm.MealAdSchedules.Add(new MealAdSchedule() { LastOrderDateTime = data.LastOrderDateTime, PickUpEndDateTime = data.PickUpEndDateTime, PickUpStartDateTime = data.PickUpStartDateTime });
                }
            }
            else
            {
                mealadvm.MealAdSchedules.Add(mealads);
            }

            return mealadvm;
        }
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        [HttpPost]
        public JsonResult AddtoCart(string mealItemId, int qty, string itemName, decimal price,string sessionId,int userId)
        {

            TempOrderList tempOrder = new TempOrderList();
            tempOrder.MealItemId = mealItemId;
            tempOrder.qty = qty;
            tempOrder.userid = userId;
            tempOrder.sessionId = sessionId;
            tempOrder.itemName = itemName;
            tempOrder.lineitemcost = price;
            tempOrder.TotalCost = qty * price;
            //dbmeals.Entry(dbmeals.TempOrderLists).State = EntityState.Added;
            dbmeals.TempOrderLists.Add(tempOrder);
            int id = Convert.ToInt32(mealItemId);
            var mealItem = dbmeals.MealItems.Where(x => x.MealItemId == id).FirstOrDefault();
            if (mealItem != null)
            {
                mealItem.Quantity = mealItem.Quantity - qty;
            }
            dbmeals.SaveChanges();
            int count = dbmeals.TempOrderLists.Where(x => x.sessionId == sessionId && x.userid == userId).Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CartList(string sessionId,int userId)
        {
            dbmeals = new ThreeSixtyTwoEntities();
            List<TempOrderList> lstTempOrderList = dbmeals.TempOrderLists.Where(x => x.sessionId == sessionId && x.userid == userId).ToList();
            return Json(lstTempOrderList, JsonRequestBehavior.AllowGet);
        }
    }
}