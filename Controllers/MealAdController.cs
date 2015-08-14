using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealsToGo.Models;
using MealsToGo.Service;
using MealsToGo.ViewModels;
using AutoMapper;
using MealsToGo.Helpers;
using WebMatrix.WebData;

namespace MealsToGo.Controllers
{
    public class MealAdController : Controller
    {
        private ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();
        EmailHelper eh = new EmailHelper();

        //
        // GET: /MealAd/


        private readonly IMealAdService _service;


        public MealAdController(IMealAdService service)
        {
            _service = service;
        }


        public ActionResult Index(int userid)
        {

            IEnumerable<MealAd> mealad = _service.FindByUser(userid);
            ViewBag.AddressPresent = false;
            ViewBag.PrivacySettingConfirmed = false;
            ViewBag.SellerTermsandSettingAccepted = false;
            Session["ReadyToAdvertise"] = "1";


            if (mealad.Count() == 0)
            {
                ViewBag.MealItemCount = db.MealItems.Where(x => x.UserId == userid).Count();
                ViewBag.AddressPresent = db.UserDetails.Any(x => x.UserId == userid && x.AddressID != null);
                ViewBag.PrivacySettingConfirmed = db.UserSettings.Where(x => x.UserID == userid).All(x => x.Verified == 1);
                var useragreed = db.UserAgreementsAcceptanceDetails.Where(x => x.UserID == userid).FirstOrDefault();
                if (useragreed != null)

                    ViewBag.SellerTermsandSettingAccepted = true;
                if (!((ViewBag.MealItemCount != 0) && (ViewBag.AddressPresent) && (ViewBag.PrivacySettingConfirmed) && (ViewBag.SellerTermsandSettingAccepted)))
                {

                    Session["ReadyToAdvertise"] = "0";
                }


            }

            IEnumerable<MealAdViewModel> mealadvm = Mapper.Map<IEnumerable<MealAd>, IEnumerable<MealAdViewModel>>(mealad);

            foreach (var item in mealad)
            {
                if (item.MealItem != null)
                    mealadvm.Where(x => x.MealAdID == item.MealAdID).FirstOrDefault().MealItemName = item.MealItem.MealItemName;
            }

            return View(mealadvm);
        }





        //
        // GET: /MealAd/Details/5

        public ActionResult Details(int id = 0)
        {
            MealAd mealad = db.MealAds.Find(id);
            if (mealad == null)
            {
                mealad = new MealAd();
            }

            MealAdViewModel mealadvm = Mapper.Map<MealAd, MealAdViewModel>(mealad);
            mealadvm.MealItemName = mealad.MealItem.MealItemName;
            mealadvm = PopulateDropDown(mealadvm, mealad, WebSecurity.CurrentUserId);
            return View(mealadvm);
        }

        //
        // GET: /MealAd/Create

        public ActionResult Create(int userid, int mealitemid = -1)
        {


            MealAd mealad = new MealAd();


            MealAdViewModel mealadvm = Mapper.Map<MealAd, MealAdViewModel>(mealad);

            mealadvm = PopulateDropDown(mealadvm, mealad, userid);


            return View(mealadvm);


        }



        //
        // POST: /MealAd/Create

        [HttpPost]
        public ActionResult Create(MealAdViewModel MealAdvm)
        {


            if (ModelState.IsValid)
            {

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
                    break;
                }

                mealad.MealAdID = _service.AddAndReturnID(mealad);

                eh.SendEmail("Insert");

                return RedirectToAction("Details", "MealAd", new { id = mealad.MealAdID });


            }

            return View(MealAdvm);
        }

        private MealAdViewModel PopulateDropDown(MealAdViewModel mealadvm, MealAd mealad, int userid)
        {
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
        public ActionResult AddScheduler()
        {
            //MealAdSchedule mealads = new MealAdSchedule();
            //mealadvm.MealAdSchedules = new List<MealAdSchedule>();

            //mealadvm.MealAdSchedules.Add(mealads);
            return PartialView("SchedulePartial", new List<MealAdSchedule>());
        }
        //
        // GET: /MealAd/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MealAd mealad = db.MealAds.Find(id);
            if (mealad == null)
            {
                return HttpNotFound();
            }
            MealAdViewModel mealadvm = Mapper.Map<MealAd, MealAdViewModel>(mealad);
            mealadvm.MealItemName = mealad.MealItem.MealItemName;
            mealadvm = PopulateDropDown(mealadvm, mealad, WebSecurity.CurrentUserId);

            return View(mealadvm);
        }

        //
        // POST: /MealAd/Edit/5

        [HttpPost]
        public ActionResult Edit(MealAdViewModel MealAdvm, FormCollection frmcoll)
        {
            if (ModelState.IsValid)
            {
                MealAd existingMealAd = db.MealAds.Find(MealAdvm.MealAdID);

                existingMealAd.MaxOrders = MealAdvm.MaxOrders;

                List<MealAd_Schedules> lstMealAd_Schedules = new List<MealAd_Schedules>();
                foreach (var allergenic in existingMealAd.MealAd_Schedules)
                    lstMealAd_Schedules.Add(allergenic);
                foreach (var allergenic in lstMealAd_Schedules)
                {
                    //mealitem.MealItems_AllergenicFoods.Remove(allergenic);

                    db.MealAd_Schedules.Remove(allergenic);

                    db.Entry(allergenic).State = EntityState.Deleted;
                    db.SaveChanges();
                }
                //existingMealAd.MealAd_Schedules.Remove(
                //db.SaveChanges();
                foreach (var schedules in MealAdvm.MealAdSchedules)
                {
                    MealAd_Schedules meadadschedule = new MealAd_Schedules();
                    meadadschedule.PickUpStartDateTime = schedules.PickUpStartDateTime;
                    meadadschedule.PickUpEndDateTime = schedules.PickUpEndDateTime;
                    meadadschedule.LastOrderDateTime = schedules.PickUpEndDateTime.AddHours(0);
                    existingMealAd.MealAd_Schedules.Add(meadadschedule);
                }

                //db.Entry(MealAd).State = EntityState.Modified;
                db.SaveChanges();
                eh.SendEmail("Update");
                return RedirectToAction("Index", new { userID = WebSecurity.CurrentUserId });
            }
            return View(MealAdvm);
        }


        public ActionResult Walkthrough()
        {
            return View();
        }

        //
        // GET: /MealAd/Delete/5

        public ActionResult Delete(int id = 0)
        {
            //MealAd MealAd = db.MealAds.Find(id);
            //if (MealAd == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(MealAd);
            MealAd existingMealAd = db.MealAds.Find(id);
            List<MealAd_Schedules> lstMealAd_Schedules = new List<MealAd_Schedules>();
            foreach (var allergenic in existingMealAd.MealAd_Schedules)
                lstMealAd_Schedules.Add(allergenic);
            foreach (var allergenic in lstMealAd_Schedules)
            {
                //mealitem.MealItems_AllergenicFoods.Remove(allergenic);

                db.MealAd_Schedules.Remove(allergenic);

                db.Entry(allergenic).State = EntityState.Deleted;
                db.SaveChanges();
            }
            List<MealAds_DeliveryMethods> lstMealAds_DeliveryMethods = new List<MealAds_DeliveryMethods>();
            foreach (var allergenic in existingMealAd.MealAds_DeliveryMethods)
                lstMealAds_DeliveryMethods.Add(allergenic);
            foreach (var allergenic in lstMealAds_DeliveryMethods)
            {
                //mealitem.MealItems_AllergenicFoods.Remove(allergenic);

                db.MealAds_DeliveryMethods.Remove(allergenic);

                db.Entry(allergenic).State = EntityState.Deleted;
                db.SaveChanges();
            }
            List<MealAds_PaymentOptions> lstMealAds_PaymentOptions = new List<MealAds_PaymentOptions>();
            foreach (var allergenic in existingMealAd.MealAds_PaymentOptions)
                lstMealAds_PaymentOptions.Add(allergenic);
            foreach (var allergenic in lstMealAds_PaymentOptions)
            {
                //mealitem.MealItems_AllergenicFoods.Remove(allergenic);

                db.MealAds_PaymentOptions.Remove(allergenic);

                db.Entry(allergenic).State = EntityState.Deleted;
                db.SaveChanges();
            }

            db.MealAds.Remove(existingMealAd);
            db.Entry(existingMealAd).State = EntityState.Deleted;
            db.SaveChanges();
            eh.SendEmail("Delete");
            return RedirectToAction("Index", new { userID = WebSecurity.CurrentUserId });
        }

        //
        // POST: /MealAd/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            MealAd MealAd = db.MealAds.Find(id);
            db.MealAds.Remove(MealAd);
            db.SaveChanges();
            eh.SendEmail("Delete");
            //return RedirectToAction("Index");
            return RedirectToAction("Index", new { userID = WebSecurity.CurrentUserId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}