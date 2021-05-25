using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Onlinecourseattendance.Controllers
{
    public class MainPageController : Controller
    {
        // GET: MainPage
        public ActionResult Index()
        {
            //string username = Request.Cookies["UserName"].Value;
            string username = string.Empty;
            //OR
            HttpCookie reqCookies = Request.Cookies["UserInfo"];  
            if(reqCookies!=null)
            {
                username = reqCookies["UserName"].ToString();
            }
            ViewBag.Name = username;
            return View();
        }
    }
}