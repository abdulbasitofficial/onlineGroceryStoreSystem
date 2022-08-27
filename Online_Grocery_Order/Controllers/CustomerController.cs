using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Online_Grocery_Order.Models;
namespace Online_Grocery_Order.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Customer()
        {
            if (Session["email"] != null)
            {
                int id = (int)Session["id"];
                Order ord = new Order();
                //session Update
                List<Order> file = ord.Customer_Repeat_Order(id);
                Session["Repeat_My_Order"] = file.Count();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            

            return View();
        }
        public ActionResult Repeat_Order()
        {
            if (Session["email"] != null)
            {
                int id = (int)Session["id"];
                Order ord = new Order();
                //session Update
                List<Order> file = ord.Customer_Repeat_Order(id);
                Session["Repeat_My_Order"] = file.Count();

                Product pro = new Product();
                ViewBag.Repeate = pro.Repeat_Order();
                return View(file);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
           
        }
        public ActionResult Repeat_Update(string orderID, string repeat)
        {
            if (Session["email"] != null)
            {
                Order ord = new Order();
                int oid = int.Parse(orderID);
                int peroid = int.Parse(repeat);
                ord.Repeat_Upadate(oid, peroid);
                return RedirectToAction("Repeat_Order", "Customer");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }
    }
}