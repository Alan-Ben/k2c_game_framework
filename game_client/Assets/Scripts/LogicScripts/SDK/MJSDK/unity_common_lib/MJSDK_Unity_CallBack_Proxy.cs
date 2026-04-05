using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 代理回执（Android）
    /// </summary>
    public class MJSDK_Unity_CallBack_Proxy
    {
        //静态对象
        private static MJSDK_Unity_CallBack_Proxy _g_instance;
        //android 对象
        private static AndroidJavaObject _g_jo;
        //是否初始化
        private static bool _g_initStatus;
        /// <summary>
        /// 获取静态对象
        /// </summary>
        /// <returns></returns>
        public static MJSDK_Unity_CallBack_Proxy getInstance()
        {
            if (_g_instance == null)
            {
                _g_instance = new MJSDK_Unity_CallBack_Proxy();
            }

            return _g_instance;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void initData()
        {
            if (_g_initStatus)
            {
                return;
            }

            try
            {
                //创建安卓端入口
                _g_jo = new AndroidJavaObject("com.mechanist.mjsdk_unity.MJSDK_Unity_CallBack_Proxy");
                setAndrodCallback_Proxy _setAndrodCallback = new setAndrodCallback_Proxy();

                if (_g_jo != null)
                {
                    _g_initStatus = true;
                    //注册回调
                    _g_jo.Call("registerCallBack", _setAndrodCallback);
                }
                else
                {
                    Debug.LogError("MJSDK_Unity_CallBack_Proxy 初始化失败");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("MJSDK_Unity_CallBack_Proxy 初始化异常 ex:" + ex.Message);
            }
        }

        /// <summary>
        /// android代理方式回执
        /// </summary>
        public class setAndrodCallback_Proxy : AndroidJavaProxy
        {
            public setAndrodCallback_Proxy() : base("com.mechanist.sdk_common_lib.MJSDK_Common._IMJSDK_CallBack_Proxy") { }

            /****************
             * 平台SDK错误回执（系统错误）
             * 一般用于MJSDK主动向引擎发起消息(错误)
             * @param _msg 消息值
             */
            public void onMJSDKError(string _msg) {
                MJSDK_Log.mjsdkLog( "代理方式收到信息  onMJSDKError" + _msg);
                MJSDK_MainMono.dealMJSDKError(_msg);
            }


            /****************
             * 平台SDK消息通知
             * 一般用于MJSDK主动向引擎发起消息
             * @param _msg 消息值
             */
            public void onMJSDKMsg(string _msg) {
                MJSDK_Log.mjsdkLog( "代理方式收到信息  onMJSDKMsg" + _msg);
                MJSDK_MainMono.dealMJSDKMsg(_msg);
            }

            /****************
             * 平台SDK协议处理成功回执
             * 一般用于MJSDK处理协议成功回执
             * @param _msg 消息值
             */
            public void onSDKDealDone(string _msg) {
                MJSDK_Log.mjsdkLog( "代理方式收到信息  onSDKDealDone" + _msg);
                MJSDK_MainMono.dealSDKDealMsg(_msg);
            }


            /****************
             * 平台SDK协议失败回执
             * 一般用于MJSDK处理协议失败回执
             * @param _msg 消息值
             */
            public void onSDKDealFail(string _msg) {
                MJSDK_Log.mjsdkLog( "代理方式收到信息  onSDKDealFail" + _msg);
                MJSDK_MainMono.dealSDKDealMsg(_msg);
            }
        }
    }
}
