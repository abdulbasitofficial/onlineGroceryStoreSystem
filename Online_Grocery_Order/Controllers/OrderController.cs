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
    public class OrderController : Controller
    {
        // GET: Order

        [HttpPost]
        public ActionResult Insert_Order(string address, string repeat)
        {
            if (Session["email"] != null)
            {
                int id = (int)Session["id"];
                Order pen = new Order();
                int peroid = int.Parse(repeat);
                pen.insert_Order_Repeat(id, address, peroid);

                return RedirectToAction("PendingOrder", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
           
        }
        public ActionResult Auto_Generate_Order(int id,int uid ,string address)
        {
            Order pen = new Order();
            pen.insert_Auto_Generate(id, uid, address);
            return RedirectToAction("RepeatOrder", "Order");
        }
        public ActionResult Order()
        {
            return View();
        }
        public ActionResult PendingOrder()
        {
            Order ord = new Order();
            Account acc = new Account();
            List<Account> files = ord.Pending_Order_Admin();
            Session["Pending_Order"] = files.Count();
            ViewBag.DeliveryBoy = acc.DeliveryBoyDropDownList();
            ViewBag.DeliveryBoyAssign = acc.GetDeliveryBoyInfo();
            return View(files);
        }
        public ActionResult RepeatOrder()
        {
            //Order ord = new Order();
            //List<Account> files = ord.Repeat_Order_To_Deliver();
            Order ord = new Order();
            List<Account> file = ord.Repeat_Order_Admin();
            Session["Repeat_Order"] = file.Count();
            return View(file);
        }
       //BoyOrder Details
        public ActionResult OrdersDetails(int id ,string name)
        {
            Order ord = new Order();
            List<Product> files = ord.OrderDetails(id);
            ViewBag.Name = name;
            return View(files);
        }
        public ActionResult OrdersDetailsAdmin(int id, string name,string address)
        {
            Order ord = new Order();
            List<Product> files = ord.OrderDetails(id);
            ViewBag.Name = name;
            ViewBag.Address = address;
            return View(files);
        }
        public ActionResult OrdersDetailsUser(int id, string name)
        {
            Order ord = new Order();
            List<Product> files = ord.OrderDetails(id);
            ViewBag.Name = name;
            return View(files);
        }
        //order id and pid
        public ActionResult ReturnProduct(int id, int pid,string name)
        {
            Order ord = new Order();
            ord.ReturnProduct(id,pid);
            if(Session["type"].ToString().Equals("DeliveryBoy"))
            {
                return RedirectToAction("OrdersDetails", new { id = id, name = name });
            }
            else
            {
                return RedirectToAction("OrdersDetailsUser", new { id = id, name = name });
            }
           
        }
        public ActionResult Picked(int id)
        {
            if (Session["email"] != null)
            {
                int bid = (int)Session["id"];
                Order ord = new Order();
                ord.picked(id, bid);
                return RedirectToAction("OrdersDeliver", "DeliveryBoy");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult AssignOrder(string orderid,string userid)
        {
            int bid = int.Parse(userid);
            int oid = int.Parse(orderid);
            Order ord = new Order();
            ord.AssignOrder(oid, bid);
            if (Session["type"].ToString().Equals("DeliveryBoy"))
            {
                return RedirectToAction("PickedDetails", "DeliveryBoy");
            }
            else
            {
                return RedirectToAction("PendingOrder");
            }
            
        }
        public ActionResult Status(string orderID,string status)
        {
            Order ord = new Order();
            int oid = int.Parse(orderID);
            ord.Status_Upadate(oid, status);
            return RedirectToAction("PickedDetails", "DeliveryBoy");
        }
        public ActionResult Cancel(int id)
        {
            string status = "Cancel";
            Order ord = new Order();
            ord.Cancel(id, status);
            return RedirectToAction("PendingOrder", "Home");
        }
        public ActionResult Delivered(int id)
        {
            string status = "Delivered";
            Order ord = new Order();
            ord.Delivered(id, status);
            if (Session["type"].ToString().Equals("DeliveryBoy"))
            {
                return RedirectToAction("PickedDetails","DeliveryBoy");
            }
            else
            {
                return RedirectToAction("PendingOrder", "Home");
            }
           
        }
        //public ActionResult Repeat_Order_Admin()
        //{
            
        //    Order ord = new Order();
        //    List<Order> file = ord.Repeat_Order_Admin();
        //    return View(file);
        //}
    }
}