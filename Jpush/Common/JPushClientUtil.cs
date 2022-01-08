using Jiguang.JPush;
using Jiguang.JPush.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Jpush.Common
{
    /// <summary>
    /// 极光推送工具类
    /// </summary>
    public class JPushClientUtil
    {
        private const string appKey = "youAppKey";
        private const string masterSecret = "youMasterSecret";
        private static JPushClient client = new JPushClient(appKey, masterSecret);

        /// <summary>
        /// 单个设备注册ID推送
        /// </summary>
        /// <param name="title">推送标题（Android才会存在）</param>
        /// <param name="noticeContent">通知内容</param>
        /// <param name="registrationid">设备注册ID(registration_id)</param>
        /// <param name="extrasParam">拓展参数(传入App接收的一些参数标识)</param>
        /// <param name="isApnsProduction">注意：iOS是否推送生产环境（true是，false否推开发环境）</param>
        /// <returns></returns>
        public bool SendPushByRegistrationId(string title, string noticeContent, string registrationid, Dictionary<string, object> extrasParam = null, bool isApnsProduction = true)
        {
            //设备标识参数拼接
            var pushRegistrationId = new RegistrationIdList();
            pushRegistrationId.registration_id.Add(registrationid);

            return JPushBaseSendMessage(title, noticeContent, isApnsProduction, pushRegistrationId, extrasParam);
        }

        /// <summary>
        /// 设备注册ID批量推送（一次推送最多1000个）
        /// </summary>
        /// <param name="title">推送标题（Android才会存在）</param>
        /// <param name="noticeContent">通知内容</param>
        /// <param name="registrationIds">注册ID(registration_id)列表,一次推送最多1000个</param>
        /// <param name="extrasParam">拓展参数(传入App接收的一些参数标识)</param>
        /// <param name="isApnsProduction">注意：iOS是否推送生产环境（true是，false否推开发环境）</param>
        /// <returns></returns>
        public bool SendPushByRegistrationIdList(string title, string noticeContent, List<string> registrationIds, Dictionary<string, object> extrasParam = null, bool isApnsProduction = true)
        {
            //设备标识参数拼接
            var pushRegistrationId = new RegistrationIdList();
            pushRegistrationId.registration_id.AddRange(registrationIds);

            return JPushBaseSendMessage(title, noticeContent, isApnsProduction, pushRegistrationId, extrasParam);
        }

        /// <summary>
        /// 广播推送
        /// </summary>
        /// <param name="title">推送标题（Android才会存在）</param>
        /// <param name="noticeContent">通知内容</param>
        /// <param name="extrasParam">拓展参数(传入App接收的一些参数标识)</param>
        /// <param name="isApnsProduction">注意：iOS是否推送生产环境（true是，false否推开发环境）</param>
        /// <returns></returns>
        public bool BroadcastPush(string title, string noticeContent, Dictionary<string, object> extrasParam = null, bool isApnsProduction = true)
        {
            return JPushBaseSendMessage(title, noticeContent, isApnsProduction, null, extrasParam, true);
        }

        /// <summary>
        /// 极光消息推送公共方法
        /// </summary>
        /// <param name="title">推送标题（Android才会存在）</param>
        /// <param name="noticeContent">通知内容</param>
        /// <param name="pushRegistrationId">设备注册ID(registration_id)</param>
        /// <param name="isApnsProduction">iOS是否推送生产环境（true是，false否推开发环境）</param>
        /// <param name="extrasParam">拓展参数</param>
        /// <param name="isRadioBroadcast">是否广播</param>
        /// <returns></returns>
        private bool JPushBaseSendMessage(string title, string noticeContent, bool isApnsProduction, RegistrationIdList pushRegistrationId, Dictionary<string, object> extrasParam, bool isRadioBroadcast = false)
        {
            try
            {
                object audience = pushRegistrationId;

                if (isRadioBroadcast)
                {
                    audience = "all";
                }

                var pushPayload = new PushPayload()
                {
                    Platform = new List<string> { "android", "ios" },//推送平台设置
                    Audience = audience,//推送目标
                    //notifacation：通知内容体。是被推送到客户端的内容。与 message 一起二者必须有其一，可以二者并存。
                    Notification = new Notification
                    {
                        Alert = noticeContent,//通知内容
                        Android = new Android
                        {
                            Alert = noticeContent,//通知内容
                            Title = title,//通知标题
                            URIActivity = "com.king.sysclearning.platform.app.JPushOpenClickActivity",//该字段用于指定开发者想要打开的 activity，值为 activity 节点的 “android:name”属性值;适配华为、小米、vivo厂商通道跳转
                            URIAction = "com.king.sysclearning.platform.app.JPushOpenClickActivity",//该字段用于指定开发者想要打开的 activity，值为 "activity"-"intent-filter"-"action" 节点的 "android:name" 属性值;适配 oppo、fcm跳转
                            Extras = extrasParam //这里自定义JSON格式的Key/Value信息，以供业务使用。
                        },
                        IOS = new IOS
                        {
                            Alert = noticeContent,
                            Badge = "+1",//此项是指定此推送的badge自动加1
                            Extras = extrasParam //这里自定义JSON格式的Key/Value信息，以供业务使用。
                        }
                    },
                    Options = new Options//可选参数
                    {
                        //iOS 环境不一致问题：API 推送消息给 iOS，需要设置 apns_production 指定推送的环境，false 为开发，true 为生产。
                        IsApnsProduction = isApnsProduction// 设置 iOS 推送生产环境。不设置默认为开发环境。
                    }
                };

                var response = client.SendPush(pushPayload);
                //200一定是正确。所有异常都不使用 200 返回码
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public class RegistrationIdList
    {
        /// <summary>
        /// 设备注册ID
        /// </summary>
        public List<string> registration_id { get; set; } = new List<string>();
    }
}