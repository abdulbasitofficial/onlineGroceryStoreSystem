using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Online_Grocery_Order.Models
{
    [SessionState(SessionStateBehavior.Default)]
    public class Order
    {
        DateTime Now_Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        DBConnection db = new DBConnection();
        public int uid { get; set; }
        public int oid { get; set; }
        public int rid { get; set; }
        public string bname { get; set; }
        public int pid { get; set; }
        public int qty { get; set; }
        public int boy { get; set; }
        public string status { get; set; }

        public string shipping { get; set; }
        public string repeat { get; set; }
        public string Product_Return { get; set; }
        public int peroid { get; set; }
        public DateTime odate { get; set; }
        public DateTime pdate { get; set; }
        public DateTime ddate { get; set; }

        groceryEntities ctx = new groceryEntities();
        public void insert_Order(int id, string address)
        {
                order ord = new order();
                ord.u_id = id;
                ord.o_date = Now_Date;
                ord.shipping = address;
                ord.status = "Pending";
                ord.repeat = "NO";
                ctx.orders.Add(ord);
                ctx.SaveChanges();

                var order_no = ctx.orders.Max(e => e.o_id);

                var file = ctx.users.Join(ctx.carts, u => u.u_id, c => c.u_id, (u,c)=>new Order() {
                    pid = (int)c.p_id,
                    qty = (int)c.qty,
                    uid = (int)c.u_id,
                    shipping = u.u_address
                }).Where(c=>c.uid==id).ToList<Order>();
                
                foreach (var item in file)
                {
                var qq = ctx.products.Select(p => new { p.p_price,p.p_id }).Where(p => p.p_id == item.pid).FirstOrDefault();
                     int price = (int)qq.p_price;
                    invoice inv = new invoice();
                    inv.p_id = item.pid;
                    inv.o_id = order_no;
                    inv.p_qty = item.qty;
                    inv.p_price = price;
                    ctx.invoices.Add(inv);
                    ctx.SaveChanges();
                }

               var user = ctx.carts.Where(c => c.u_id == id).ToList();
               foreach (var item in user)
                {
                 ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                 ctx.SaveChanges();
                }
            
        }
        public void insert_Auto_Generate(int id, int uid,string address)
        {
            repeat rep = new repeat();
            rep.u_id = uid;
            rep.o_date = Now_Date;
            rep.shipping = address;
            rep.status = "Pending";
            rep.o_id = id;
            ctx.repeats.Add(rep);
            ctx.SaveChanges();
        }
        public void insert_Order_Repeat(int id, string address, int peroid)
        {
            order ord = new order();
            ord.u_id = id;
            ord.o_date = Now_Date;
            ord.shipping = address;
            if (peroid == 0)
            {
                ord.repeat = "NO";
                ord.period = 0;
            }
            else
            {
                ord.repeat = "YES";
                ord.period = peroid;
            }
            ctx.orders.Add(ord);
            ctx.SaveChanges();

            var order_no = ctx.orders.Where(o => o.u_id == id).Max(e => e.o_id);

            repeat rep = new repeat();
            rep.u_id = id;
            rep.o_date = Now_Date;
            rep.shipping = address;
            rep.status = "Pending";
            rep.o_id = order_no;
            rep.boy_id = 0;
            ctx.repeats.Add(rep);
            ctx.SaveChanges();

            var rep_order_no = ctx.repeats.Where(o => o.u_id == id).Max(e => e.r_id);

            var file = ctx.users.Join(ctx.carts, u => u.u_id, c => c.u_id, (u, c) => new Order()
            {
                pid = (int)c.p_id,
                qty = (int)c.qty,
                uid = (int)c.u_id,
                shipping = u.u_address
            }).Where(c => c.uid == id).ToList<Order>();

            foreach (var item in file)
            {
                var qq = ctx.products.Select(p => new { p.p_price, p.p_id }).Where(p => p.p_id == item.pid).FirstOrDefault();
                int price = (int)qq.p_price;
                invoice inv = new invoice();
                inv.p_id = item.pid;
                inv.o_id = order_no;
                inv.p_qty = item.qty;
                inv.p_price = price;
                ctx.invoices.Add(inv);
                ctx.SaveChanges();
            }
            Product pp = new Product();
            List<Product> list = pp.dispaly_Product();
            foreach (var itm in list)
            {
                foreach (var cat in file)
                {
                    if (itm.Product_ID == cat.pid)
                    {
                        int stock = itm.Product_Quantity - cat.qty;
                        var result = ctx.products.SingleOrDefault(b => b.p_id == itm.Product_ID);
                        if (result != null)
                        {
                            result.p_quantity = stock;
                            ctx.SaveChanges();
                        }
                    }

                }
            }
            var user = ctx.carts.Where(c => c.u_id == id).ToList();
            foreach (var item in user)
            {
                ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

        }
        public List<Product> display_Pending(int id)
        {
            List<Product> files =new  List<Product>();
            
                var lastorder = ctx.repeats.Where(o => o.u_id == id && o.status != "Cancel" && o.status != "Delivered");
                
                if(lastorder.Count() > 0)
                {
                var LastRepeayeOrder = ctx.repeats.Where(o => o.u_id == id && o.status != "Cancel" && o.status != "Delivered").Max(e => e.r_id);
                var OrderId = ctx.repeats.Select(e =>new { e.o_id ,e.r_id}).Where(e=>e.r_id==LastRepeayeOrder).FirstOrDefault();
                  
                    files = ctx.invoices.Join(ctx.products, i => i.p_id, p => p.p_id, (i, p) =>
                                  new Product()
                                  {
                                      Product_ID = p.p_id,
                                      Product_Quantity = (int)i.p_qty,
                                      Product_Weight = (int)p.p_weight,
                                      Product_Name = p.p_name,
                                      Product_Deacription = p.p_description,
                                      Product_Calories = (int)p.p_calories,
                                      Product_img = p.p_img,
                                      Product_Price = (int)p.p_price,
                                      Product_Category = (int)p.c_id,
                                      Product_Unit = (int)p.p_unit,
                                      Type = p.p_img_type,
                                      Order_No = (int)i.o_id,
                                      rid=OrderId.r_id
                                  }).Where(e => e.Order_No == OrderId.o_id).ToList<Product>();
                }
               
            
            return files;
        }
        public List<Product> OrderDetails( int id)
        {
            List<Product> files = new List<Product>();

                var inv = ctx.invoices.Where(i => i.o_id == id);

            files = inv.Join(ctx.products, i => i.p_id, p => p.p_id, (i, p) =>
                          new Product()
                          {
                              Product_ID = p.p_id,
                              Product_Quantity = (int)i.p_qty,
                              Product_Weight = (int)p.p_weight,
                              Product_Name = p.p_name,
                              Product_Deacription = p.p_description,
                              Product_Calories = (int)p.p_calories,
                              Product_img = p.p_img,
                              Product_Price = (int)i.p_price,
                              Product_Category = (int)p.c_id,
                              Product_Unit = (int)p.p_unit,
                              Type = p.p_img_type,
                              Order_No = id,
                              Product_Return=i.p_return
                              }).OrderBy(e=>e.Product_Return).ToList<Product>();
           
            return files;
        }

        public void picked(int oid, int bid)
        {
            var result = ctx.repeats.SingleOrDefault(o => o.r_id == oid);
            if (result != null)
            {
                result.status = "Picked";
                result.boy_id = bid;
                result.p_date = Now_Date;
                ctx.SaveChanges();
            }
        }
        public void AssignOrder(int oid, int bid)
        {
            var result = ctx.repeats.SingleOrDefault(o => o.r_id == oid);
            if (result != null)
            {
                result.status = "Picked";
                result.boy_id = bid;
                result.p_date = Now_Date;
                ctx.SaveChanges();
            }
        }
        public void Status_Upadate(int oid ,string status)
        {
            var result = ctx.repeats.SingleOrDefault(o => o.r_id ==oid);
            if (result != null)
            {
                result.status = status;
                ctx.SaveChanges();
            }
        }
        public void Repeat_Upadate(int oid, int peroid)
        {
            var result = ctx.orders.SingleOrDefault(o => o.o_id == oid);
           
            if (result != null)
            {
                if (peroid == 0)
                {
                    result.repeat = "NO";
                    result.period = null;
                    ctx.SaveChanges();
                }
                else
                {
                    result.period = peroid;
                    ctx.SaveChanges();
                }
            }
        }
        public void Delivered(int oid, string status)
        {
            var result = ctx.repeats.SingleOrDefault(o => o.r_id == oid);
            if (result != null)
            {
                result.d_date = Now_Date;
                result.status = status;
                ctx.SaveChanges();
            }
        }
        public void ReturnProduct(int oid, int pid)
        {
            var result = ctx.invoices.Where(x=>x.o_id==oid && x.p_id==pid).FirstOrDefault();
            if (result != null)
            {
                result.p_return = "YES";
                ctx.SaveChanges();
            }
        }
        public void Cancel(int oid, string status)
        {
            var result = ctx.repeats.SingleOrDefault(o => o.r_id == oid);
            if (result != null)
            {
                result.c_date = Now_Date;
                result.status = status;
                ctx.SaveChanges();
            }
        }
        public List<Account> Dispaly_Picked(int id)
        {
            List<Account> files = null;
           
                var group = ctx.repeats.Select(e => new { e.r_id, e.u_id, e.status, e.shipping, e.o_date,e.boy_id,e.o_id,e.p_date }).Where(e => e.status != "Cancel" && e.status != "Delivered" && e.boy_id==id);
                files = group.Join(ctx.users, i => i.u_id, u => u.u_id, (i, u) => new Account()
                {
                    id = u.u_id,
                    Full_Name = u.u_name,
                    Email = u.u_email,
                    Address = u.u_address,
                    Contact = u.u_contact,
                    Password = u.u_password,
                    User_Type = u.u_type,
                    Gender = u.u_gender,
                    Account_img = u.u_img,
                    type_img = u.u_img_type,
                    OrderDate = i.o_date.ToString(),
                    PickedDate= i.p_date.ToString(),
                    statusOrder = i.status,
                    order_ID = (int)i.o_id,
                    rid= (int)i.r_id

                }).Distinct().ToList<Account>();
            
            return files;
        }
        public List<Account> Pending_Order_Admin()
        {
            List<Account> files = null;
            
                var group = ctx.repeats.Select(e => new { e.r_id, e.u_id, e.status, e.shipping, e.o_date,e.o_id,e.p_date,e.c_date,e.d_date,e.boy_id }).Where(e => e.status != "Cancel" && e.status != "Delivered").ToList();
                files = group.Join(ctx.users, i => i.u_id, u => u.u_id, (i, u) => new Account()
                {
                    id = u.u_id,
                    boy_ID=(int)i.boy_id,
                    Full_Name = u.u_name,
                    Email = u.u_email,
                    Address = u.u_address,
                    Contact = u.u_contact,
                    Password = u.u_password,
                    User_Type = u.u_type,
                    Gender = u.u_gender,
                    Account_img = u.u_img,
                    type_img = u.u_img_type,
                    OrderDate = i.o_date.ToString(),
                    PickedDate = i.p_date.ToString(),
                    statusOrder = i.status,
                    order_ID = (int)i.o_id,
                    rid= (int)i.r_id
                }).OrderBy(o => o.statusOrder).Distinct().ToList<Account>();
            
            return files;
        }
        public List<Account> Repeat_Order_To_Deliver()
        {
            List<Account> files = new List<Account>();
            Account user = new Account();
            var group = ctx.repeats.GroupBy(x => x.o_id).Select(x => x.OrderByDescending(z => new { z.o_date,z.u_id,z.shipping,z.o_id,z.r_id }).FirstOrDefault());
            List<Order> ord = group.Join(ctx.orders, r => r.o_id, o => o.o_id, (r, o) => new Order()
            {
                rid = r.r_id,
                oid=o.o_id,
                uid=(int)o.u_id,
                odate=(DateTime)r.o_date,
                shipping=r.shipping,
                repeat=o.repeat,
                peroid=(int)o.period
            }).Where(e => e.repeat != "NO").ToList<Order>();
            
            foreach(Order item in ord)
            {
                TimeSpan diffResult = Now_Date.Subtract(item.odate);

                if (item.peroid<= diffResult.Days)
                {
                    Account record = user.update_Account(item.uid);
                    files.Add(
                        new Account {
                            rid=item.rid,
                            id = item.uid,
                            Full_Name = record.Full_Name,
                            Email = record.Email,
                            Address = item.shipping,
                            Contact = record.Contact,
                            Account_img = record.Account_img,
                            type_img = record.type_img,
                            Register_Date = item.odate,
                            statusOrder = item.status,
                            order_ID = item.oid
                        });
                }
            }
            return files;
        }
        public List<Account> Pending_Order_Boy()
        {
            List<Account> files = null;
            
                var group = ctx.repeats.Select(e => new { e.r_id, e.u_id, e.status, e.shipping, e.o_date ,e.o_id}).Where(e => e.status == "Pending");
                files = group.Join(ctx.users, i => i.u_id, u => u.u_id, (i, u) => new Account()
                {
                    id = u.u_id,
                    Full_Name = u.u_name,
                    Email = u.u_email,
                    Address = u.u_address,
                    Contact = u.u_contact,
                    Password = u.u_password,
                    User_Type = u.u_type,
                    Gender = u.u_gender,
                    Account_img = u.u_img,
                    type_img = u.u_img_type,
                    Register_Date = (DateTime)i.o_date,
                    statusOrder = i.status,
                    order_ID = (int)i.o_id
                }).Distinct().ToList<Account>();
            
            return files;
        }
        public List<Order> Customer_Repeat_Order(int id)
        {
            List<Order> files = new List<Order>();
            files = ctx.orders.Join(ctx.repeats, o => o.o_id, r => r.o_id, (o, r) => new Order()
            {
                rid=r.r_id,
                oid= o.o_id,
                uid =(int)o.u_id,
                odate = (DateTime)o.o_date,
                peroid=(int)o.period,
                repeat=o.repeat,
                status=r.status
            }).Where(w=>w.repeat=="YES" && w.uid==id).Distinct().ToList<Order>();
            return files;
        }
        public List<Account> Repeat_Order_Admin()
        {
            List<Account> files = null;

            var group = ctx.repeats.Select(e => new { e.r_id, e.u_id, e.status, e.shipping, e.o_date, e.o_id }).Where(e => e.status != "Cancel" && e.status != "Delivered");
            files = ctx.orders.Join(ctx.users, i => i.u_id, u => u.u_id, (i, u) => new Account()
            {
                id = u.u_id,
                Full_Name = u.u_name,
                Email = u.u_email,
                Address = i.shipping,
                Contact = u.u_contact,
                Account_img = u.u_img,
                type_img = u.u_img_type,
                Register_Date = (DateTime)i.o_date,
                statusOrder = i.status,
                period = (int)i.period,
                repeat = i.repeat,
                order_ID = i.o_id
            }).Where(w => w.repeat == "YES").Distinct().ToList<Account>();

            return files;
            //List<Order> files = new List<Order>();
            //files = ctx.orders.Join(ctx.repeats, o => o.o_id, r => r.o_id, (o, r) => new Order()
            //{
            //    rid = r.r_id,
            //    oid = o.o_id,
            //    uid = (int)o.u_id,
            //    odate = (DateTime)o.o_date,
            //    peroid = (int)o.period,
            //    repeat = o.repeat,
            //    status = r.status
            //}).Where(w => w.repeat == "YES" && w.status != "Delivered" && w.status != "Cancel").Distinct().ToList<Order>();
            //return files;
        }
    }
}