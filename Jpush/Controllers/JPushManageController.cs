using Jiguang.JPush;
using Jiguang.JPush.Model;
using Jpush.Common;
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

        private readonly JPushClientUtil _jPushClientUtil;

        public JPushManageController(JPushClientUtil jPushClientUtil)
        { 
          this._jPushClientUtil=jPushClientUtil;
        }


        /// <summary>
        /// 单个设备注册ID推送
        /// </summary>
        /// <returns></returns>
        public ActionResult SendPushByRegistrationId()
        {
            var isOk = _jPushClientUtil.SendPushByRegistrationId("追逐时光者欢迎你！", "2022新年快乐", "1507bfd3f715abecfa4", new Dictionary<string, object>(), true);

            return Json(new { result = isOk });
        }


        /// <summary>
        /// 设备注册ID批量推送（一次推送最多1000个）
        /// </summary>
        /// <returns></returns>
        public ActionResult SendPushByRegistrationIdList()
        {
            var registrationIds = new List<string>() { "1507bfd3f715abecfa455", "1507bfd3f715abecfa433", "1507bfd3f715abecfa422" };
            var isOk = _jPushClientUtil.SendPushByRegistrationIdList("追逐时光者欢迎你！", "2022新年快乐", registrationIds, new Dictionary<string, object>(), true);

            return Json(new { result = isOk });
        }


        /// <summary>
        /// 广播推送
        /// </summary>
        /// <returns></returns>
        public ActionResult BroadcastPush()
        {
            var isOk = _jPushClientUtil.BroadcastPush("追逐时光者欢迎你！", "2022新年快乐", new Dictionary<string, object>(), true);

            return Json(new { result = isOk });
        }

    }
}