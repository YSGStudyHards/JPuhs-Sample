using Jiguang.JPush;
using Jiguang.JPush.Model;
using Jpush.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jpush.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var registrationIdList = new List<RegistrationIdList>();
            registrationIdList.Add(new RegistrationIdList() { registration_id="123456789"});

            new JPushManageController().JPushSendMessage("hello 极光","hello",registrationIdList);
            return View();
        }
    }
}
