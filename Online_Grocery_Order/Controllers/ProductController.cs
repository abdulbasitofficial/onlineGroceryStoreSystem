using Online_Grocery_Order.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web;
using System;
using System.Web.SessionState;
using System.Linq;

namespace Online_Grocery_Order.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class ProductController : Controller
    {
        public object PostedFile { get; private set; }
        // GET: Product
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Product(int page=1)
        {
            try
            {
                Product pro = new Product();
                List<Product> files = pro.dispaly_Product();
                ViewBag.TotalPages = (files.Count() / 40) + 1;
                files = files.Skip((page - 1) * 40).Take(40).ToList();
                return View(files);
            }
            catch (Exception e)
            {
                return View(e);
            }
            
        }
        [HttpGet]
        [AllowAnonymous]
       
        public ActionResult AddProduct()
        {
            try
            {
                Product pro = new Product();
                pro.category = pro.CategoryDropDownList();
                return View(pro);
            }
            catch (Exception e)
            {
                return View(e);
            }
           
        }
       
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(Product p, HttpPostedFileBase PostedFile)
        {
            try
            {
                Product pro = new Product();
                pro.insert_Product(p, PostedFile);
                return RedirectToAction("Product");
            }
            catch (Exception e)
            {
                return View(e);
            }
          
          
        }
        [HttpGet]
        
        public ActionResult Delete(int id)
        {
            try
            {
                Product pro = new Product();
                pro.delete_Product(id);

                return RedirectToAction("Product");
            }
            catch (Exception e)
            {
                return View(e);
            }
            
        }


        [HttpGet]
        public ActionResult Offer(int id)
        {
            try
            {
                Cart car = new Cart();
                Session["cart_count"] = car.count_Item();

                Product pro = new Product();
                List<Product> files= pro.Offer_Product(id);
                Category cat = new Category();
                ViewBag.category = cat.CategoryDropDownList();
               
                return View(files);
            }
            catch (Exception e)
            {
                return View(e);
            }
        }

        [HttpGet]
        public ActionResult AddCart(int id)
        {

            if (Session["email"] != null)
            {
                int uid = (int)Session["id"];
                Cart c = new Cart();
                cart item = c.check_Cart(id, uid);
                if (item != null)
                {
                    return RedirectToAction("Offer");
                }
                else
                {
                    c.add_Cart(id, uid);
                    return RedirectToAction("Offer");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        [HttpGet]
        [AllowAnonymous]
        
        public ActionResult Edit(int id)
        {
            try
            {
                Product pro = new Product();
                Product p = pro.upadate_Product(id);
                return View(p);
            }
            catch (Exception e)
            {
                return View(e);
            }
           
        }
       // Product ID
       [HttpPost]
        public ActionResult Attach(string  id)
        {
            try
            {
                int ii = int.Parse(id);
                Product pro = new Product();
                //pro.Attach_Product(ii);
                return RedirectToAction("Product");
            }
            catch (Exception e)
            {
                return View(e);
            }
        }
       
        [HttpGet]
        public ActionResult Attach_Now(int id)
        {
          
                Product pro = new Product();
                Product file = pro.single_Product(id);
                ViewBag.Product_list = product_List();
                return View(file);
           
        }
        [HttpPost]
        public ActionResult Attach_Now( string attach, string pid)
        {
            try
            {
                int attch = int.Parse(attach);
                int pro_id = int.Parse(pid);

                Product pro = new Product();
                pro.Attach_Product(attch, pro_id);
                return RedirectToAction("Product");
            }
            catch (Exception e)
            {
                return View(e);
            }
        }
        public List<SelectListItem> product_List()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Product pp = new Product();
            List<Product> file = pp.dispaly_Product();
            foreach(var item in file)
            {
                list.Add(new SelectListItem { Value=item.Product_ID.ToString(),Text=item.Product_Name });
            }

            return list;
        }
        public ActionResult Remove_Attach(int id)
        {
            try
            {
                Product pro = new Product();
                pro.Remove_Attach(id);
                return RedirectToAction("Product");
            }
            catch (Exception e)
            {
                return View(e);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product p, HttpPostedFileBase PostedFile)
        {
            try
            {
                Product pro = new Product();
                pro.upadate_Product(p, PostedFile);
                return RedirectToAction("Product");
            }
            catch (Exception e)
            {
                return View(e);
            }
           
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Single(int id)
        {
            try
            {
                Product pro = new Product();
                Product p = pro.single_Product(id);
                return View(p);
            }
            catch (Exception e)
            {
                return View(e);
            }
           
        }

    }
}
