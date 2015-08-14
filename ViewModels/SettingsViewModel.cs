using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MealsToGo.ViewModels
{
    public class UserSettingsViewModel
    {

        public int UserID { get; set; }
        public int UserSettingsID { get; set; }
        public int ActivityTypeID { get; set; }
        public string ActivityType { get; set; }
        [Required]
        public int PrivacySettingID { get; set; }
        public string PrivacySetting { get; set; }
        [Required]
        public int ReceiveEmailNotificationID { get; set; }
        [Required]
        public int ReceiveMobileTextNotificationID { get; set; }
                
        public string ReceiveEmailNotification { get; set; }
        public string ReceiveMobileTextNotification { get; set; }
        public string NotificationFrequency { get; set; }
        [Required]
        public string NotificationFrequencyID { get; set; }
        public IEnumerable<SelectListItem> NotificationFrequencyList { get; set; }
        public IEnumerable<SelectListItem> PrivacySettingList { get; set; }
        public IEnumerable<SelectListItem> YesNo
        {
            get
            {
                return new[]
            {
                new SelectListItem { Value = "1", Text = "Yes" },
                new SelectListItem { Value = "0", Text = "No" },
                
            };
            }
        }
        

    }
}


