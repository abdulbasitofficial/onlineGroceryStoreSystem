using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Online_Grocery_Order.Models
{
    [SessionState(SessionStateBehavior.Default)]
    public class Account
    {
        


        [Required(ErrorMessage = "Please Enter Full Name")]
        [Display(Name = "Full Name")]
        [StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter a valid Name")]
        public string Full_Name { get; set; }

        [StringLength(40, ErrorMessage = "Email Lenght Range exceed Must Enter 50 Character")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please Enter Email Address ")]
        [Display(Name = "Email Address")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                            ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }


        [MaxLength(10)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Password")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }



        [Required(ErrorMessage = "Please Enter Address")]
        [Display(Name = "H Address")]
        [StringLength(200, ErrorMessage = "Address Range exceed,  Must Enter 200 Character")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Enter Offer Address")]
        [Display(Name = "O Address")]
        [StringLength(200, ErrorMessage = "Address Range exceed,  Must Enter 200 Character")]
        public string Address_Office { get; set; }


        [Required(ErrorMessage = "Please Select Gender")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }


        [Required(ErrorMessage = "Please Enter Contact")]
        [Display(Name = "Contact")]
        [MaxLength(14)]
        [RegularExpression(@"^\(?([0-9]{2})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{7})$", ErrorMessage = "Not a valid phone number (92-302-7777777)")]
        [DataType(DataType.PhoneNumber)]
        public string Contact { get; set; }


        [Required(ErrorMessage = "Please Select User Type")]
        [Display(Name = "User Type")]
        public string User_Type { get; set; }

        
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Profile Image")]

        public byte[] Account_img { get; set; }
        
        public string type_img { get; set; }

        public HttpPostedFile img { get; set; }
        public DateTime Register_Date { get; set; }
        public DateTime Cancel_Date { get; set; }
        public DateTime Picked_Date { get; set; }

        public string OrderDate { get; set; }
        public string CancelDate { get; set; }
        public string PickedDate { get; set; }
        public string DeliveredDate { get; set; }

        public string statusOrder { get; set; }
         public int boy_ID { get; set; }
        public int order_ID { get; set; }
        DateTime Now_Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        [Display(Name = "User ID")]
        public int id { get; set; }
        public int rid { get; set; }
        public int period { get; set; }
        public String repeat { get; set; }
        public int bid { get; set; }
        groceryEntities ctx = new groceryEntities();
        public void insert_Account(Account a)
        {
            
                user u = new user();
                u.u_name = a.Full_Name;
                u.u_email = a.Email;
                u.u_address = a.Address;
                u.u_address_office = a.Address_Office;
                u.u_contact = a.Contact;
                u.u_type = a.User_Type;
                u.u_gender = a.Gender;
                u.u_password= a.Password;
                u.i_date = Now_Date;
                ctx.users.Add(u);
                ctx.SaveChanges();
            
        }
        public Account Login( Account c)
        {
            Account files = null;
            
                files = ctx.users.Where(a => a.u_email== c.Email && a.u_password==c.Password).Select(a => new Account()
                {
                    id = a.u_id,
                    Full_Name = a.u_name,
                    Email = a.u_email,
                    Address = a.u_address,
                    Address_Office=a.u_address_office,
                    Contact = a.u_contact,
                    Password = a.u_password,
                    User_Type = a.u_type,
                    Gender = a.u_gender,
                    Register_Date= (DateTime)a.i_date,
                    Account_img = (byte[])a.u_img,
                    type_img = a.u_img_type
                }).SingleOrDefault();
            
            return files;
        }
        public void delete_Account(int id)
        {
           
                var u = ctx.users.Where(s => s.u_id == id).FirstOrDefault();
                ctx.Entry(u).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            
        }
        public Account update_Account(int id)
        {
            Account files = null;

           
                files = ctx.users.Where(a => a.u_id == id).Select(a => new Account()
                {
                    id = a.u_id,
                    Full_Name = a.u_name,
                    Email = a.u_email,
                    Address = a.u_address,
                    Address_Office = a.u_address_office,
                    Contact = a.u_contact,
                    Password = a.u_password,
                    User_Type = a.u_type,
                    Gender = a.u_gender,
                    Account_img = a.u_img,
                    type_img = a.u_img_type

                }).SingleOrDefault();
            
            return files;
        }
        public List<Account> GetDeliveryBoyInfo()
        {
            List<Account> files = null;


            files = ctx.users.Where(a => a.u_type == "DeliveryBoy").Select(a => new Account()
            {
                id = a.u_id,
                Full_Name = a.u_name,
                Email = a.u_email,
                Address = a.u_address,
                Address_Office = a.u_address_office,
                Contact = a.u_contact,
                Password = a.u_password,
                User_Type = a.u_type,
                Gender = a.u_gender,
                Account_img = a.u_img,
                type_img = a.u_img_type

            }).ToList<Account>();

            return files;
        }
        public List<Account> GetDeliveryBoyInfo(int id)
        {
            List<Account> files = null;


            files = ctx.users.Where(a => a.u_type == "DeliveryBoy" && a.u_id!=id).Select(a => new Account()
            {
                id = a.u_id,
                Full_Name = a.u_name,
                Email = a.u_email,
                Address = a.u_address,
                Address_Office = a.u_address_office,
                Contact = a.u_contact,
                Password = a.u_password,
                User_Type = a.u_type,
                Gender = a.u_gender,
                Account_img = a.u_img,
                type_img = a.u_img_type

            }).ToList<Account>();

            return files;
        }
        public void update_Account(Account a, HttpPostedFileBase PostedFile)
        {
            
              var res = ctx.users.SingleOrDefault(b => b.u_id == a.id);
                if (res != null)
                {
                    res.u_name = a.Full_Name;
                    res.u_email = a.Email;
                    res.u_address = a.Address;
                    res.u_address_office = a.Address_Office;
                    res.u_contact = a.Contact;
                    res.u_password = a.Password;
                    res.u_type = a.User_Type;
                    res.u_gender = a.Gender;
                    ctx.SaveChanges();
                }
            

        }
        public void update_Picture(HttpPostedFileBase PostedFile,int id)
        {
            byte[] bytes;
            BinaryReader br = new BinaryReader(PostedFile.InputStream);
            bytes = br.ReadBytes(PostedFile.ContentLength);
            string type = PostedFile.ContentType;
           
                var res = ctx.users.SingleOrDefault(b => b.u_id == id);
                if (res != null)
                {
                    res.u_img = bytes;
                    res.u_img_type = type;
                    ctx.SaveChanges();
                }
            

        }
       
        public List<Account> dispaly_Account()
        {
            List<Account> files = null;

           
                files = ctx.users.Select(a => new Account()
                {
                    id = a.u_id,
                    Full_Name = a.u_name,
                    Email = a.u_email,
                    Address = a.u_address,
                    Address_Office=a.u_address_office,
                    Contact = a.u_contact,
                    Password = a.u_password,
                    User_Type = a.u_type,
                    Gender = a.u_gender,
                    Account_img = a.u_img,
                    type_img = a.u_img_type,
                    Register_Date= (DateTime)a.i_date

                }).ToList<Account>();
            
            return files;
        }
        public List<SelectListItem> AddressDropDownList(int id)
        {
            Account files = update_Account(id);
            List<SelectListItem> address = new List<SelectListItem>();
            address.Add(new SelectListItem { Value = files.Address.ToString(), Text = files.Address.ToString() });
            address.Add(new SelectListItem { Value = files.Address_Office.ToString(), Text = files.Address_Office.ToString() });
            return address;
        }
        public List<SelectListItem> DeliveryBoyDropDownList()
        {
            List<Account> files = GetDeliveryBoyInfo();
            List<SelectListItem> address = new List<SelectListItem>();
            foreach (var item in files)
            {
                address.Add(new SelectListItem { Value = item.id.ToString(), Text = item.Full_Name.ToString() });
            }
            return address;
        }
        public List<SelectListItem> DeliveryBoyDropDownList(int id)
        {
            List<Account> files = GetDeliveryBoyInfo(id);
            List<SelectListItem> address = new List<SelectListItem>();
            foreach (var item in files)
            {
                address.Add(new SelectListItem { Value = item.id.ToString(), Text = item.Full_Name.ToString() });
            }
            return address;
        }
    }
}
public enum user_type { Customer, DeliveryBoy};