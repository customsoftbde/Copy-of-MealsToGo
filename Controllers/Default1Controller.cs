using System.Web.Mvc;
using MealsToGo.Repository;
using MealsToGo.Models;

namespace RepostioryExampleMVC.Controllers
{
    //public class CustomerController : Controller
    //{
    //    private readonly NorthwindEntities _context;
    //    private readonly IRepository<Customer> _reporsitory;
    //    //private readonly IRepository<Order> _reporsitoryOrders;

    //    public CustomerController()
    //    {
    //        _context = new NorthwindEntities();
    //        _reporsitory = new Repository<Customer>(_context, false);
    //        //_reporsitoryOrders = new Repository<Order>(_context, false);
    //    }

    //    public ActionResult Index()
    //    {
    //        var customers = _reporsitory.GetAll();
    //        return View(customers);
    //    }

    //    //public ActionResult Index(int page, int pageSize)
    //    //{
    //    //    var customers = _reporsitory.GetAllPaged(page, pageSize);
    //    //    return View(customers);
    //    //}

    //    //http://www.mydomain.com/Customer/orders/ALFKI
    //    //public ActionResult Orders(string id)
    //    //{
    //    //    var orders = _reporsitoryOrders.Find(o => o.CustomerID == id);
    //    //    return View(orders);
    //    //}      

    //    //http://www.mydomain.com/Customer/Details/ALFKI
    //    public ActionResult Details(string id)
    //    {
    //        var customer = _reporsitory.Single(c => c.CustomerID == id);
    //        return View(customer);
    //    }

    //    //http://www.mydomain.com/Customer/Create
    //    public ActionResult Create()
    //    {
    //        return View();
    //    }

    //    [HttpPost]
    //    public ActionResult Create(Customer customer)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _reporsitory.Add(customer);
    //            _reporsitory.SaveChanges();
    //            return RedirectToAction("Index");
    //        }
    //        ModelState.AddModelError("", "Error Message");
    //        return View();
    //    }

    //    //http://www.mydomain.com/Customer/Edit/ALFKI
    //    public ActionResult Edit(string id)
    //    {
    //        var customer = _reporsitory.Single(c => c.CustomerID == id);
    //        return View(customer);
    //    }

    //    [HttpPost]
    //    public ActionResult Edit(string id, Customer customer)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _reporsitory.Edit(customer);
    //            _reporsitory.SaveChanges();
    //            return RedirectToAction("Index");
    //        }
    //        ModelState.AddModelError("", "Error Message");
    //        return View();
    //    }

    //    //http://www.mydomain.com/Customer/Delete/ALFKI
    //    public ActionResult Delete(string id)
    //    {
    //        var customer = _reporsitory.Single(c => c.CustomerID == id);
    //        return View(customer);
    //    }

    //    [HttpPost]
    //    public ActionResult Delete(string id, FormCollection collection)
    //    {
    //        var customer = _reporsitory.Single(c => c.CustomerID == id);
    //        if (ModelState.IsValid)
    //        {
    //            _reporsitory.Delete(customer);
    //            //_reporsitory.DeleteRelatedEntities(customer);
    //            _reporsitory.SaveChanges();
    //            return RedirectToAction("Index");
    //        }
    //        ModelState.AddModelError("", "Error Message");
    //        return View();
    //    }

    //    //public string Count(Customer customer)
    //    //{
    //    //    return _reporsitory.Count().ToString();
    //    //}
    //}
}
