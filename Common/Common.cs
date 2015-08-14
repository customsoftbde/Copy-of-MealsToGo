using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using GeoCoding;
using MealsToGo.Models;
using MealsToGo.Helpers;
using System.Net.Mail;
using System.Transactions;
using System.IO;


namespace MealsToGo
{
    public static class Common
    {


        public static bool sendeMail(EmailModel emailmodel, bool EmailExist)
        {
            try
            {
                MailMessage mail = new MailMessage();
                string receiver = emailmodel.To;
                //string sender = "kanjasaha@gmail.com";
                string sender = "qat2015team@gmail.com";
                //string verifyUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Home/ValidateRequest/?id=" + 4;
                //string Body = verifyUrl;
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(sender);
                mail.To.Add(receiver);
                mail.Bcc.Add(sender);
                mail.Subject = emailmodel.Subject;
                mail.Body = emailmodel.EmailBody;
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("kanjasaha@gmail.com", "Debabrata71");
                SmtpServer.Credentials = new System.Net.NetworkCredential("qat2015team@gmail.com", "q$7@wt%j*65ba#3M@9P6");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool sendMail(EmailModel emailmodel, bool EmailExist)
        {
            try
            {
                MailMessage mail = new MailMessage();
                string receiver = emailmodel.To;
                string sender = emailmodel.From;
                string verifyUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Home/ValidateRequest/?id=" + 4;
                string Body = verifyUrl;
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(sender);
                mail.To.Add(receiver);
                mail.Bcc.Add(sender);
                mail.Subject = "Request";
                mail.Body = Body;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("kanjasaha@gmail.com", "Debabrata71");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

       
        public static DateTime AbsoluteStart(this DateTime dateTime)
        {
            
            return dateTime.Date;
        }

        /// <summary>
        /// Gets the 11:59:59 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteEnd(this DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        }

        public static string GetFullAddress(MealsToGo.Models.AddressList address)
        {
            List<string> fulladdress = new List<string>();

            if ((address.Address1 != null) && (address.Address1 != ""))
                fulladdress.Add(address.Address1);

            if ((address.Address2 != null) && (address.Address2 != ""))
                fulladdress.Add(address.Address1);

            if ((address.City != null) && (address.City != ""))
                fulladdress.Add(address.City);

            if ((address.Province != null) && (address.Province != ""))
                fulladdress.Add(address.Province);

            if ((address.Zip != null) && (address.Zip != ""))
                fulladdress.Add(address.Zip);

            if ((address.LKUPCountry.Country != null) && (address.LKUPCountry.Country != ""))
                fulladdress.Add(address.LKUPCountry.Country);

            return string.Join(",", fulladdress.ToArray());



        }
        
        public static IEnumerable<IEnumerable<T>> Partition<T>
   (this IEnumerable<T> source, int size)
        {
            T[] array = null;
            int count = 0;
            foreach (T item in source)
            {
                if (array == null)
                {
                    array = new T[size];
                }
                array[count] = item;
                count++;
                if (count == size)
                {
                    yield return new ReadOnlyCollection<T>(array);
                    array = null;
                    count = 0;
                }
            }
            if (array != null)
            {
                Array.Resize(ref array, count);
                yield return new ReadOnlyCollection<T>(array);
            }
        }


      

    }


}