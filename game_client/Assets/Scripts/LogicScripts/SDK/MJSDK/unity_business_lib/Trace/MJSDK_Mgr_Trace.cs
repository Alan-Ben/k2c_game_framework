using System;
using System.Collections;
using UnityEngine;

namespace MJSDK_Package
{
    public class MJSDK_Mgr_Trace
    {
        /// <summary>
        /// 自定义埋点事件（第三方）- 全部平台
        /// </summary>
        /// <param name="eventValue">数据</param>
        /// <param name="_sucDelegate">成功回执</param>
        /// <param name="_failDelegate">失败回执</param>
        public static void trace_customEvent_allThird(
        MJSDK_2SDK_ThirdParty_customEvent_base _eventValue,
        Action<E_MJSDK_Third_Trace_Type, string> _sucDelegate,
        Action<int, string> _failDelegate)
        {
            //循环E_MJSDK_Third_Trace_Type 枚举
            foreach (E_MJSDK_Third_Trace_Type type in Enum.GetValues(typeof(E_MJSDK_Third_Trace_Type)))
            {
                //跳过all
                if (type == E_MJSDK_Third_Trace_Type.all)
                {
                    continue;
                }
                //调用单个事件
                trace_customEvent(type, _eventValue, _sucDelegate, _failDelegate);
            }
        }


        /// <summary>
        /// 自定义埋点事件（第三方）
        /// </summary>
        /// <param name="type">平台</param>
        /// <param name="eventValue">数据</param>
        /// <param name="_sucDelegate">成功回执</param>
        /// <param name="_failDelegate">失败回执</param>
        public static void trace_customEvent(
            E_MJSDK_Third_Trace_Type type,
            MJSDK_2SDK_ThirdParty_customEvent_base _eventValue,
        Action<E_MJSDK_Third_Trace_Type, string> _sucDelegate,
        Action<int, string> _failDelegate)
        {
            //设置参数
            Hashtable ht = new Hashtable();
            ht.Add("eventValue", JsonUtility.ToJson(_eventValue));
            ht.Add("mainOrder", type.ToString());
            MJSDK.sendMsgToPhonePlatform("mj", "thirdCustomEvent", ht.toJson(),
            new MJSDK_CallbackDealer_Action(_resultValue =>
            {
                if (_sucDelegate != null)
                {
                    _sucDelegate(type, _resultValue as string);
                }
            }, _failDelegate));
        }


        /// <summary>
        /// 购买事件（第三方）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="_eventValue"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void trace_purchaseEvent(
            MJSDK_2SDK_ThirdParty_purchaseEvent_base _eventValue,
            Action<E_MJSDK_Third_Trace_Type, string> _sucDelegate,
            Action<int, string> _failDelegate,
            E_MJSDK_Third_Trace_Type type = E_MJSDK_Third_Trace_Type.appsflyer)
        {
            //设置参数
            Hashtable ht = new Hashtable();
            ht.Add("eventValue", JsonUtility.ToJson(_eventValue));
            ht.Add("mainOrder", type.ToString());
            MJSDK.sendMsgToPhonePlatform("mj", "thirdPurchaseEvent", ht.toJson(),
            new MJSDK_CallbackDealer_Action(_resultValue =>
            {
                if (_sucDelegate != null)
                {
                    _sucDelegate(type, _resultValue as string);
                }
            }, _failDelegate));
        }
    }
}
