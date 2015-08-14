using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MealsToGo.Models;

namespace MealsToGo.Controllers
{
    public class OrderController : Controller
    {
        private ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();

        //
        // GET: /Order/

        public ActionResult Index(int MealAdid)
        {
            //var Order = db.Orders.Where(s => s.MealAdID == MealAdid).Include(m => m.UserDetail);
            //return View(Order.ToList());
            return View();
        }

        //
        // GET: /Order/Details/5

        public ActionResult Details(int id = 0)
        {
            Order Order = db.Orders.Find(id);
            if (Order == null)
            {
                return HttpNotFound();
            }
            return View(Order);
        }

        //
        // GET: /Order/Create

        public ActionResult Create(int Orderid)
        {
            Order Order = new Order();
            Order.OrderID = Orderid;
            return View(Order);
        }

        //
        // POST: /Order/Create

        [HttpPost]
        public ActionResult Create(Order Order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(Order);
                db.SaveChanges();
                return RedirectToAction("Details", "Order", new { id = Order.OrderID });
            }

            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", Order.UserId);
            return View(Order);
        }

        //
        // GET: /Order/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Order Order = db.Orders.Find(id);
            if (Order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", Order.UserId);
            return View(Order);
        }

        //
        // POST: /Order/Edit/5

        [HttpPost]
        public ActionResult Edit(Order Order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", Order.UserId);
            return View(Order);
        }

        //
        // GET: /Order/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Order Order = db.Orders.Find(id);
            if (Order == null)
            {
                return HttpNotFound();
            }
            return View(Order);
        }

        //
        // POST: /Order/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Order Order = db.Orders.Find(id);
            db.Orders.Remove(Order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}