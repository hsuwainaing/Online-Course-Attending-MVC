using Onlinecourseattendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Onlinecourseattendance.Controllers
{
    public class UserInfoController : Controller
    {
        // GET: UserInfo
        public ActionResult UserInfo()
        {
            using (var context = new onlinelearningcontext())
            {
                return View(context.AccountManagerset.ToList());
            }
               
        }
        public ActionResult UserForm(int userid)
        {
            ViewBag.userid = userid;
            return View();
        }
        //POST: User Info
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserForm(int userid,UserInfo user)

        {
            
                using (var context = new onlinelearningcontext())
                {
                    var r = context.UserInfoset.FirstOrDefault(x => x.UserID == userid);

                    // Checking if any such record exist 
                    if (r != null)
                    {

                        r.UserName = user.UserName;
                        r.PhoneNo = user.PhoneNo;
                        r.Address = user.Address;
                        r.NRICNo = user.NRICNo;
                        r.PassportNo = user.PassportNo;
                        r.Gender = user.Gender;
                        r.Country = user.Country;
                        r.Subject = user.Subject;
                        r.Level = user.Level;
                        r.ProcessingDate = DateTime.UtcNow;
                        context.SaveChanges();
                        return RedirectToAction("ViewProfile", "UserInfo", new { @userid = userid });
                    }
                    else
                    {
                    UserInfo a = new UserInfo();
                        a.UserID = userid;
                        a.UserName = user.UserName;
                        a.PhoneNo = user.PhoneNo;
                        a.Address = user.Address;
                        a.NRICNo = user.NRICNo;
                        a.PassportNo = user.PassportNo;
                        a.Gender = user.Gender;
                        a.Country = user.Country;
                        a.Subject = user.Subject;
                        a.Level = user.Level;
                        a.ProcessingDate = DateTime.UtcNow;
                        context.UserInfoset.Add(a);
                        context.SaveChanges();
                        return RedirectToAction("ViewProfile", "UserInfo", new { @userid=userid });
                    }

                }
         
        }

        public ActionResult CreateRole(int userid)
        {
            ViewBag.userid = userid;
            return View();
        }
        public ActionResult ViewProfile(int userid)
        {
            using (var context = new onlinelearningcontext())
            {
                var res = context.UserInfoset.Where(e => e.UserID == userid).ToList();
                
                return View(res);
            }
        }
        public ActionResult DeleteUser(int userid)
        {
            using (var context = new onlinelearningcontext())
            {
                var data = context.AccountManagerset.SingleOrDefault(e => e.UserId == userid);
                context.AccountManagerset.Remove(data);

                var r = context.UserInfoset.SingleOrDefault(e => e.UserID == userid);
                if(r!=null)
                {
                    context.UserInfoset.Remove(r);
                  
                }
                context.SaveChanges();
                return RedirectToAction("UserInfo", "UserInfo");
            }
        }
    }
}