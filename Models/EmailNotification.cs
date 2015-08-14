using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MealsToGo.Models
{
    public class EmailNotification
    {

         
        public int UserTableID { get; set; }

        public int? UserID { get; set; }

        public string ReceiverID { get; set; }

        public string SenderEmailAddress { get; set; }

        public int? Frequecy { get; set; }
        public string receiverEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string DeliveryMethod { get; set; }
        public DateTime DeliveryTime { get; set; }

        public string RecipientEmailAddress { get; set; }
        public string Status { get; set; }
        public DateTime SendDate { get; set; }
        public string NotificationTime { get; set; }

        public int Activity { get; set; }
    }
    }


