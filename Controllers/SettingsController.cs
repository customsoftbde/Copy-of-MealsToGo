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
using WebMatrix.WebData;

namespace MealsToGo.Controllers
{
    public class SettingsController : Controller
    {
       private ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();
        
        //
        // GET: /UserSetting/

        
         private readonly IUserSettingService _service;


         public SettingsController(IUserSettingService service)
         {
            _service = service;
         }

         public ActionResult Index(int userid)
         {

             // IEnumerable<MealAd> mealads = db.MealAds.Where(x=>x.MealItem.UserId == userid);
           //  ViewBag.MealItemCount = db.MealItems.Where(x => x.UserId == userid).Count();
             //ViewBag.UserID = userid;
             //return View(mealads.ToList());



             IEnumerable<UserSetting> usersetting = _service.FindByUser(userid);
             IEnumerable<UserSettingsViewModel> usvm = Mapper.Map<IEnumerable<UserSetting>, IEnumerable<UserSettingsViewModel>>(usersetting);
             
             //IEnumerable<LKUPDeliveryMethod> DeliveryMethodList = DeliveryMethods.ToList();
             //mo.DeliveryMethodDDList =
             //    DeliveryMethodList.Select(x => new SelectListItem
             //    {
             //        Value = x.DeliveryMethodID.ToString(),
             //        Text = x.DeliveryMethod
             //    });
             //return View(mo);

             //UserSettingsViewModel usvm = new UserSettingsViewModel();
             //usvm = Mapper.Map<UserSetting, UserSettingsViewModel>(usersetting);

             

             return View(usvm);
         }
        //
        // GET: /Settings/Details/5

        public ActionResult Details(int id = 0)
        {
            UserSetting usersetting = db.UserSettings.Find(id);
            if (usersetting == null)
            {
                return HttpNotFound();
            }
            return View(usersetting);
        }

        //
        // GET: /Settings/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Settings/Create

        [HttpPost]
        public ActionResult Create(UserSetting usersetting)
        {
            if (ModelState.IsValid)
            {
                db.UserSettings.Add(usersetting);
                db.SaveChanges();
                return RedirectToAction("Index", new { userID = WebSecurity.CurrentUserId });
            }

            return View(usersetting);
        }
        
        public ActionResult Confirm(int userid)
        {


            List<UserSetting> usersettings = db.UserSettings.Where(x=>x.UserID == userid).ToList();
            foreach ( UserSetting us in usersettings)
     
            {
                us.Verified = 1;
                db.Entry(us).State = EntityState.Modified;
            
            }
            
          
                
            
                db.SaveChanges();
                return RedirectToAction("Index","MealAd",routeValues: new { userid = userid });
            

           
        }
        //
        // GET: /Settings/Edit/5

        public ActionResult Edit(int usersettingsid)
        {
            UserSetting usersetting = db.UserSettings.Find(usersettingsid);
            if (usersetting == null)
            {
                return HttpNotFound();
            }

            UserSettingsViewModel usvm  = Mapper.Map<UserSetting, UserSettingsViewModel>(usersetting);
            
           
            var PrivacySettingOptions = from b in db.LKUPPrivacySettings select b;

            IEnumerable<LKUPPrivacySetting> PrivacySettings = PrivacySettingOptions.ToList();
            usvm.PrivacySettingList = PrivacySettings.Select(x => new SelectListItem
                                         {
                                        Value = x.PrivacySettingsID.ToString(),
                                        Text = x.PrivacySettings.ToString()
                                        });

            var Notification = from b in db.NotificationFrequencies select b;

            IEnumerable<NotificationFrequency> NotificationFrequencyOptions = Notification.ToList();
            usvm.NotificationFrequencyList = NotificationFrequencyOptions.Select(x => new SelectListItem
            {
                Value = x.NotificationFrequencyID.ToString(),
                Text = x.Description.ToString()
            });

            
            return View(usvm);
            
           
        }

        //
        // POST: /Settings/Edit/5

        [HttpPost]
        public ActionResult Edit(UserSettingsViewModel usvm)
        {

            if (ModelState.IsValid)
            {
                UserSetting usersetting = db.UserSettings.Find(usvm.UserSettingsID);
                usersetting.ReceiveEmailNotification = usvm.ReceiveEmailNotificationID;
                usersetting.ReceiveMobileTextNotification = usvm.ReceiveMobileTextNotificationID;
                usersetting.NotificationFrequencyID = (String.IsNullOrEmpty(usvm.NotificationFrequencyID) ? 0 : Convert.ToInt32(usvm.NotificationFrequencyID));
                usersetting.PrivacySettingsID = usvm.PrivacySettingID;

                UserSetting us = Mapper.Map<UserSettingsViewModel, UserSetting>(usvm);
                db.Entry(usersetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { userID = WebSecurity.CurrentUserId });
            }
            else
            {
                UserSetting usersetting = db.UserSettings.Find(usvm.UserSettingsID);

                UserSettingsViewModel usvm1 = Mapper.Map<UserSetting, UserSettingsViewModel>(usersetting);
                usvm.ActivityType = usvm1.ActivityType;

                var PrivacySettingOptions = from b in db.LKUPPrivacySettings select b;

                IEnumerable<LKUPPrivacySetting> PrivacySettings = PrivacySettingOptions.ToList();
                usvm.PrivacySettingList = PrivacySettings.Select(x => new SelectListItem
                {
                    Value = x.PrivacySettingsID.ToString(),
                    Text = x.PrivacySettings.ToString()
                });

                var Notification = from b in db.NotificationFrequencies select b;

                IEnumerable<NotificationFrequency> NotificationFrequencyOptions = Notification.ToList();
                usvm.NotificationFrequencyList = NotificationFrequencyOptions.Select(x => new SelectListItem
                {
                    Value = x.NotificationFrequencyID.ToString(),
                    Text = x.Description.ToString()
                });


                return View(usvm);
            }

            return View(usvm);
        }

        //
        // GET: /Settings/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserSetting usersetting = db.UserSettings.Find(id);
            if (usersetting == null)
            {
                return HttpNotFound();
            }
            return View(usersetting);
        }

        //
        // POST: /Settings/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserSetting usersetting = db.UserSettings.Find(id);
            db.UserSettings.Remove(usersetting);
            db.SaveChanges();
            return RedirectToAction("Index", new { userID = WebSecurity.CurrentUserId });
        }

         public ActionResult PrivacyRules()
        {
            return View();
         }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        
    }
}