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
    public class MealItemController : Controller
    {
        private ThreeSixtyTwoEntities db = new ThreeSixtyTwoEntities();
        private readonly IMealItemService _service;


        public MealItemController(IMealItemService service)
        {
            _service = service;
        }



        // GET: /MealItem/

        public ActionResult Index(int UserID)
        {

            IEnumerable<MealItem> mt = _service.FindByUser(UserID);
            IEnumerable<MealItemViewModel> mtvm = Mapper.Map<IEnumerable<MealItem>, IEnumerable<MealItemViewModel>>(mt);
            //mtvm = PopulateDropDown(mtvm);

            return View(mt);
        }

        //
        // GET: /MealItem/Details/5

        public ActionResult Details(int id)
        {
            MealItem mealitem = _service.GetById(id);
            if (mealitem == null)
            {
                mealitem = new MealItem();
            }

            MealItemViewModel mtvm = Mapper.Map<MealItem, MealItemViewModel>(mealitem);
            mtvm = PopulateDropDown(mtvm, mealitem);
            ViewData["MealItemViewModel"] = mtvm;
            //MealItemViewModel mtvm = Mapper.Map<MealItem, MealItemViewModel>(mealitem);
            //if (mtvm.MealTypeDD == null)
            //    mtvm.MealTypeDD = new MealTypeDDListViewModel();
            //mtvm.MealTypeDD.SelectedMealType = _service.MealTypeDDList().Where(x=>x.MealTypeID==mtvm.MealTypeDD.;
            return View(mealitem);
        }

        //
        // GET: /MealItem/Create

        public ActionResult Create()
        {

            MealItem mt = new MealItem();
            mt.UserId = WebSecurity.CurrentUserId;

            MealItemViewModel mtvm = Mapper.Map<MealItem, MealItemViewModel>(mt);

            mtvm = PopulateDropDown(mtvm, mt);

            return View(mtvm);
        }

        private MealItemViewModel PopulateDropDown(MealItemViewModel mtvm, MealItem mealitem)
        {
            if (mtvm == null)
                mtvm = new MealItemViewModel();
            mtvm.ServingUnitDDList = _service.GetServingUnitDDList().Select(x => new SelectListItem
            {
                Value = x.ServingUnitID.ToString(),
                Text = x.ServingUnit,
                Selected = (mealitem != null && mealitem.ServingUnit == x.ServingUnitID)
            });

            mtvm.MealTypeDD.MealTypeDDList = _service.MealTypeDDList().Select(x => new SelectListItem
            {
                Value = x.MealTypeID.ToString(),
                Text = x.Name,
                Selected = (mealitem != null && mealitem.MealTypeID == x.MealTypeID)
            });
            mtvm.CusineTypeDD.CuisineDDList = _service.CuisineTypeDDList().Select(x => new SelectListItem
            {
                Value = x.CuisineTypeID.ToString(),
                Text = x.Name,
                Selected = (mealitem != null && mealitem.CusineTypeID == x.CuisineTypeID)
            });

            mtvm.DietTypeDD.DietTypeDDList = _service.DietTypeDDList().Select(x => new SelectListItem
            {
                Value = x.DietTypeID.ToString(),
                Text = x.Name,
                Selected = (mealitem != null && mealitem.DietTypeID == x.DietTypeID)
            });
            mtvm.AllergenDD = _service.AllergenicFoodsDDList().Select(x => new Allergen
            {
                AllergenName = x.AllergenicFood,
                AllergenID = x.AllergenicFoodID,
                Selected = (mealitem != null && mealitem.MealItems_AllergenicFoods.Where(y => y.AllergenicFoodID == x.AllergenicFoodID ).Count() > 0)
            }).ToList();

            return mtvm;
        }
        //
        // POST: /MealItem/Create

        [HttpPost]
        public ActionResult Create(MealItemViewModel mtvms, HttpPostedFileBase[] Photos)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    MealItem mealitem = Mapper.Map<MealItemViewModel, MealItem>(mtvms);

                    foreach (var fileBase in Photos)
                    {
                        if (fileBase != null)
                        {

                            if (fileBase.ContentLength > 0)
                            {
                                var path = Path.Combine(Server.MapPath("~/MealPhotos"), WebSecurity.CurrentUserId + '-' + Path.GetRandomFileName().Replace(".", "").Substring(0, 8) + '-' + Path.GetFileName(fileBase.FileName));


                                fileBase.SaveAs(path);
                                MealItems_Photos mp = new MealItems_Photos();
                                mp.Photo = path;
                                mealitem.MealItems_Photos.Add(mp);
                            }
                        }

                    }


                    foreach (var mealaller in mtvms.AllergenDD)
                    {
                        if (mealaller.Selected)
                        {

                            MealItems_AllergenicFoods aller = new MealItems_AllergenicFoods();
                            aller.AllergenicFoodID = mealaller.AllergenID;
                            mealitem.MealItems_AllergenicFoods.Add(aller);
                        }

                    }





                    mealitem.MealItemId = _service.AddAndReturnID(mealitem);

                    return RedirectToAction("Details", "MealItem", new { id = mealitem.MealItemId });

                }
            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes." + ex.Message.ToString());
            }
            mtvms = PopulateDropDown(mtvms, null);
            return View(mtvms);
        }



        public bool RemovePhoto(int id = 0)
        {
            var mealItems_Photos=db.MealItems_Photos.FirstOrDefault(x => x.MealItemPhotoID == id);
            if (mealItems_Photos != null)
            {
                db.MealItems_Photos.Remove(mealItems_Photos);
                db.SaveChanges();
            }
            return true;
        }
        //
        // GET: /MealItem/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MealItem mealitem = _service.GetById(id);
            MealItemViewModel mtvm = Mapper.Map<MealItem, MealItemViewModel>(mealitem);

            mtvm = PopulateDropDown(mtvm, mealitem);
            ViewData["MealItemViewModel"] = mealitem;
            return View(mtvm);
          
            //if (mealitem == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.CusineTypeID = new SelectList(db.LKUPCuisineTypes, "CuisineTypeID", "Name", mealitem.CusineTypeID);
            //ViewBag.MealTypeID = new SelectList(db.LKUPCuisineTypes, "MealTypeID", "Name", mealitem.MealTypeID);
            //ViewBag.UserId = new SelectList(db.UserDetails, "UserId", "FirstName", mealitem.UserId);
            // return View(mealitem);
        }

        //
        // POST: /MealItem/Edit/5

        [HttpPost]
        public ActionResult Edit(MealItemViewModel mtvms, HttpPostedFileBase[] Photos)
        {
            if (ModelState.IsValid)
            {
                MealItem mealitem132 = Mapper.Map<MealItemViewModel, MealItem>(mtvms);
                MealItem mealitem = db.MealItems.Where(x => x.MealItemId == mtvms.MealItemId).FirstOrDefault();
                mealitem.MealItemName = mtvms.MealItemName;
                mealitem.Ingredients = mtvms.Ingredients;
                mealitem.DietTypeID = mealitem132.DietTypeID;
                mealitem.MealTypeID = mealitem132.MealTypeID;
                mealitem.CusineTypeID = mealitem132.CusineTypeID;
                mealitem.ServingUnit = mealitem132.ServingUnit;
                mealitem.Quantity = mtvms.Quantity;
                mealitem.Status = mtvms.Status;
                mealitem.Price = mtvms.Price;
               
                List<MealItems_AllergenicFoods> lstMealItems_AllergenicFoods = new List<MealItems_AllergenicFoods>();
                foreach (var allergenic in mealitem.MealItems_AllergenicFoods)
                    lstMealItems_AllergenicFoods.Add(allergenic);
                foreach (var allergenic in lstMealItems_AllergenicFoods)
                {
                    //mealitem.MealItems_AllergenicFoods.Remove(allergenic);

                    db.MealItems_AllergenicFoods.Remove(allergenic);

                    db.Entry(mealitem).State = EntityState.Modified;
                    db.SaveChanges();
                }

                //db.SaveChanges();
                foreach (var allergenic in mtvms.AllergenDD.Where(x => x.Selected))
                {
                    mealitem.MealItems_AllergenicFoods.Add(new MealItems_AllergenicFoods() { AllergenicFoodID = allergenic.AllergenID, MealItemID = mealitem.MealItemId });

                }

                if (Photos != null)
                {
                    foreach (var fileBase in Photos)
                    {
                        if (fileBase != null)
                        {
                            List<MealItems_Photos> lstMealItems_Photos = new List<MealItems_Photos>();
                            foreach (var photo in mealitem.MealItems_Photos)
                            {
                                lstMealItems_Photos.Add(photo);
                            }
                            //foreach (var photo in lstMealItems_Photos)
                            //{
                            //    db.MealItems_Photos.Remove(photo);

                            //    db.Entry(mealitem).State = EntityState.Modified;
                            //    db.SaveChanges();
                            //}
                            if (fileBase.ContentLength > 0)
                            {
                                var path = Path.Combine(Server.MapPath("~/MealPhotos"), WebSecurity.CurrentUserId + '-' + Path.GetRandomFileName().Replace(".", "").Substring(0, 8) + '-' + Path.GetFileName(fileBase.FileName));


                                fileBase.SaveAs(path);
                                MealItems_Photos mp = new MealItems_Photos();
                                mp.Photo = path;
                                mealitem.MealItems_Photos.Add(mp);
                            }
                        }

                    }
                }

                db.Entry(mealitem).State = EntityState.Modified;
                db.SaveChanges();
                //MealItem mealitem = Mapper.Map<MealItemViewModel, MealItem>(mtvms);
                //_service.Update(mealitem);
                return RedirectToAction("Index", new { userID = WebSecurity.CurrentUserId });
            }
            else
            {
                //MealItem mealitem = _service.GetById(id);
                //MealItemViewModel mtvm = Mapper.Map<MealItem, MealItemViewModel>(mealitem);

                mtvms = PopulateDropDown(mtvms, null);
                return View(mtvms);
            }
            //return View(mtvms);
        }

        //
        // GET: /MealItem/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MealItem mealitem = _service.GetById(id);
            if (mealitem == null)
            {
                return HttpNotFound();
            }
            return View(mealitem);
        }

        //
        // POST: /MealItem/Delete/5

        [HttpPost, ActionName("Delete")]
        public bool DeleteConfirmed(int id)
        {
            //MealItem mealitem = _service.GetById(id);

            MealItem mealitem =db.MealItems.Where(a => a.MealItemId == id).FirstOrDefault();
            if (mealitem != null)
            {
                foreach (var allergenic in db.MealItems_AllergenicFoods.Where(x => x.MealItemID == id))
                {
                    db.MealItems_AllergenicFoods.Remove(allergenic);
                }
                db.SaveChanges();

                //db.Entry(mealitem).State = EntityState.Modified;
                //db.Entry(mealitem).State = EntityState.Deleted;
                db.MealItems.Remove(mealitem);
                
                db.SaveChanges();
            }
            return true;
        }


    }
}