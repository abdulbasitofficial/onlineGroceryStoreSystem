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
    public class DeliveryBoyController : Controller
    {
        // GET: DeliveryBoy
        public ActionResult DeliveryBoy()
        {
            int id = (int)Session["id"];
            Order ord = new Order();
            History his = new History();
            List<Account> undelivered = ord.Pending_Order_Boy();
            List<Account> picked = ord.Dispaly_Picked(id);
            List<Account> history = his.Boy_History(id);
            Session["History"] = history.Count();
            Session["Undelivered"] = undelivered.Count();
            Session["Picked"] = picked.Count();
            return View();
        }
        [HttpGet]
        public ActionResult OrdersDeliver()
        {
            Order ord = new Order();
            List<Account> files = ord.Pending_Order_Boy();
            Session["Undelivered"] = files.Count();
            return View(files);
        }
        public ActionResult PickedDetails()
        {
            int id = (int)Session["id"];
            Order ord = new Order();
            Account acc = new Account();
            List<Account> files = ord.Dispaly_Picked(id);
            ViewBag.DeliveryBoy = acc.DeliveryBoyDropDownList(id);
            ViewBag.Status = StatusDropDownList();
            Session["Picked"] = files.Count();
            return View(files);
        }
        public List<SelectListItem> StatusDropDownList()
        {

            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem { Value = "Delivery", Text = "Delivery" });
            return status;
        }

    }
}