using Online_Grocery_Order.Models;
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
    public class Cart
    {
        public int Cart_ID { get; set; }
        public int U_ID { get; set; }
        public int P_ID { get; set; }
        public int qty { get; set; }

        groceryEntities ctx = new groceryEntities();

        public List<int> Check_Cart_Product(int id)
        {
            List<int> files = new List<int>();
            var list = ctx.carts.Select(e => new { e.p_id, e.u_id }).Where(e => e.u_id == id).ToList();
            foreach (var item in list)
            {
                files.Add((int)item.p_id);
            }
            return files;
        }
        public void add_Cart(int pid , int uid)
        {
            
                cart cat = new cart();
                cat.p_id = pid;
                cat.u_id = uid;
                cat.qty = 1;
                ctx.carts.Add(cat);
                ctx.SaveChanges();
            
        }
        public cart check_Cart(int pid, int uid)
        {
            var cat= ctx.carts.Where(c => c.p_id == pid && c.u_id == uid).FirstOrDefault();
            return cat;
        }
        public void update_Cart(int id)
        {
            int uid = (int)HttpContext.Current.Session["id"];
           
                var result = ctx.carts.SingleOrDefault(c => c.u_id == uid && c.p_id==id);
                if (result != null)
                {
                    result.p_id = id;
                    ctx.SaveChanges();
                }
            
        }
        public int count_Item()
        {
            int cnt;
            var uid = Convert.ToString(HttpContext.Current.Session["id"]);
            
                var u = ctx.carts.Where(c => c.u_id.ToString() == uid).Select(c=>c.qty).ToList();
                cnt = u.Count();
            
            return cnt;
        }
        public void qty_Increase(int id)
        {
            var uid = (int)HttpContext.Current.Session["id"];
            int qty;
          
                var query = ctx.carts.Select(c => new { c.qty,c.p_id,c.u_id }).Where(c => c.u_id == uid && c.p_id == id).FirstOrDefault();
                qty = (int)query.qty +1;
                var result = ctx.carts.SingleOrDefault(c => c.u_id == uid && c.p_id == id);
                if (result != null)
                {
                    result.qty = qty;
                    ctx.SaveChanges();
                }
            
        }
        public void qty_Decrease(int id)
        {

            int uid = (int)HttpContext.Current.Session["id"];
            int qty;
           
                var query = ctx.carts.Select(c => new { c.qty, c.p_id, c.u_id }).Where(c => c.u_id == uid && c.p_id == id).FirstOrDefault();
                qty = (int)query.qty - 1;
                var result = ctx.carts.SingleOrDefault(c => c.u_id == uid && c.p_id == id);
                if (result != null)
                {
                    result.qty = qty;
                    ctx.SaveChanges();
                }
            
        }
        public void item_Remove(int id)
        {
            var uid = Convert.ToString(HttpContext.Current.Session["id"]);
           
                var u = ctx.carts.Where(c => c.u_id.ToString() == uid && c.p_id == id).FirstOrDefault();
                ctx.Entry(u).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();

        }
        public void cart_Empty()
        {
            var uid = Convert.ToString(HttpContext.Current.Session["id"]);
           
                var u = ctx.carts.Where(c => c.u_id.ToString() == uid).ToList();
                foreach(var item in u)
                {
                    ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
            
        }
        public List<Product> cart_Details()
        {
            List<Product> files = null;
            var uid = Convert.ToString(HttpContext.Current.Session["id"]);
           
                var cart = ctx.carts.Where(c => c.u_id.ToString() == uid);
                files = cart.Join(ctx.products, c=>c.p_id, p=>p.p_id, (c,p)=>
                          new Product()
                          {
                              
                              Product_ID = p.p_id,
                              Product_Quantity = (int)c.qty,
                              Product_Weight = (int)p.p_weight,
                              Product_Name = p.p_name,
                              Product_Deacription = p.p_description,
                              Product_Calories = (int)p.p_calories,
                              Product_img = p.p_img,
                              Product_Price = (int)p.p_price,
                              Product_Category = (int)p.c_id,
                              Product_Unit=(int)p.p_unit,
                              Type = p.p_img_type

                         }).ToList<Product>();
            
            return files;
        }
    }
}