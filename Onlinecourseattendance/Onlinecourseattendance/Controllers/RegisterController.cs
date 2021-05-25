using Onlinecourseattendance.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineLearning.Controllers
{
    public class RegisterController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {

            return View();
        }

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AccountManager user)

        {
            if (ModelState.IsValid)
            {

                using (var context = new onlinelearningcontext())
                {
                    AccountManager r = new AccountManager();
                    r.FirstName = user.FirstName;
                    r.MiddleName = user.MiddleName;
                    r.LastName = user.LastName;
                    r.Email = user.Email;

                    //Encrypt Password

                    //r.Password = OnlineLearning.Models.encryptPassword.textToEncrypt(user.Password);
                        byte[] b;
                        b = Convert.FromBase64String(user.Password);
                        r.Password = System.Text.ASCIIEncoding.ASCII.GetString(b);
                    
                    r.Role = "Student";
                    r.ProcessingDate = DateTime.UtcNow;
                    r.LastChanged = DateTime.UtcNow;
                    //Email exists or not
                    var emailcheck = context.AccountManagerset.FirstOrDefault(s => s.Email == user.Email);
                    context.AccountManagerset.Add(r);
                    if (emailcheck == null)
                    {
                        context.SaveChanges();
                     
                    }
                    else
                    {
                        return View("Register");
                    }

                }
                ViewBag.Message = "User Details Saved";
                return View("Register");
            }
            else
            {

                return View("Register");
            }
        }

      

        
    }

}
