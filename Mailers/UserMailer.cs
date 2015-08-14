using Mvc.Mailer;
using System.Net.Mail;
using System.Collections.Generic;
namespace MealsToGo.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}

       
        public virtual MvcMailMessage Welcome(MailAddress fromemail, List<MailAddress> toemails, string EmailBody)
        {
            var mailMessage = new MvcMailMessage();


            foreach (var emailaddress in toemails)
            {
                mailMessage.To.Add(emailaddress);
            }
            mailMessage.From = fromemail;
            mailMessage.Subject = "Welcome to Fun Fooding";
            mailMessage.Body = EmailBody;
            mailMessage.IsBodyHtml = true;
         //   PopulateBody(mailMessage, viewName: "Welcome");

            return mailMessage;
        }


        public virtual MvcMailMessage Connect(MailAddress fromemail, List<MailAddress> toemails, string EmailBody)
        {
            var mailMessage = new MvcMailMessage();

            mailMessage.To.Add(fromemail);
            mailMessage.Subject = "Connect with me at Funfooding";
            mailMessage.Body = EmailBody;
            PopulateBody(mailMessage, viewName: "Welcome");

            return mailMessage;
        }


 
       
 	}
}