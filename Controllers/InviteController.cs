using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealsToGo.Models;

namespace MealsToGo.Controllers
{
    public class InviteController : Controller
    {
        private UsersContext db = new UsersContext();
        private ThreeSixtyTwoEntities dbmeals = new ThreeSixtyTwoEntities();

        //
        // GET: /Invite/

        public ActionResult Index()
        {
            var tset = dbmeals.SendEmails.ToList();
            List<ContactList> invitees = new List<ContactList>();
            return View(invitees);
        }

        //
        // GET: /Invite/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Invite/Create

        public ActionResult Create()
        {
            return View();
         //   Response.Redirect(User/Invite)
        }

        //
        // POST: /Invite/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Invite/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Invite/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Invite/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Invite/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
