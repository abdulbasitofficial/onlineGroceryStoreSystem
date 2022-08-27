using Online_Grocery_Order.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Online_Grocery_Order.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(Category c)
        {
            if (ModelState.IsValid)
            {
                Category cat = new Category();
                cat.insert_Category(c);

                return RedirectToAction("Category");
            }
            else
            {
                return View("AddCategory");
            }
        }
        [HttpGet]
        [AllowAnonymous]
      
        public ActionResult Category(int page =1)
        {
            Category cat = new Category();
            List<Category> files = cat.dispaly_Category();
            ViewBag.TotalPages = (files.Count() / 20) + 1;
            files = files.Skip((page - 1) * 20).Take(20).ToList();
            return View(files);
        }
        [HttpGet]
        [AllowAnonymous]
       
        public ActionResult Delete(int id)
        {
            Category cat = new Category();
            cat.delete_Category(id);
            return RedirectToAction("Category");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Category cat = new Category();
            Category c = cat.upadate_Category(id);
            return View(c);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category c)
        {
            Category cat = new Category();
            cat.upadate_Category(c);
            return RedirectToAction("Category");
        }
    }
}