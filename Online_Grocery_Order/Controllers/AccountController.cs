using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Online_Grocery_Order.Models;
using System.Web.Security;
using System.Linq;

namespace Online_Grocery_Order.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class AccountController : Controller
    {
        DateTime Now_Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        groceryEntities ctx = new groceryEntities();
        public object PostedFile { get; private set; }
        // GET: Account
        [HttpGet]
       
        public ActionResult Admin()
        {

            return View();
        }
        
        [HttpGet]
        public ActionResult Account(int page=1)
        {
            try
            {
                Account user = new Account();
                List<Account> files = user.dispaly_Account();

                ViewBag.TotalPages = (files.Count() / 20) + 1;
                files = files.Skip((page - 1) * 20).Take(20).ToList();
                return View(files);
            }
            catch(Exception e)
            {
                return View(e);
            }
           
        }
        [HttpGet]
        [AllowAnonymous]
       
        public ActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return View(e);
            }
           
        }
        [HttpPost]
        
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account c)
        {
            try
            {
                Account user = new Account();
                Account file = user.Login(c);
                if (file != null)
                {
                    Session["name"] = file.Full_Name;
                    Session["id"] = file.id;
                    Session["officeAddress"] = file.Address_Office;
                    Session["contact"] = file.Contact;
                    Session["address"] = file.Address;
                    Session["email"] = file.Email;
                    Session["type"] = file.User_Type;
                    Session["image"] = file.Account_img;
                    Session["img-type"] = file.type_img;

                    Cart car = new Cart();
                    Session["cart_count"] = car.count_Item();

                    if (Session["type"].ToString().Equals("Admin"))
                    {
                        //var group = ctx.repeats.GroupBy(x => x.o_id).Select(x => x.OrderByDescending(z => new { z.o_date, z.u_id, z.shipping, z.o_id, z.r_id }).FirstOrDefault());
                        //List<Order> ord = group.Join(ctx.orders, r => r.o_id, o => o.o_id, (r, o) => new Order()
                        //{
                        //    rid = r.r_id,
                        //    oid = o.o_id,
                        //    uid = (int)o.u_id,
                        //    odate = (DateTime)r.o_date,
                        //    shipping = r.shipping,
                        //    repeat = o.repeat,
                        //    peroid = (int)o.period
                        //}).Where(e => e.repeat != "NO").ToList<Order>();
                        //foreach (Order item in ord)
                        //{
                        //    TimeSpan diffResult = Now_Date.Subtract(item.odate);
                        //    if (item.peroid <= diffResult.Days)
                        //    {
                        //        repeat rep = new repeat();
                        //        rep.u_id = item.uid;
                        //        rep.o_date = Now_Date;
                        //        rep.shipping = item.shipping;
                        //        rep.status = "Pending";
                        //        rep.o_id = item.oid;
                        //        rep.boy_id = 0;
                        //        ctx.repeats.Add(rep);
                        //        ctx.SaveChanges();
                        //    }
                        //}
                        return RedirectToAction("Account", "Account");
                    }
                    else if (Session["type"].ToString().Equals("DeliveryBoy"))
                    {
                        return RedirectToAction("DeliveryBoy", "DeliveryBoy");
                    }
                    else
                    {
                        return RedirectToAction("Product", "Home");
                    }
                }
                else
                {
                    ViewBag.Res = "Invalid Users Record";
                    return View();
                }
            }
            catch (Exception e)
            {
                return View(e);
            }
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            try
            {
                Session.RemoveAll(); //Clear all session variables
                return RedirectToAction("Product", "Home");
            }
            catch (Exception e)
            {
                return View(e);
            }    
        }
        [HttpGet]
        [AllowAnonymous]
       
        public ActionResult SignUp()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return View(e);
            }
           
        }
       
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Account a)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Account user = new Account();
                    user.insert_Account(a);
                    ViewBag.Res = "Successfull Registration";
                    Account file = user.Login(a);
                    if (file != null)
                    {
                        Session["name"] = file.Full_Name;
                        Session["id"] = file.id;
                        Session["officeAddress"] = file.Address_Office;
                        Session["contact"] = file.Contact;
                        Session["address"] = file.Address;
                        Session["email"] = file.Email;
                        Session["type"] = file.User_Type;
                        Session["image"] = file.Account_img;
                        Session["img-type"] = file.type_img;

                        Cart car = new Cart();
                        Session["cart_count"] = car.count_Item();

                        if (Session["type"].ToString().Equals("Admin"))
                        {
                            return RedirectToAction("Account", "Account");
                        }
                        else if (Session["type"].ToString().Equals("DeliveryBoy"))
                        {
                            return RedirectToAction("DeliveryBoy", "DeliveryBoy");
                        }
                        else
                        {
                            return RedirectToAction("Product", "Home");
                        }
                    }
                    else
                    {
                        ViewBag.Res = "Invalid Users Record";
                        return View();
                    }
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return View("SignUp");

                }
            }
            catch (Exception e)
            {
                return View(e);
            }
            
        }
        [HttpGet]
        public ActionResult UserProfile()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return View(e);
            }

        }
        [HttpGet]
        [AllowAnonymous]
       
        public ActionResult Delete(int id)
        {
            try
            {
                Account user = new Account();
                user.delete_Account(id);
                return RedirectToAction("Account");
            }
            catch (Exception e)
            {
                return View(e);
            }
           
        }

        [HttpGet]
        [AllowAnonymous]

        public ActionResult Edit(int id)
        {
            try
            {
                Account user = new Account();
                Account files = user.update_Account(id);
                return View(files);
            }
            catch (Exception e)
            {
                return View(e);
            }
          
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account a, HttpPostedFileBase PostedFile)
        {
            try
            {
                Account user = new Account();
                user.update_Account(a, PostedFile);
                return RedirectToAction("Account");
            }
            catch (Exception e)
            {
                return View(e);
            }
          
        }
        public ActionResult Profile_picture(HttpPostedFileBase PostedFile)
        {
            try
            {
                int id = (int)Session["id"];
                Account user = new Account();
                user.update_Picture(PostedFile, id);
                Account file = user.update_Account(id);

                Session["name"] = file.Full_Name;
                Session["id"] = file.id;
                Session["gender"] = file.Gender;
                Session["contact"] = file.Contact;
                Session["address"] = file.Address;
                Session["email"] = file.Email;
                Session["type"] = file.User_Type;
                Session["image"] = file.Account_img;
                Session["img-type"] = file.type_img;

                if (Session["type"].ToString().Equals("Admin"))
                {
                    return RedirectToAction("Account", "Account");
                }
                else if (Session["type"].ToString().Equals("DeliveryBoy"))
                {
                    return RedirectToAction("DeliveryBoy", "DeliveryBoy");
                }
                else
                {
                    return RedirectToAction("Customer", "Customer");
                }
            }
            catch (Exception e)
            {
                return View(e);
            }
           
        }
    }
}
