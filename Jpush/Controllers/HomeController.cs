using Jiguang.JPush;
using Jiguang.JPush.Model;
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
            JPushSendMessage();
            return View();
        }

        private static JPushClient client = new JPushClient("a49571009ca3c76be8c91a9e", "e3b6b538e7a94005393e025a");

        public ActionResult JPushSendMessage()
        {
            PushPayload pushPayload = new PushPayload()
            {
                Platform = new List<string> { "android", "ios" },
                Audience =new List<string> {"334342432","23432423423" },//推送目標

        Notification = new Notification
                {
                    Alert = "hello jpush",
                    Android = new Android
                    {
                        Alert = "android alert",
                        Title = "title"
                    },
                    IOS = new IOS
                    {
                        Alert = "ios alert",
                        Badge = "+1"
                    }
                },
                Message = new Message
                {
                    Title = "message title",
                    Content = "message content",
                    Extras = new Dictionary<string, string>
                    {
                        ["key1"] = "value1"
                    }
                },
                Options = new Options
                {
                    IsApnsProduction = true // 设置 iOS 推送生产环境。不设置默认为开发环境。
                }
            };
            var response = client.SendPush(pushPayload);
            Console.WriteLine(response.Content);

            return View(new {code=0,msg= response.Content });
        }

    }
}
