using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Online_Grocery_Order.Models;
namespace Online_Grocery_Order
{
    public class MvcApplication : System.Web.HttpApplication
    {
        groceryEntities ctx = new groceryEntities();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //System.Timers.Timer timer = new System.Timers.Timer();
            //timer.Interval = 60000; // one Minute
            //timer.Elapsed += timer_Elapsed;
            //timer.Start();

            //     var UserInRole = db.UserProfiles.Join(db.UsersInRoles, u => u.UserId, uir => uir.UserId,
            //(u, uir) => new { u, uir }). Join(db.Roles, r => r.uir.RoleId, ro => ro.RoleId, (r, ro) => new { r, ro })
            //.Where(m => m.r.u.UserId == 1)
            //.Select(m => new AddUserToRole
            //{
            //    UserName = m.r.u.UserName,
            //    RoleName = m.ro.RoleName
            //});
            //TimeSpan start = new TimeSpan(10, 0, 0); //10 o'clock
            //TimeSpan end = new TimeSpan(12, 0, 0); //12 o'clock
            //TimeSpan now = DateTime.Now.TimeOfDay;

            DateTime Now_Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //DateTime now = DateTime.Now;
            //DateTime currentTime = new DateTime();
            //DateTime fix= new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);  //00:00:00 => 12:00 AM

            TimeSpan start = TimeSpan.Parse("23:59");  // 10 PM
            TimeSpan end = TimeSpan.Parse("12:05");    // 2 AM
            TimeSpan now = DateTime.Now.TimeOfDay;

            string startTime = "12:00 AM";
            string endTime = "8:30 PM";


            bool bMatched = now >= start && now < end;
            if (bMatched)
            {
                var group = ctx.repeats.GroupBy(x => x.o_id).Select(x => x.OrderByDescending(z => new { z.o_date, z.u_id, z.shipping, z.o_id, z.r_id }).FirstOrDefault());
                List<Order> ord = group.Join(ctx.orders, r => r.o_id, o => o.o_id, (r, o) => new Order()
                {
                    rid = r.r_id,
                    oid = o.o_id,
                    uid = (int)o.u_id,
                    odate = (DateTime)r.o_date,
                    shipping = r.shipping,
                    repeat = o.repeat,
                    peroid = (int)o.period
                }).Where(e => e.repeat != "NO").ToList<Order>();
                foreach (Order item in ord)
                {
                    TimeSpan diffResult = Now_Date.Subtract(item.odate);
                    if (item.peroid <= diffResult.Days)
                    {
                        repeat rep = new repeat();
                        rep.u_id = item.uid;
                        rep.o_date = Now_Date;
                        rep.shipping = item.shipping;
                        rep.status = "Pending";
                        rep.o_id = item.oid;
                        rep.boy_id = 0;
                        ctx.repeats.Add(rep);
                        ctx.SaveChanges();
                    }
                }
            }
        }
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Order ord = new Order();
            List<Account> acc = ord.Repeat_Order_To_Deliver();
            if (acc.Count() > 0)
            {
                foreach (var item in acc)
                {
                    ord.insert_Auto_Generate(item.order_ID, item.id, item.Address);
                }
            }

            //groceryEntities DB = new groceryEntities();
            //cart Y = new cart();
            //Y.u_id = 11;
            //Y.p_id = 7;
            //Y.qty = 1;
            //DB.carts.Add(Y);
            //DB.SaveChanges();
        }
    }
}
