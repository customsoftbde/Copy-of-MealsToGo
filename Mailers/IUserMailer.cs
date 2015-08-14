using Mvc.Mailer;
using System.Net.Mail;
using System.Collections.Generic;

namespace MealsToGo.Mailers
{ 
    public interface IUserMailer
    {
        MvcMailMessage Welcome(MailAddress fromemail, List<MailAddress> toemails, string EmailBody);
        MvcMailMessage Connect(MailAddress fromemail, List<MailAddress> toemails, string EmailBody);
		//	MvcMailMessage GoodBye();
	}
}