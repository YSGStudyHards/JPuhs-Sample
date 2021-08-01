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
    /// <summary>
    /// 极光推送管理
    /// </summary>
    public class JPushManageController : Controller
    {

        private static JPushClient client = new JPushClient("appKey", "masterSecret");

        /// <summary>
        /// 极光消息推送
        /// 功能需要
        /// 详细文档地址：https://docs.jiguang.cn/jpush/server/push/rest_api_v3_push/
        /// </summary>
        /// <param name="noticeContent">通知内容</param>
        /// <param name="title">推送标题（Android才会存在）</param>
        /// <param name="registrationIdList">注册ID(registration_id)列表</param>
        /// <param name="pushPlatform">推送平台（0 Android，1 Ios，2 Android&Ios）</param>
        /// <returns></returns>
        public ActionResult JPushSendMessage(string noticeContent, string title, List<RegistrationIdList> registrationIdList, int pushPlatform = 2)
        {
            PushPayload pushPayload = new PushPayload()
            {
                Platform = GetPushPlatform(pushPlatform),//推送平台设置

                Audience = registrationIdList,//推送目标（注意，我这里只对注册ID(registration_id)进行推送，对一台或者多台设备列表进行推送）
                //推送设备对象，表示一条推送可以被推送到哪些设备列表。确认推送设备对象，JPush 提供了多种方式，比如：别名、标签、注册ID、分群、广播等。

                //NOtifacation：通知内容体。是被推送到客户端的内容。与 message 一起二者必须有其一，可以二者并存。
                Notification = new Notification
                {
                    //这个位置的 "alert" 属性（直接在 notification 对象下），是一个快捷定义，各平台的 alert 信息如果都一样，则可不定义。如果各平台有定义，则覆盖这里的定义。
                    Alert = noticeContent,
                    Android = new Android
                    {
                        //Alert = "android alert",//通知内容
                        Title = "title"//通知标题
                    },
                    IOS = new IOS
                    {
                        //Alert = "ios alert",
                        Badge = "+1"
                    }
                },
                Options = new Options//可选参数
                {
                    IsApnsProduction = true // 设置 iOS 推送生产环境。不设置默认为开发环境。
                }
            };

            //200 一定是正确。所有异常都不使用 200 返回码
            var response = client.SendPush(pushPayload);
            Console.WriteLine(response.Content);

            return View(new { code = 0, msg = response.Content });
        }



        /// <summary>
        /// 获取推送平台标识
        /// </summary>
        /// <param name="pushPlatform">推送平台（0 Android，1 Ios，2 Android&Ios）</param>
        /// <returns></returns>
        public List<string> GetPushPlatform(int pushPlatform)
        {
            switch (pushPlatform)
            {
                case 0:
                    return new List<string> { "android" };
                case 1:
                    return new List<string> { "ios" };
                case 2:
                    return new List<string> { "android", "ios" };
                default:
                    return new List<string> { "android", "ios" };
            }
        }
    }
}