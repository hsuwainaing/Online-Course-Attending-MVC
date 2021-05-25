using Newtonsoft.Json;
using Onlinecourseattendance.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Onlinecourseattendance.Controllers
{
    public class LoginController : Controller
    {
        private string ClientId=ConfigurationManager.AppSettings["Google.ClientID"];
        private string SecretKey = ConfigurationManager.AppSettings["Google.SecretKey"];
        private string RedirectUrl=ConfigurationManager.AppSettings["Google.RedirectUrl"];
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        //POST: Login
        public ActionResult Login(AccountManager m)
        {
            onlinelearningcontext context = new onlinelearningcontext();
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(m.Password);
            string encrypted = Convert.ToBase64String(b);
            var result = context.AccountManagerset.Where(a => a.Email == m.Email && a.Password == encrypted).ToList();
            HttpCookie UserInfo = new HttpCookie("UserInfo");
            UserInfo["UserName"] = result.Select(a => a.FirstName).FirstOrDefault();
            UserInfo["Role"] = result.Select(a => a.Role).FirstOrDefault();
            UserInfo.Expires.Add(new TimeSpan(0, 1, 0));
            Response.Cookies.Add(UserInfo);
            if(result.Count()!=0)
            {
                return RedirectToAction("Index", "MainPage");
            }
            else
            {
                return View();

            }
        }


        //Login with Google
        public void LoginWithMail()
        {
           
            Response.Redirect($"https://accounts.google.com/o/oauth2/v2/auth?client_id={ClientId}&response_type=code&scope=openid%20email%20profile&redirect_uri={RedirectUrl}&state=abcdef");
            //return null; 
        }
  
        //Forgot email
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(AccountManager m)
        {
            string Username = ConfigurationManager.AppSettings["UserName"];
            string Password = ConfigurationManager.AppSettings["Password"];
            string SmtpPort = ConfigurationManager.AppSettings["Smtport"];
            string Host = ConfigurationManager.AppSettings["Host"];
            if (ModelState.IsValid)
            {

                onlinelearningcontext context = new onlinelearningcontext();

                var result = context.AccountManagerset.Where(a => a.Email == m.Email).FirstOrDefault();
                if (result != null)
                {
                    int token = GenerateToken(result.UserId);
               
                    var lnkHref = @"<a href='" + Url.Action("ResetPassword", "Login", new { UserId = result.UserId, code = token }, "http") + "'>Reset Password</a>";
                    //HTML Template for Send email
                    string subject = "Your changed password";
                    string body = @"<b>Please find the Password Reset Link. </b><br/>" + lnkHref + "<b>Confirmation Code:</b>" + token;
                    body=  "<html>" + body + "</html>";
                    SendEmail(m.Email, subject, body, Username, Password, SmtpPort, Host);
                    //return RedirectToAction("ResetPassword",new { UserId = result.UserId, code=token });
                    ViewBag.Message = "Please Check Your Email!";
                    return View();
                }
                else
                {
                    ViewBag.Message = "Your Account is not registered! Please Try Again!";
                    
                }
            }
            else
            {
                ViewBag.Message = "Your Account is Invalid!  Please Try Again!";
                
            }
            return View();
        }

        //send email reset link
        public static void SendEmail(string To,string subject,string body,string from,string password,string smtp,string host)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(To);
            mail.From = new MailAddress(from);
            mail.Subject = subject;
            mail.Body = body;
            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(from, password);
                client.Host = host;
                client.Port = Convert.ToInt32(smtp);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(mail);
            }
         

        }

        //ResetPassword
        public ActionResult ResetPassword(int UserId, int code)
        {
            ForgotPassword newpwd = new ForgotPassword();
            newpwd.UserId = UserId;
            newpwd.Token = code;
            ViewBag.UserId = UserId;
            return View();

        }

        [HttpPost]
        public ActionResult ResetPassword(string pwd, int? UserId,int? code)
        {
            ForgotPassword newpwd = new ForgotPassword();
            onlinelearningcontext context = new onlinelearningcontext();
          //check whethere the code is latest one
            var codetesting = context.ForgotPasswordset.Where(a => a.UserId == UserId).OrderByDescending(a => a.ProcessingTime).FirstOrDefault();
            if(code==codetesting.Token)
            {
                //for code expire, if don't type within two minutes, code will be expired
                if(codetesting.ProcessingTime<DateTime.Now.AddMinutes(-2))
                {
                    ViewBag.Message = "Your Confirmation Code is Expired! Please Reset Again!";
                    return View();
                }
                //save new password to db
                else
                {
                    //encrypt password first
                    byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(pwd);
                    string encrypted = Convert.ToBase64String(b);
                    var user = context.AccountManagerset.Find(UserId);
                    user.Password = encrypted;
                    user.LastChanged = DateTime.Now;
                    context.SaveChanges();
                    return RedirectToAction("Index", "MainPage");
                }

            }
            else
            {
                //string ErrorCode = "Your Confirmation Code is Wrong! Please Try Again!";
                return Json("Your Confirmation Code is Wrong! Please Try Again!");
            }
           
        }

        public int GenerateToken(int userid)
        {
            Random _random = new Random();
            int code= _random.Next(0, 10000000);
            onlinelearningcontext context = new onlinelearningcontext();
            ForgotPassword newcode = new ForgotPassword();
            newcode.UserId = userid;
            newcode.Token = code;
            newcode.ProcessingTime = DateTime.Now;
            context.ForgotPasswordset.Add(newcode);
            //context.SaveChanges();
            return code;
        }
    }
}