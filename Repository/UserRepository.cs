using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MealsToGo.Models;

namespace MealsToGo.Repository
{
    public class UserRepository : IUserRepository
    {
     
        private ThreeSixtyTwoEntities dbmeals = new ThreeSixtyTwoEntities();
        private UsersContext db = new UsersContext();

        public UserRepository(ThreeSixtyTwoEntities dbmeals)
        {
            this.dbmeals = dbmeals;
        }

        public IEnumerable<UserDetail> GetUsers()
        {
            return dbmeals.UserDetails.ToList();
        }

        public UserDetail GetUserByID(int userId)
        {
            return dbmeals.UserDetails.Find(userId);
        }

        public int GetUserIDByEmail(string username)
        {
            return db.UserProfiles.Find(username).UserId;
        }

        public void InsertContactList(ContactList newcontact)
        {
            int? RecipientID = GetUserIDByEmail(newcontact.RecipientEmailAddress);
            newcontact.RecipientUserID = RecipientID;

            dbmeals.ContactLists.Add(newcontact);
        }

        public void InsertUser(UserDetail user)
        {
            dbmeals.UserDetails.Add(user);
        }

        public void DeleteUser(int userId)
        {
            UserDetail user = dbmeals.UserDetails.Find(userId);
            dbmeals.UserDetails.Remove(user);
        }

        public void UpdateUser(UserDetail user)
        {
            dbmeals.Entry(user).State = EntityState.Modified;
        }

        public void Save()
        {
            dbmeals.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbmeals.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}