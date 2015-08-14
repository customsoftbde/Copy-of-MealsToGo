using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MealsToGo.Models;
using MealsToGo.Repository;
using System.Linq.Expressions;
using System.IO;


namespace MealsToGo.Service
{
    public class ContactService : IContactService
    {
        //  private readonly UserSettingRepository UserSettingrepos;
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ContactService()
        {
        }

        public IEnumerable<Contact> FindByUser(long userid)
        {

            Contact ct = new Contact();
            List<Contact> ctlist=new List<Contact>();
            int count=10;

            while (count >= 0)
            {
                ct.EmailContact = "kanjasaha@gmail.com";
                ct.CuisineType = "Indian";
                ct.KitchenName = "kanja kitchen";
                ctlist.Add(ct);
                count = count - 1;
            }
            return ctlist;
        }

        public Contact GetById(long Id)
        {
            throw new NotImplementedException();


        }

        public IEnumerable<Contact> GetAll()
        {
            throw new NotImplementedException();


        }

        public IEnumerable<Contact> Find(Expression<Func<Contact, bool>> predicate)
        {
            throw new NotImplementedException();



        }






        public void Add(Contact UserSetting)
        {

            throw new NotImplementedException();

        }







        public void Delete(Contact mt)
        {
            throw new NotImplementedException();
        }


        public void Update(Contact mt)
        {
            throw new NotImplementedException();
        }

        //protected override void Dispose(bool disposing)
        //{
        //    unitOfWork.Dispose();
        //    base.Dispose(disposing);
        //}

    }
}