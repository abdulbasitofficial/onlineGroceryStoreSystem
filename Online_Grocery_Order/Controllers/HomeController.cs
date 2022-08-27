using Online_Grocery_Order.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Online_Grocery_Order.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    
    public class HomeController : Controller
    {
        groceryEntities ctx = new groceryEntities();
        // GET: Home
        [HttpGet]
        public ActionResult PendingOrder()
        {
            Cart car = new Cart();
            Session["cart_count"] = car.count_Item();

            int id = (int)Session["id"];
            Order pen = new Order();
            List<Product> files = pen.display_Pending(id);
            if (files.Count() > 0) { 
            var orderno = ctx.repeats.Where(o => o.u_id == id).Max(e => e.r_id);
            var order = ctx.repeats.Where(o => o.r_id == orderno && o.boy_id !=0);
                if (order !=null && order.Count() !=0)
                {
                    var file = order.Join(ctx.users, o => o.boy_id, u => u.u_id, (o, u) => new { o, u }).First();
                    if(file!=null)
                    {
                        ViewBag.Boy_Id = file.u.u_id;
                        ViewBag.Boy_Name = file.u.u_name;
                        ViewBag.Boy_Contact = file.u.u_contact;
                        ViewBag.Boy_Status = file.o.status;
                        ViewBag.OrderDate = file.o.o_date;
                        ViewBag.PickDate = file.o.p_date;
                    }
                }
                else
                {
                    ViewBag.Boy_Id = null ;
                    ViewBag.Boy_Name = null;
                    ViewBag.Boy_Contact = null;
                    ViewBag.Boy_Status = null;
                    ViewBag.OrderDate = null;
                    ViewBag.PickDate = null;
                }
            }
            return View(files);
        }

       
        public ActionResult History()
        {
            return View();
        }
        
       
        public ActionResult Product(int page =1)
        {
            Cart car = new Cart();
            Session["cart_count"] = car.count_Item();
            if (Session["id"] != null)
            {
                int u_id = (int)Session["id"];
                ViewBag.CartCheck = car.Check_Cart_Product(u_id);
            }
            Product pro = new Product();
            IList<Product> files = pro.dispaly_Product();
            
            ViewBag.TotalPages = (files.Count() / 30) + 1;
            files = files.Skip((page - 1) * 30).Take(30).ToList();

            Category cat = new Category();
            ViewBag.category = cat.CategoryDropDownList();

            return View(files);

        }
       
        public ActionResult Filteration(string id,int page=1)
        {
            if (id != "")
            { 
            Product pro = new Product();
            List<Product> files = pro.filteraton_Product(id);
            
            ViewBag.TotalPages = (files.Count() / 5) + 1;
            files = files.Skip((page - 1) * 5).Take(5).ToList();

            Category cat = new Category();
            ViewBag.category = cat.CategoryDropDownList();

            return View("Product",files);
            }
            else
            {
                return RedirectToAction("Product");
            }
        }
        public ActionResult Search(string name, int page = 1)
        {
            if (name != "")
            {
                Product pro = new Product();
                List<Product> files = new List<Product>();
                if (Regex.IsMatch(name, @"^\d+$"))
                {
                    int calory = int.Parse(name);
                    files = pro.Search_Calories(calory);
                }
                else
                {
                    files = pro.Search_Product(name);
                }

                ViewBag.TotalPages = (files.Count() / 30) + 1;
                files = files.Skip((page - 1) * 30).Take(30).ToList();

                Category cat = new Category();
                ViewBag.category = cat.CategoryDropDownList();

                return View("Product", files);
            }
            else
            {
                return RedirectToAction("Product");
            }
        }


    }
}