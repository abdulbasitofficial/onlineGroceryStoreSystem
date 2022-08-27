using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Grocery_Order.Models
{
    public class History
    {
        groceryEntities ctx = new groceryEntities();
        public List<Account> Boy_History(int id)
        {
            List<Account> user = new List<Account>();
           
                var group = ctx.repeats.Select(e => new { e.r_id, e.u_id, e.status, e.shipping, e.o_date, e.boy_id,e.o_id,e.p_date,e.d_date }).Where(e => e.status == "Cancel" || e.status=="Delivered");
                user = group.Join(ctx.users, i => i.u_id, u => u.u_id, (i, u) => new Account()
                {
                    id = u.u_id,
                    boy_ID= (int)i.boy_id,
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
                    DeliveredDate = i.d_date.ToString(),
                    statusOrder = i.status,
                    order_ID = (int)i.o_id,
                    rid = (int)i.r_id
                }).Where(e=>e.boy_ID==id).Distinct().ToList<Account>();
            
            return user;
        }
        public List<Account> User_History(int id)
        {
            List<Account> user = new List<Account>();
           
                var group = ctx.repeats.Select(e => new { e.r_id, e.u_id, e.status, e.shipping, e.o_date, e.boy_id,e.o_id,e.p_date,e.d_date }).Where(e => e.status == "Cancel" || e.status == "Delivered");
            user = group.Join(ctx.users, i => i.u_id, u => u.u_id, (i, u) => new Account()
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
                PickedDate = i.p_date.ToString(),
                DeliveredDate = i.d_date.ToString(),
                statusOrder = i.status,
                order_ID = (int)i.o_id,
                rid=(int)i.r_id
                }).Where(e=>e.id==id).Distinct().ToList<Account>();
            
            return user;
        }
        public List<Account> Admin_History()
        {
            List<Account> user = new List<Account>();
           
                var group = ctx.repeats.Select(e => new { e.r_id, e.u_id, e.status, e.shipping, e.o_date,e.boy_id,e.p_date,e.d_date ,e.o_id}).Where(e => e.status == "Cancel" || e.status == "Delivered");
                user = group.Join(ctx.users, i => i.u_id, u => u.u_id, (i, u) => new Account()
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
                    PickedDate = i.p_date.ToString(),
                    DeliveredDate = i.d_date.ToString(),
                    statusOrder = i.status,
                    order_ID = (int)i.o_id,
                    boy_ID = (int)i.boy_id,
                    rid = (int)i.r_id
                }).Distinct().ToList<Account>();
           
            return user;
        }
       
    }
}