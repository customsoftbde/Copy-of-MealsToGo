using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealsToGo.Models;
using System.IO;
using MealsToGo.ViewModels;
using AutoMapper;
using MealsToGo.Service;
using WebMatrix.WebData;

namespace MealsToGo.Controllers
{
    public class ContactController : Controller
    {
         private ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();
        private readonly IContactService _service;
       
        public ContactController(IContactService service)
        {
            _service = service;
        }

        // GET: /Contact/

        public ActionResult Index(int UserID)
        {
            
            IEnumerable<Contact> contacts = _service.FindByUser(UserID);

            return View(contacts);
        }

        //
        // GET: /Contact/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Contact/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Contact/Create

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
        // GET: /Contact/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Contact/Edit/5

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
        // GET: /Contact/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Contact/Delete/5

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
