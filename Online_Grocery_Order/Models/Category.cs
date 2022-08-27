using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Online_Grocery_Order.Models;
namespace Online_Grocery_Order.Models
{
    [SessionState(SessionStateBehavior.Default)]
    public class Category
    {
        DateTime Now_Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        public List<SelectListItem> category { get; set; }
        public int Category_ID { get; set; }
        public string Category_Name { get; set; }
        public DateTime Insert_Date { get; set; }

        groceryEntities ctx = new groceryEntities();
        public void insert_Category(Category c)
        {
            
                category cat = new category();
                cat.c_name = c.Category_Name;
                cat.i_date = Now_Date;
                ctx.categories.Add(cat);
                ctx.SaveChanges();
            
        }
        public List<Category> dispaly_Category()
        {
            List<Category> files = null;

                files = ctx.categories.Select(c => new Category()
                {
                   Category_ID = c.c_id,
                   Category_Name = c.c_name,
                   Insert_Date =(DateTime)c.i_date,

                }).ToList<Category>();
            
            return files;
        }
        public void delete_Category(int id)
        {
            
                var u = ctx.categories.Where(c => c.c_id == id).FirstOrDefault();
                ctx.Entry(u).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            
        }
        public Category upadate_Category(int id)
        {
            Category files = null;

                files = ctx.categories.Where(c=>c.c_id==id).Select(c => new Category()
                {
                    Category_ID = c.c_id,
                    Category_Name = c.c_name,
                }).SingleOrDefault(); 
            
            return files;
        }
        public void upadate_Category(Category c)
        {
           
                var result = ctx.categories.SingleOrDefault(b => b.c_id == c.Category_ID);
                if (result != null)
                {
                    result.c_name = c.Category_Name;
                    ctx.SaveChanges();
                }
            
        }
        public List<SelectListItem> CategoryDropDownList()
        {
            List<Category> files = dispaly_Category();
            List<SelectListItem> category = new List<SelectListItem>();
            foreach (var item in files){
                category.Add(new SelectListItem { Value = item.Category_ID.ToString(), Text = item.Category_Name.ToString() });
            }

            return category;
        }
    }
}