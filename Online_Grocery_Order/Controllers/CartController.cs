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
    public class CartController : Controller
    {
        DBConnection db = new DBConnection();
        // GET: Cart
        [HttpGet]
        public ActionResult AddCart(int id)
        {
           
            if (Session["email"]!=null)
            {
                int uid = (int)Session["id"];
                Cart c = new Cart();
                cart item = c.check_Cart(id, uid);
                if (item !=null)
                {
                    return RedirectToAction("Product", "Home");
                }
                else
                {
                    c.add_Cart(id,uid);
                    return RedirectToAction("Product", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }
        [HttpGet]
        public ActionResult Cart()
        {
            if (Session["email"] != null)
            {
                int id = (int)Session["id"];
                Cart cart = new Cart();
                List<Product> files = cart.cart_Details();

                Cart car = new Cart();
                Session["cart_count"] = car.count_Item();

                Account acc = new Account();
                ViewBag.Address = acc.AddressDropDownList(id);

                Product pro = new Product();
                ViewBag.Repeate = pro.Repeat_Order();

                return View(files);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult Increase(int id)
        {
            Cart cart = new Cart();
            cart.qty_Increase(id);
            return RedirectToAction("Cart");
        }
        public ActionResult Decrease(int id)
        {
            Cart cart = new Cart();
            cart.qty_Decrease(id);
            return RedirectToAction("Cart");
        }
        public ActionResult Remove(int id)
        {
            Cart cart = new Cart();
            cart.item_Remove(id);
            return RedirectToAction("Cart");
        }
        public ActionResult Cancel()
        {
            Cart cart = new Cart();
            cart.cart_Empty();
            return RedirectToAction("Cart");
        }
    }
}