using Online_Grocery_Order.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Online_Grocery_Order.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class HistoryController : Controller
    {
        // GET: History
        public ActionResult Admin_History()
        {
            
            History history = new History();
            List<Account> files = history.Admin_History();
            Account acc = new Account();
            ViewBag.DeliveryBoyAssign = acc.GetDeliveryBoyInfo();
            return View(files);
        }
        public ActionResult User_History()
        {
            int id = (int)Session["id"];
            History history = new History();
            List<Account> files = history.User_History(id);
            return View(files);
        }
        public ActionResult Boy_History()
        {
            int id = (int)Session["id"];
            History history = new History();
            List<Account> files = history.Boy_History(id);
            return View(files);
        }
    }
}