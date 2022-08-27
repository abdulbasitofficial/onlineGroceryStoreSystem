using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Online_Grocery_Order.Models
{
    public class Product
    {
        DateTime Now_Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        public IEnumerable<Category> category { get; set; }
        public int Product_ID { get; set; }

        [Required(ErrorMessage = "Please Enter Product Name")]
        [Display(Name = "Product Name")]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter a valid Name")]
        public string Product_Name { get; set; }


        [Required(ErrorMessage = "Please Enter Product Price")]
        [Display(Name = "Product Price")]
        
        public int Product_Price { get; set; }

        
       
        [Display(Name = "Product Category")]
        public int Product_Category { get; set; }

        public int attach { get; set; }


        [Required(ErrorMessage = "Please Enter Product Quantity")]
        [Display(Name = "Product Quatity")]
       
        public int Product_Quantity { get; set; }

        [Required(ErrorMessage = "Please Enter Product Weight")]
        [Display(Name = "Product Weight")]
        
        public int Product_Weight { get; set; }

        [Required(ErrorMessage = "Please Enter Product Calories")]
        [Display(Name = "Product Calories")]
        
        public int Product_Calories { get; set; }

        [Required(ErrorMessage = "Please Enter Product Description")]
        [Display(Name = "Product Description")]
        [StringLength(200, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 10)]
        public string Product_Deacription { get; set; }

        public string Product_Return { get; set; }

        [Display(Name = "Product Image")]
        public byte[] Product_img { get; set; }
        
        public string Type { get; set; }
        [Required(ErrorMessage = "Please Select Product Offer")]
        [Display(Name = "Product Offer")]
        public string Offer { get; set; }
        // public string img64 { get; set; }
        public string img64url { get; set; }

        public DateTime odate { get; set; }
        public string contact { get; set; }
        public string boyname { get; set; }
        public int uid { get; set; }
        public int rid { get; set; }
        public int Order_No { get; set; }
        public int Product_Unit { get; set; }
        public string status { get; set; }
        public DateTime Insert_Date { get; set; }

        groceryEntities ctx = new groceryEntities();
        public void insert_Product(Product p, HttpPostedFileBase PostedFile)
        {
            byte[] bytes;
            BinaryReader br = new BinaryReader(PostedFile.InputStream);
            bytes = br.ReadBytes(PostedFile.ContentLength);

                product pro = new product();
                pro.p_name = p.Product_Name;
                pro.p_price = p.Product_Price;
                pro.p_quantity = p.Product_Quantity;
                pro.p_weight = p.Product_Weight;
                pro.p_calories = p.Product_Calories;
                pro.p_description = p.Product_Deacription;
                pro.p_img = bytes;
                pro.p_unit = p.Product_Unit;
                pro.p_img_type = PostedFile.ContentType;
                pro.c_id = p.Product_Category;
                pro.i_date = Now_Date;
                pro.p_offer = "NO";
                pro.p_discount = 0;
                ctx.products.Add(pro);
                ctx.SaveChanges();
            
        }
        public List<Product> dispaly_Product()
        {
            List<Product> files = null;

            files = ctx.products.Select(p => new Product()
            {
                Product_ID = p.p_id,
                Product_Quantity = (int)p.p_quantity,
                Product_Weight = (int)p.p_weight,
                Product_Name = p.p_name,
                Product_Deacription = p.p_description,
                Product_Calories = (int)p.p_calories,
                Product_img = p.p_img,
                Product_Price = (int)p.p_price,
                Product_Category = (int)p.c_id,
                Product_Unit = (int)p.p_unit,
                Insert_Date = (DateTime)p.i_date,
                Type = p.p_img_type,
                Offer = p.p_offer,
                }).OrderBy(e=>e.Product_Name).ToList<Product>();
            
           
            return files;
        }
        public List<Product> Home_Product()
        {
           
            List<Product> files = null;

            files = ctx.products.Where(p => ctx.carts.Any(c => (c.p_id == p.p_id))).Select(p => new Product()
            {
                Product_ID = p.p_id,
                Product_Quantity = (int)p.p_quantity,
                Product_Weight = (int)p.p_weight,
                Product_Name = p.p_name,
                Product_Deacription = p.p_description,
                Product_Calories = (int)p.p_calories,
                Product_img = p.p_img,
                Product_Price = (int)p.p_price,
                Product_Category = (int)p.c_id,
                Product_Unit = (int)p.p_unit,
                Insert_Date = (DateTime)p.i_date,
                Type = p.p_img_type,


            }).ToList<Product>();


            return files;
        }
        public void delete_Product(int id)
        {
                var u = ctx.products.Where(p => p.p_id == id).FirstOrDefault();
                ctx.Entry(u).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            
        }
        public Product upadate_Product(int id)
        {
            Product files = single_Product(id);
            return files;
        }
        public void Attach_Product(int attach,int pid)
        {
            var file = ctx.attaches.Where(e => e.Pid == pid && e.AttachProduct == attach).ToList();
            if(file.Count()>0)
            {

            }
            else
            {
                attach aa = new attach();
                aa.Pid = attach;
                aa.AttachProduct =pid ;
                ctx.attaches.Add(aa);
                ctx.SaveChanges();
            }
           
        }
        public void Remove_Attach(int id)
        {
            var result = ctx.products.SingleOrDefault(b => b.p_id == id);
            result.p_offer = "NO";
            ctx.SaveChanges();
        }
        public List<Product> Offer_Product(int id)
        {
            List<Product> files = new List<Product>();
            var list = ctx.products.Join(ctx.attaches, p => p.p_id, a => a.Pid, (p, a) => new { p, a }).Where(m=>m.a.Pid==id).ToList();
           foreach (var item in list)
           {
                var result = ctx.products.Where(p => p.p_id == item.a.AttachProduct).FirstOrDefault();
                files.Add(new Product
                {
                    Product_ID = result.p_id,
                    Product_Quantity = (int)result.p_quantity,
                    Product_Weight = (int)result.p_weight,
                    Product_Name = result.p_name,
                    Product_Deacription = result.p_description,
                    Product_Calories = (int)result.p_calories,
                    Product_img = result.p_img,
                    Product_Price = (int)result.p_price,
                    Product_Category = (int)result.c_id,
                    Product_Unit = (int)result.p_unit,
                    Insert_Date = (DateTime)result.i_date,
                    Type = result.p_img_type,
                    Offer = result.p_offer,
                });
            }
          
            return files;
        }
        public void upadate_Product(Product p, HttpPostedFileBase PostedFile)
        {
            //byte[] bytes;
            //string type;
            //if (PostedFile !=null)
            //{
            //    BinaryReader br = new BinaryReader(PostedFile.InputStream);
            //    bytes = br.ReadBytes(PostedFile.ContentLength);
            //    type = PostedFile.ContentType;
            //}
            //else
            //{
            //    bytes = p.Product_img;
            //    type = p.Type;

            //}
                var result = ctx.products.SingleOrDefault(b => b.p_id ==p.Product_ID);
                if (result != null)
                {
                    result.p_name = p.Product_Name;
                    result.p_price = p.Product_Price;
                    result.p_quantity = p.Product_Quantity;
                    result.p_calories = p.Product_Calories;
                    result.p_weight = p.Product_Weight;
                    result.p_description = p.Product_Deacription;
                    result.p_unit = p.Product_Unit;
                    ctx.SaveChanges();
                }
            
        }
        public Product single_Product(int id)
        {
            Product files = null;

                files = ctx.products.Where(p => p.p_id == id).Select(p => new Product()
                {
                    Product_ID = p.p_id,
                    Product_Quantity = (int)p.p_quantity,
                    Product_Weight = (int)p.p_weight,
                    Product_Name = p.p_name,
                    Product_Deacription = p.p_description,
                    Product_Calories = (int)p.p_calories,
                    Product_img = p.p_img,
                    Product_Price = (int)p.p_price,
                    Product_Unit=(int)p.p_unit,
                    Product_Category = (int)p.c_id,
                    Type = p.p_img_type

                }).SingleOrDefault();
            
            return files;
        }
       
        public List<Product> filteraton_Product(string id)
        {
            List<Product> files = null;
            
                files = ctx.products.Select(p => new Product()
                {
                    Product_ID = p.p_id,
                    Product_Quantity = (int)p.p_quantity,
                    Product_Weight = (int)p.p_weight,
                    Product_Name = p.p_name,
                    Product_Deacription = p.p_description,
                    Product_Calories = (int)p.p_calories,
                    Product_img = p.p_img,
                    Product_Price = (int)p.p_price,
                    Product_Unit = (int)p.p_unit,
                    Product_Category = (int)p.c_id,
                    Type = p.p_img_type
                }).Where(p=>p.Product_Category.ToString()==id).ToList<Product>();
            
            return files;
        }
        public List<Product> Search_Product(string name)
        {
            List<Product> files = null;

                files = ctx.products.Select(p => new Product()
                {
                    Product_ID = p.p_id,
                    Product_Quantity = (int)p.p_quantity,
                    Product_Weight = (int)p.p_weight,
                    Product_Name = p.p_name,
                    Product_Deacription = p.p_description,
                    Product_Calories = (int)p.p_calories,
                    Product_img = p.p_img,
                    Product_Price = (int)p.p_price,
                    Product_Category = (int)p.c_id,
                    Product_Unit = (int)p.p_unit,
                    Type = p.p_img_type
                }).Where(p => p.Product_Name.Contains(name)).ToList<Product>();
            
            return files;
        }
        public List<Product> Search_Calories(int calory)
        {
            List<Product> files = null;

            files = ctx.products.Select(p => new Product()
            {
                Product_ID = p.p_id,
                Product_Quantity = (int)p.p_quantity,
                Product_Weight = (int)p.p_weight,
                Product_Name = p.p_name,
                Product_Deacription = p.p_description,
                Product_Calories = (int)p.p_calories,
                Product_img = p.p_img,
                Product_Price = (int)p.p_price,
                Product_Category = (int)p.c_id,
                Product_Unit = (int)p.p_unit,
                Type = p.p_img_type
            }).Where(p => p.Product_Calories <=calory).ToList<Product>();

            return files;
        }
        public List<Category> CategoryDropDownList()
        {
            Category cat = new Category();
            List<Category> category = new List<Category>();
            List<Category> list = cat.dispaly_Category();
            foreach(var item in list)
            {
                category.Add(new Category { Category_ID = item.Category_ID, Category_Name = item.Category_Name});
            }
            return category;
        }
        public List<SelectListItem> Repeat_Order()
        {
            List<SelectListItem> category = new List<SelectListItem>();
            var data = new[]{
                new SelectListItem { Value = "0", Text = "No Repeat" },
                new SelectListItem { Value = "1", Text = "Repeat After One Day" },
                new SelectListItem { Value = "7", Text = "Repeat After One Week" },
                new SelectListItem { Value = "14", Text = "Repeat After Two Week" },
                new SelectListItem { Value = "21", Text = "Repeat After Three Week" },
                new SelectListItem { Value = "30", Text = " Repeat After One Month" }
            };
            category= data.ToList();
            return category;
        }
    }

}

public enum offer { No,Week,Discount,Other};