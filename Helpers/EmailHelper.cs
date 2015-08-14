using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MealsToGo.Models;
using System.Net.Mail;

namespace MealsToGo.Helpers
{
    public class EmailHelper 
    {



        private ThreeSixtyTwoEntities dbmeals = new ThreeSixtyTwoEntities();
       


        public void SendBulkMail(List<SendEmail> Receivers, string EventType)
        {
            string sender = "kanjasaha@gmail.com";
            string subject = ""; string Body = "";
            string receiver = "";
            SendEmail SendEmail = new SendEmail();
                
            if (EventType == "Insert")
            {
                subject = "Data Has Been Inserted...!!!!";
                Body = "The Record has been inserted to the Data Base with selected Table to be watched";

            }
            else if (EventType == "Update")
            {
                subject = "Data Has Been Update...!!!!";
                Body = "The Record has been Updated to the Table with selected Table to be watched";

            }
            SendEmail.SenderEmailAddress = sender;
            SendEmail.Status = "Success";
            SendEmail.SendDate = DateTime.Now.Date;
            SendEmail.Subject = subject;
            SendEmail.Body = Body;
            SendEmail.DeliveryTime = DateTime.Now;
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(sender);
                for (int i = 0; i < Receivers.Count; i++)
                {
                   // receiver = Convert.ToString(Receivers[i].ReceiverID);
                    SendEmail.RecipientEmailAddress = receiver;
                    SendEmail.DeliveryMethod = "Test";
                    mail.Bcc.Add(receiver);
                    mail.Subject = subject;
                    mail.Body = Body;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("kanjasaha@gmail.com", "Debanjan71");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                    InsertEmailStatus(SendEmail);
                }


            }
            catch (Exception ex)
            {

            }

        }


        //public void SendBulkMailWithFreq(List<SendEmail> Receivers, string EventType)
        //{
        //    string sender = "prasanth04it@gmail.com";
        //    string subject = ""; string Body = "";
        //    string receiver = "";
        //    SendEmail SendEmail = new SendEmail();

        //    if (EventType == "Insert")
        //    {
        //        subject = "Freq-Data Has Been Inserted...!!!!";
        //        Body = "Freq-The Record has been inserted to the Data Base with selected Table to be watched";

        //    }
        //    else if (EventType == "Update")
        //    {
        //        subject = "Freq-Data Has Been Update...!!!!";
        //        Body = "Freq-The Record has been Updated to the Table with selected Table to be watched";

        //    }
        //    SendEmail.SenderEmailAddress = sender;
        //    SendEmail.Status = "";
        //    SendEmail.SendDate = DateTime.Now.Date;
        //    SendEmail.Subject = subject;
        //    SendEmail.Body = Body;
        //    SendEmail.DeliveryTime = DateTime.Now;
        //    try
        //    {
        //        InsertEmailWithFreq(SendEmail);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}

        public List<SendEmail> getUserRecords(string sender)
        {
            List<SendEmail> lstUserRecords = new List<SendEmail>();
            try
            {
                lstUserRecords = (from a in dbmeals.EmailSubscriptions
                                  where a.Sender == sender
                                  select new SendEmail
                                  {
                                       RecipientEmailAddress=a.Recipient,
                                       SenderEmailAddress=a.Sender,
                                      

                                  }).ToList();

                return lstUserRecords;
            }
            catch (Exception ex)
            {

                return lstUserRecords;
            }
        }

        //public List<SendEmail> getUserRecordsWithFrquency(string sender)
        //{
        //    List<SendEmail> lstUserRecords = new List<SendEmail>();
        //    try
        //    {
        //        lstUserRecords = (from a in dbmeals.EmailSubscriptions
        //                          join b in dbmeals.UserSettings on a.EmailSubscriptionID equals b.UserID
        //                          where a.Sender == sender
        //                          select new SendEmail
        //                          {
        //                                SenderEmailAddress=a.UserID,
        //                              ReceiverID = a.Recipient,
        //                                = b.NotificationFrequency


        //                          }).ToList();

        //        return lstUserRecords;
        //    }
        //    catch (Exception ex)
        //    {

        //        return lstUserRecords;
        //    }
        //}

        public void SendEmail(string eventtype)
        {
            List<SendEmail> lstUserRecords = new List<SendEmail>();
            List<SendEmail> lstUserRecordsFrq = new List<SendEmail>();
            lstUserRecords = getUserRecords("1");
           // lstUserRecordsFrq = getUserRecordsWithFrquency("1");
            if (lstUserRecords.Count > 0)
            {
                SendBulkMail(lstUserRecords, eventtype);
                // return true;
            }
            //if (lstUserRecordsFrq.Count > 0)
            //{
            //    SendBulkMailWithFreq(lstUserRecords, eventtype);
            //}
            //  return false;
            
        }

        public void InsertEmailStatus(SendEmail objmodel)
        {
            int intResult = 0;

            try
            {

                SendEmail objTblUser = new SendEmail();
                objTblUser.SenderEmailAddress = objmodel.SenderEmailAddress;
                objTblUser.RecipientEmailAddress = objmodel.RecipientEmailAddress;
                objTblUser.Subject = objmodel.Subject;
                objTblUser.Body = objmodel.Body;
                objTblUser.DeliveryMethod = objmodel.DeliveryMethod;
                objTblUser.DeliveryTime = objmodel.DeliveryTime;
                objTblUser.Status = objmodel.Status;
                objTblUser.SendDate = objmodel.SendDate;             
                dbmeals.SendEmails.Add(objTblUser);              
                intResult = dbmeals.SaveChanges();

            }
            catch (Exception ex)
            {


            }
        }

        //public void InsertEmailWithFreq(SendEmail objmodel)
        //{
        //    int intResult = 0;

        //    try
        //    {

        //        SendEmail objTblUser = new SendEmail();
        //        objTblUser.SenderEmailAddress = objmodel.SenderEmailAddress;
        //        objTblUser.RecipientEmailAddress = objmodel.RecipientEmailAddress;
        //        objTblUser.RecipientEmailAddress = "prasanth04it@gmail.com";
        //        objTblUser.Subject = objmodel.Subject;
        //        objTblUser.Body = objmodel.Body;
        //        objTblUser.DeliveryMethod = objmodel.DeliveryMethod;
        //       // objTblUser.DeliveryTime = objmodel.DeliveryTime;
        //        objTblUser.Status = objmodel.Status;
        //        //objTblUser.SendDate = objmodel.SendDate;
        //        objTblUser.NotificationTime = objmodel.NotificationTime;
        //        objTblUser.NotificationTime = "1";
        //        objTblUser.UserID = 1;
        //        dbcontext.SendEmails.Add(objTblUser);
        //        intResult = dbcontext.SaveChanges();

        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}

    }
}